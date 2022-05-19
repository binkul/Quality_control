using Quality_Control.Forms.Statistic.Command;
using Quality_Control.Forms.Statistic.Model;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

namespace Quality_Control.Forms.Statistic.ModelView
{
    internal class StatisticTodayMV : INotifyPropertyChanged
    {
        private static readonly StatisticDto statisticDto = new StatisticDto("Statystyka na dziś: " + DateTime.Today.ToShortDateString(),
                DateTime.Now, DateTime.Now, StatisticType.Today);

        private ICommand _saveTodayButton;
        private readonly StatisticService _service = new StatisticService(statisticDto);
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public DataTable TodayData => _service.Statistic;

        public bool IsAnyQuality => TodayData.Rows.Count > 0;

        public bool Modified => _service.Modified;

        public List<string> GetActiveFields => _service.GetVisibleColumn;

        public ICommand TodaySaveButton
        {
            get
            {
                if (_saveTodayButton == null) _saveTodayButton = new TodaySaveButton(this);
                return _saveTodayButton;
            }
        }

        internal void SaveToday()
        {
            _ = _service.SaveToday();
        }
    }
}
