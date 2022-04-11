using GalaSoft.MvvmLight.CommandWpf;
using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityFormMV : INotifyPropertyChanged
    {
        private ICommand _saveButton;

        private readonly double _startLeftPosition = 32;
        private readonly WindowData _windowData = WindowSettings.Read();
        private readonly QualityService _service = new QualityService();
        private int _selectedIndex;

        public bool Modified { get; private set; } = false;
        public SortableObservableCollection<QualityModel> Quality { get; }
        public ObservableCollection<QualityModel> tmp = new ObservableCollection<QualityModel>();
        public RelayCommand<CancelEventArgs> OnClosingCommand { get; set; }



        public QualityFormMV()
        {
            Quality = _service.GetAllQuality(DateTime.Today.Year);
            OnClosingCommand = new RelayCommand<CancelEventArgs>(this.OnClosingCommandExecuted);
            Quality.CollectionChanged += Quality_CollectionChanged;
        }

        private void Quality_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Modified = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public Thickness TxtNumberLeftPosition => new Thickness(_startLeftPosition, 0, 0, 0);

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value < 0) return;

                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
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

        }
    }
}
