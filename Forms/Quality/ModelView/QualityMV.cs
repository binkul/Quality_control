using GalaSoft.MvvmLight.CommandWpf;
using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityMV : INotifyPropertyChanged
    {
        private ICommand _saveButton;

        private readonly double _startLeftPosition = 32;
        private readonly WindowData _windowData = WindowSettings.Read();
        private readonly QualityService _service = new QualityService();
        private int _selectedIndex;
        private string _remarks;
        private QualityDataMV _qualityDataMV;
        private DateTime _productionDate;
        public event PropertyChangedEventHandler PropertyChanged;

        public SortableObservableCollection<QualityModel> Quality { get; }
        public RelayCommand<CancelEventArgs> OnClosingCommand { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNumberFilterTextChanged { get; set; }

        public QualityMV()
        {
            Quality = _service.GetAllQuality(DateTime.Today.Year);
            OnClosingCommand = new RelayCommand<CancelEventArgs>(this.OnClosingCommandExecuted);
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductNumberFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNumberTextChangedFilter);
        }

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void OnClosingCommandExecuted(CancelEventArgs e)
        {
            MessageBoxResult ansver = MessageBoxResult.No;

            //if (Modified)
            //{
            //    ansver = MessageBox.Show("Dokonano zmian w rekordach. Czy zapisać zmiany?", "Zamis zmian", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //}

            //if (ansver == MessageBoxResult.Yes)
            //{
            //    SaveAll();
            //    WindowSetting.Save(_windowData);
            //}
            //else if (ansver == MessageBoxResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
                WindowSettings.Save(_windowData);
            //}
        }

        public string ProductName { get; set; }

        public string ProductNumber { get; set; }

        public void OnProductNameTextChangedFilter(TextChangedEventArgs e)
        {
            for (int i = 0; i < Quality.Count; i++)
            {

            }
        }

        public void OnProductNumberTextChangedFilter(TextChangedEventArgs e)
        {
            for (int i = 0; i < Quality.Count; i++)
            {

            }
        }

        internal QualityModel GetCurrentQuality => Quality[_selectedIndex];

        public bool IsAnyQuality => Quality.Count > 0;

        internal void SetQualityDataMV(QualityDataMV qualityDataMV)
        {
            _qualityDataMV = qualityDataMV;
        }

        internal bool Modified => Quality.Any(x => x.Modified) || _qualityDataMV != null ? _qualityDataMV.Modified : false;

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

        public Thickness TxtNumberLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value < 0 || Quality.Count == 0 || value >= Quality.Count) return;

                _selectedIndex = value;
                _remarks = Quality[_selectedIndex].Remarks;
                _productionDate = Quality[_selectedIndex].ProductionDate;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(Remarks));
                OnPropertyChanged(nameof(ProductionDate));

                if (_qualityDataMV != null)
                    _qualityDataMV.RefreshQualityData(Quality[_selectedIndex]);
            }
        }

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

        public ICommand SaveButton
        {
            get
            {
                if (_saveButton == null) _saveButton = new SaveButton(this);
                return _saveButton;
            }
        }

        public void SaveAll()
        {
            _qualityDataMV.Save();
        }
    }
}
