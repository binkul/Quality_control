using Quality_Control.Repository;
using System.Data;

namespace Quality_Control.Service
{
    public class QualityDataService
    {
        private readonly QualityDataRepository _repository;

        public QualityDataService()
        {
            _repository = new QualityDataRepository();
        }

        public DataTable GetQualityDataById(long id)
        {
            return _repository.GetQualityDataByQualityId(id);
        }

        public void RefreshQualityData(long id, DataTable table)
        {
            table.Rows.Clear();
            _repository.LoadQualityDataById(table, id);
        }

    }
}
