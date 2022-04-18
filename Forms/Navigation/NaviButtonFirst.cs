using System;
using System.Windows.Input;

namespace Quality_Control.Forms.Navigation
{
    internal class NaviButtonFirst : ICommand
    {
        private readonly NavigationMV _navigation;

        public NaviButtonFirst(NavigationMV navigation)
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
            return _navigation.DgRowIndex > 0;
        }

        public void Execute(object parameter)
        {
            _navigation.DgRowIndex = 0;
        }

    }
}
