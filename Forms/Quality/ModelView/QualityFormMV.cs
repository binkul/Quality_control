using GalaSoft.MvvmLight.CommandWpf;
using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.ComponentModel;
using System.Windows;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityFormMV : INotifyPropertyChanged
    {
        private readonly double _startLeftPosition = 32;
        private readonly WindowData _windowData = WindowSettings.Read();
        private readonly QualityService _service = new QualityService();
        private int _selectedIndex;

        public SortableObservableCollection<QualityModel> Quality { get; }
        public RelayCommand<CancelEventArgs> OnClosingCommand { get; set; }



        public QualityFormMV()
        {
            Quality = _service.GetAllQuality(DateTime.Today.Year);
            OnClosingCommand = new RelayCommand<CancelEventArgs>(this.OnClosingCommandExecuted);
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

    }
}
