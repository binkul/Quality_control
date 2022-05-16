using Quality_Control.Repository;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Quality_Control.Service
{
    public enum StatisticType
    {
        Today,
        Range,
        Product
    }

    public class StatisticService
    {
        private StatisticType _type;
        private readonly StatisticRepository _repository;
        public DataTable GetTodayData { get; }
        public List<string> GetVisibleColumn { get; }
        public bool Modified { get; set; } = false;

        public StatisticService(StatisticType type)
        {
            _type = type;
            _repository = new StatisticRepository();
            GetTodayData = _repository.GetStatisticToday();
            GetVisibleColumn = ShowColumnForToday();
            GetTodayData.ColumnChanged += GetTodayData_ColumnChanged;
        }

        private void GetTodayData_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            Modified = true;
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

        public bool SaveToday()
        {
            bool result = true;

            if (!Modified)
                return result;

            DataTable modifiedRows = GetTodayData.GetChanges(DataRowState.Modified);

            foreach (DataRow row in modifiedRows.Rows)
            {
                result = _repository.UpdateQualityData(row);
                if (result)
                {
                    row.AcceptChanges();
                }
                else
                {
                    return result;
                }
            }

            Modified = false;
            return result;
        }
    }
}

