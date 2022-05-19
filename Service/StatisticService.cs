using Quality_Control.Forms.Statistic.Model;
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
        public DataTable Statistic { get; }
        public List<string> GetVisibleColumn { get; }
        public bool Modified { get; set; } = false;

        public StatisticService(StatisticDto statisticDto)
        {
            _type = statisticDto.Type;
            _repository = new StatisticRepository();
            Statistic = GetStatistic(statisticDto.Type);
            GetVisibleColumn = ColumnVisibility(statisticDto.Type);
            Statistic.ColumnChanged += GetStatisticData_ColumnChanged;
        }

        private void GetStatisticData_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            Modified = true;
        }

        private DataTable GetStatistic(StatisticType type)
        {
            switch (type)
            {
                case StatisticType.Product:
                    return null;
                case StatisticType.Range:
                    return _repository.GetStatisticRange();
                default:
                    return _repository.GetStatisticToday();
            }
        }

        private List<string> ColumnVisibility(StatisticType type)
        {
            return Statistic
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

            DataTable modifiedRows = Statistic.GetChanges(DataRowState.Modified);

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

