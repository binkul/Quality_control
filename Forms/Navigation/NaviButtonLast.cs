using System;
using System.Windows.Input;

namespace Quality_Control.Forms.Navigation
{
    internal class NaviButtonLast : ICommand
    {
        private readonly NavigationMV _navigation;

        public NaviButtonLast(NavigationMV navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException("Model widoku jest null");
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _navigation.DgRowIndex < _navigation.GetRowCount - 1;
        }

        public void Execute(object parameter)
        {
            int index = _navigation.GetRowCount - 1;
            _navigation.DgRowIndex = index;
        }
    }
}
