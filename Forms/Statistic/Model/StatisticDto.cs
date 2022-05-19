using Quality_Control.Service;
using System;

namespace Quality_Control.Forms.Statistic.Model
{
    public class StatisticDto
    {
        public string Title { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public StatisticType Type { get; set; }

        public StatisticDto()
        {
        }

        public StatisticDto(string title, DateTime dateStart, DateTime dateEnd, StatisticType type)
        {
            Title = title;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Type = type;
        }
    }
}
