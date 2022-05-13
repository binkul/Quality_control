using Quality_Control.Forms.Statistic.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control.Forms.Statistic.Command
{
    internal class RangeButton : ICommand
    {
        private readonly StatisticMV _modelView;

        public RangeButton(StatisticMV modelView)
        {
            _modelView = modelView ?? throw new ArgumentNullException("Model widoku jest null");
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
            _modelView.ShowRange();
        }
    }
}
