using Quality_Control.Forms.Statistic.Command;
using Quality_Control.Service;
using System.ComponentModel;
using System.Windows.Input;

namespace Quality_Control.Forms.Statistic.ModelView
{
    internal class StatisticRangeMV : INotifyPropertyChanged
    {
        private ICommand _saveRangeButton;
        private readonly StatisticService _service = new StatisticService(StatisticType.Range);
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        internal bool Modified => _service.Modified;

        public ICommand RangeSaveButton
        {
            get
            {
                if (_saveRangeButton == null) _saveRangeButton = new RangeSaveButton(this);
                return _saveRangeButton;
            }
        }

        internal void SaveRange()
        {

        }

    }
}
