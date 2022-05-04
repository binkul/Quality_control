using Quality_Control.Forms.Setting.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control.Forms.Setting.Command
{
    internal class CopyButton : ICommand
    {
        private readonly SettingMV _modelView;

        public CopyButton(SettingMV modelView)
        {
            if (modelView == null) throw new ArgumentNullException("Model widoku jest null");
            _modelView = modelView;
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
            return true;
        }

        public void Execute(object parameter)
        {
            _modelView.Copy();
        }
    }
}
