using Quality_Control.Forms.Statistic.Command;
using Quality_Control.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

namespace Quality_Control.Forms.Statistic.ModelView
{
    internal class StatisticTodayMV : INotifyPropertyChanged
    {
        private ICommand _saveButton;

        private readonly StatisticService _service = new StatisticService();
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public DataTable TodayData => _service.GetTodayData;

        public bool IsAnyQuality => TodayData.Rows.Count > 0;

        public bool Modified => _service.Modified;

        public List<string> GetActiveFields => _service.GetVisibleColumn;

        public ICommand SaveButton
        {
            get
            {
                if (_saveButton == null) _saveButton = new TodaySaveButton(this);
                return _saveButton;
            }
        }

        internal void Save()
        {

        }
    }
}
