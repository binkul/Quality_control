using GalaSoft.MvvmLight.CommandWpf;
using Quality_Control.Commons;
using Quality_Control.Forms.AddNew;
using Quality_Control.Forms.Navigation;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityMV : INotifyPropertyChanged, INavigation
    {
        private ICommand _saveButton;
        private ICommand _deleteButton;
        private ICommand _addNewButton;

        private readonly double _startLeftPosition = 32;
        private readonly WindowData _windowData = WindowSettings.Read();
        private readonly QualityService _service = new QualityService();
        private QualityDataMV _qualityDataMV;
        private NavigationMV _navigationMV;
        private QualityModel _actualRow;
        private int _selectedIndex;
        private string _remarks;
        private string _productName = "";
        private string _productNumber = "";
        private DateTime _productionDate;
        public event PropertyChangedEventHandler PropertyChanged;

        public SortableObservableCollection<QualityModel> FullQuality { get; private set; }
        public SortableObservableCollection<QualityModel> Quality { get; private set; }
        public RelayCommand<CancelEventArgs> OnClosingCommand { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNumberFilterTextChanged { get; set; }
        public RelayCommand<SelectionChangedEventArgs> OnComboYearSelectionChanged { get; set; }


        public QualityMV()
        {
            FullQuality = _service.GetAllQuality(DateTime.Today.Year);
            Quality = FullQuality;
            OnClosingCommand = new RelayCommand<CancelEventArgs>(this.OnClosingCommandExecuted);
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductNumberFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNumberTextChangedFilter);
            OnComboYearSelectionChanged = new RelayCommand<SelectionChangedEventArgs>(OnYearSelectionCommandExecuted);
        }

        internal void SetQualityDataMV(QualityDataMV qualityDataMV)
        {
            _qualityDataMV = qualityDataMV;
        }

        internal NavigationMV SetNavigationMV
        {
            set => _navigationMV = value;
        }

        internal bool Modified => ModifiedQuality || ModifiedData;

        private bool ModifiedQuality => Quality.Any(x => x.Modified);

        private bool ModifiedData
        {
            get
            {
                if (_qualityDataMV != null)
                    return _qualityDataMV.Modified;
                else
                    return false;
            }
        }

        #region Events - RelayCommand

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void OnClosingCommandExecuted(CancelEventArgs e)
        {
            MessageBoxResult ansver = MessageBoxResult.No;

            if (Modified)
            {
                ansver = MessageBox.Show("Dokonano zmian w rekordach. Czy zapisać zmiany?", "Zamis zmian", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }

            if (ansver == MessageBoxResult.Yes)
            {
                SaveAll();
                WindowSettings.Save(_windowData);
            }
            else if (ansver == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                WindowSettings.Save(_windowData);
            }
        }

        private void OnYearSelectionCommandExecuted(SelectionChangedEventArgs e)
        {
            SaveDataQuality();
            if (SaveQuality())
            {
                ReloadYears();
            }
            FullQuality = _service.GetAllQuality(Year);
            Quality = FullQuality;
            Filter();
        }

        #endregion

        #region Filtering

        public string ProductName
        { 
            get => _productName;
            set => _productName = value;
        }

        public string ProductNumber
        {
            get => _productNumber;
            set => _productNumber = value;
        }

        public void OnProductNameTextChangedFilter(TextChangedEventArgs e)
        {
            Filter();
        }

        public void OnProductNumberTextChangedFilter(TextChangedEventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(ProductName) || !string.IsNullOrEmpty(ProductNumber))
            {

                int number = ProductNumber.Length > 0 ? Convert.ToInt32(ProductNumber) : -1;

                List<QualityModel> result = FullQuality
                    .Where(x => x.ProductName.ToLower().Contains(ProductName))
                    .Where(x => x.Number >= number)
                    .ToList();

                SortableObservableCollection<QualityModel> newQuality = new SortableObservableCollection<QualityModel>();
                foreach (QualityModel model in result)
                {
                    newQuality.Add(model);
                }

                Quality = newQuality;
            }
            else
            {
                Quality = FullQuality;
            }
            DgRowIndex = 0;
            OnPropertyChanged(nameof(Quality));
        }

        #endregion

        #region Form dimension and position

        public Thickness TxtNumberLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);

        public double FormXpos
        {
            get => _windowData.FormXpos;
            set
            {
                _windowData.FormXpos = value;
                OnPropertyChanged(nameof(FormXpos));
            }
        }

        public double FormYpos
        {
            get => _windowData.FormYpos;
            set
            {
                _windowData.FormYpos = value;
                OnPropertyChanged(nameof(FormYpos));
            }
        }

        public double FormWidth
        {
            get => _windowData.FormWidth;
            set
            {
                _windowData.FormWidth = value;
                OnPropertyChanged(nameof(FormWidth));
            }
        }

        public double FormHeight
        {
            get => _windowData.FormHeight;
            set
            {
                _windowData.FormHeight = value;
                OnPropertyChanged(nameof(FormHeight));
            }
        }

        #endregion

        #region Current

        public bool IsFilterOn => ProductName.Length > 0 || ProductNumber.Length > 0;

        public bool IsAnyQuality => Quality.Count > 0;

        internal QualityModel GetCurrentQuality => Quality[_selectedIndex];

        public QualityModel ActualQuality
        {
            internal get => _actualRow;
            set => _actualRow = value;
        }

        public bool IsTextBoxActive { get; set; } = true;

        public string Remarks
        {
            get => _remarks;
            set
            {
                _remarks = value;
                Quality[_selectedIndex].Remarks = _remarks;
                Quality[_selectedIndex].Modified = true;
            }
        }

        public DateTime ProductionDate
        {
            get => _productionDate;
            set
            {
                _productionDate = value;
                Quality[_selectedIndex].ProductionDate = _productionDate;
                Quality[_selectedIndex].Modified = true;
            }
        }

        public int Year
        {
            get => _service.Year;
            set => _service.Year = value;
        }

        public List<int> Years
        {
            get => _service.Years;
        }

        #endregion

        #region Navigation

        public int DgRowIndex
        {
            get => _selectedIndex;
            set
            {
                QualityModel model = null;
                _selectedIndex = value;

                if (value >= 0 && Quality.Count != 0 && value < Quality.Count)
                {
                    model = Quality[_selectedIndex];
                    _remarks = model.Remarks;
                    _productionDate = model.ProductionDate;
                    IsTextBoxActive = true;
                }
                else
                {
                    _remarks = "";
                    _productionDate = DateTime.Today;
                    IsTextBoxActive = false;
                }
                OnPropertyChanged(nameof(DgRowIndex),
                    nameof(IsAnyQuality),
                    nameof(Remarks),
                    nameof(ProductionDate),
                    nameof(IsTextBoxActive));

                if (_qualityDataMV != null) _qualityDataMV.RefreshQualityData(model);
                Refresh();
            }
        }

        public int GetRowCount => Quality.Count;

        public void Refresh()
        {
            if (_navigationMV != null)
                _navigationMV.Refresh();
        }

        #endregion

        public ICommand SaveButton
        {
            get
            {
                if (_saveButton == null) _saveButton = new SaveButton(this);
                return _saveButton;
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                if (_deleteButton == null) _deleteButton = new DeleteButton(this);
                return _deleteButton;
            }
        }

        public ICommand AddNewButton
        {
            get
            {
                if (_addNewButton == null) _addNewButton = new AddNewButton(this);
                return _addNewButton;
            }
        }

        public bool SaveQuality()
        {
            if (!ModifiedQuality) return false;
            bool reload = false;

            List<QualityModel> qualities = FullQuality.Where(x => x.Modified == true).ToList();
            foreach (QualityModel quality in qualities)
            {
                if (_service.Update(quality))
                {
                    quality.Modified = false;
                    if (CheckQualityYear(quality)) reload = true;
                }
            }
            return reload;
        }

        public void SaveDataQuality()
        {
            if (ModifiedData)
                _qualityDataMV.Save();
        }

        public void SaveAll()
        {
            bool reload = SaveQuality();
            SaveDataQuality();

            _qualityDataMV.Save();
            if (reload) ReloadYears();
        }

        private void ReloadYears()
        {
            _service.ReloadYears();
            OnPropertyChanged(nameof(Years), nameof(Year));
        }

        private bool CheckQualityYear(QualityModel quality)
        {
            bool result = false;

            if (quality.ProductionDate.Year != Year)
            {
                _ = FullQuality.Remove(quality);
                _ = Quality.Remove(quality);
                if (FullQuality.Count == 0 || !Years.Contains(quality.ProductionDate.Year)) result = true;
            }

            return result;
        }

        public void DeleteAll()
        {
            if (ActualQuality == null) return;

            long id = ActualQuality.Id;
            if (_service.Delete(ActualQuality))
            {
                QualityModel quality = FullQuality.First(x => x.Id == id);
                _ = FullQuality.Remove(quality);
                _ = Quality.Remove(quality);
            }
        }

        public void AddNew()
        {
            AddNewForm form = new AddNewForm();
            _ = form.ShowDialog();
        }
    }
}
