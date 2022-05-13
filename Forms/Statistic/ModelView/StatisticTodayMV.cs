using Quality_Control.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Quality_Control.Forms.Statistic.ModelView
{
    internal class StatisticTodayMV : INotifyPropertyChanged
    {
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

        public List<string> GetActiveFields => _service.GetVisibleColumn;
    }
}
