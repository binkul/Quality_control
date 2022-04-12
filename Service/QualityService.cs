using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System.ComponentModel;
using System.Data;

namespace Quality_Control.Service
{
    public class QualityService
    {
        private readonly QualityRepository _repository;
        private readonly DataTable _dataQualityTable;
        private readonly DataView _dataQualityView;

        public QualityService()
        {
            _repository = new QualityRepository();
            _dataQualityTable = GetQualityDataById(-1);
            _dataQualityView = new DataView(_dataQualityTable);
        }

        public DataView DataQualityView => _dataQualityView;

        public SortableObservableCollection<QualityModel> GetAllQuality(int year)
        {
            var list = _repository.GetAllByYear(year);
            list.Sort(x => x.Number, ListSortDirection.Ascending);
            return list;
        }

        public DataTable GetQualityDataById(long id)
        {
            return _repository.GetQualityDataByQualityId(id);
        }

        public void RefreshQualityData(long id)
        {
            _dataQualityTable.Rows.Clear();
            _repository.LoadQualityDataById(_dataQualityTable, id);
        }
    }
}
