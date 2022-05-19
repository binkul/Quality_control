using Quality_Control.Forms.Statistic.Command;
using Quality_Control.Service;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

namespace Quality_Control.Forms.Statistic.ModelView
{
    internal class StatisticRangeMV : INotifyPropertyChanged
    {
        private ICommand _saveRangeButton;

        public StatisticService Service;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        internal bool Modified => Service != null && Service.Modified;

        public DataTable RangeData => Service?.Statistic;

        public bool IsAnyQuality => Service != null && RangeData.Rows.Count > 0;

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
