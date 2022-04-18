using GalaSoft.MvvmLight.CommandWpf;
using Quality_Control.Commons;
using Quality_Control.Forms.Navigation;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private readonly double _startLeftPosition = 32;
        private readonly WindowData _windowData = WindowSettings.Read();
        private readonly QualityService _service = new QualityService();
        private QualityDataMV _qualityDataMV;
        private NavigationMV _navigationMV;
        private int _selectedIndex;
        private string _remarks;
        private string _productName = "";
        private string _productNumber = "";
        private int _year = DateTime.Today.Year;
        private DateTime _productionDate;
        public event PropertyChangedEventHandler PropertyChanged;

        public SortableObservableCollection<QualityModel> FullQuality { get; private set; }
        public SortableObservableCollection<QualityModel> Quality { get; private set; }
        public List<int> Years { get; private set; }
        public RelayCommand<CancelEventArgs> OnClosingCommand { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNumberFilterTextChanged { get; set; }
        public RelayCommand<SelectionChangedEventArgs> OnComboYearSelectionChanged { get; set; }


        public QualityMV()
        {
            FullQuality = _service.GetAllQuality(DateTime.Today.Year);
            Quality = FullQuality;
            Years = _service.GetAllYears();
            OnClosingCommand = new RelayCommand<CancelEventArgs>(this.OnClosingCommandExecuted);
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductNumberFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNumberTextChangedFilter);
            OnComboYearSelectionChanged = new RelayCommand<SelectionChangedEventArgs>(OnYearSelectionCommandExecuted);
        }

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
            FullQuality = _service.GetAllQuality(Year);
            Quality = FullQuality;
            Filter();
        }

        internal void SetQualityDataMV(QualityDataMV qualityDataMV)
        {
            _qualityDataMV = qualityDataMV;
        }

        internal NavigationMV SetNavigationMV
        {
            set => _navigationMV = value;
        }

        internal bool Modified => (Quality.Any(x => x.Modified) || _qualityDataMV != null) && _qualityDataMV.Modified;

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

                var result = FullQuality
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

        #region Current

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

        public bool IsFilterOn => ProductName.Length > 0 || ProductNumber.Length > 0;

        public bool IsAnyQuality => Quality.Count > 0;

        internal QualityModel GetCurrentQuality => Quality[_selectedIndex];

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
            }
        }

        public int GetRowCount => Quality.Count;

        public void Refresh()
        {
            _navigationMV.Refresh();
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
            get => _year;
            set => _year = value;
        }

        #endregion

        public Thickness TxtNumberLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);

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

        public void SaveAll()
        {

            _qualityDataMV.Save();
        }

        public void DeleteAll()
        {
            
        }
    }
}
