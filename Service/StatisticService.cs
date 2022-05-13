using Quality_Control.Repository;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Quality_Control.Service
{
    public class StatisticService
    {
        private readonly StatisticRepository _repository;
        public DataTable GetTodayData { get; }
        public List<string> GetVisibleColumn { get; }

        public StatisticService()
        {
            _repository = new StatisticRepository();
            GetTodayData = _repository.GetStatisticToday();
            GetVisibleColumn = ShowColumnForToday();
        }

        private List<string> ShowColumnForToday()
        {
            return GetTodayData
                .AsEnumerable()
                .Select(row => row.Field<string>("active_fields"))
                .Select(field => field.Split('|'))
                .SelectMany(x => x)
                .Distinct()
                .ToList();
                //.Aggregate((x, y) => x + "|" + y);
        }

    }
}
