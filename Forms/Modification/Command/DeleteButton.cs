using Quality_Control.Forms.Modification.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control.Forms.Modification.Command
{
    internal class DeleteButton : ICommand
    {
        private readonly ModificationMV _modelView;

        public DeleteButton(ModificationMV modelView)
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
            return _modelView.IsAnySet;
        }

        public void Execute(object parameter)
        {
            _modelView.Delete();
        }
    }
}
