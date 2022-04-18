using System.ComponentModel;
using System.Windows.Input;


namespace Quality_Control.Forms.Navigation
{
    /// <summary>
    /// The main ModelView have to have reference to this class and this class must have reference to main ModelView
    /// Set it in ...xaml.cs just after InitializeComponen(), e.g.
    /// 
    /// MainModelView mainModelView = new MainWindowEditMV();
    /// NavigationMV _navigationMV = Resources["navi"] as NavigationMV;
    /// _navigationMV.ModelView = mainModelView;
    /// mainModelView.SetNavigationMV = _navigationMV;
    /// 
    /// NavigationMV.Refresh() should be in MainModelView.Rehresh() called every time in set part of DgRowIndex { get; set;}
    /// 
    /// </summary>

    internal class NavigationMV : INotifyPropertyChanged
    {
        private ICommand _moveRight;
        private ICommand _moveLeft;
        private ICommand _moveLast;
        private ICommand _moveFirst;
        private INavigation _modelView;
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation ModelView
        {
            private get => _modelView;
            set
            {
                _modelView = value;
                Refresh();
            }
        }

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        internal int DgRowIndex
        {
            get => ModelView != null ? ModelView.DgRowIndex : 0;
            set
            {
                if (ModelView != null)
                {
                    ModelView.DgRowIndex = value;
                }
            }
        }

        internal int GetRowCount => ModelView != null ? ModelView.GetRowCount : 0;

        internal void Refresh()
        {
            OnPropertyChanged(nameof(DgRowIndex), nameof(GetRowCount));
        }

        public ICommand MoveRight
        {
            get
            {
                if (_moveRight == null)
                {
                    _moveRight = new NaviButtonRight(this);
                }

                return _moveRight;
            }
        }

        public ICommand MoveLast
        {
            get
            {
                if (_moveLast == null)
                {
                    _moveLast = new NaviButtonLast(this);
                }

                return _moveLast;
            }
        }

        public ICommand MoveFirst
        {
            get
            {
                if (_moveFirst == null)
                {
                    _moveFirst = new NaviButtonFirst(this);
                }

                return _moveFirst;
            }
        }

        public ICommand MoveLeft
        {
            get
            {
                if (_moveLeft == null)
                {
                    _moveLeft = new NaviButtonLeft(this);
                }

                return _moveLeft;
            }
        }

    }
}
