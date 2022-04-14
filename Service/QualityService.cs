using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Quality_Control.Service
{
    public class QualityService
    {
        private readonly QualityRepository _repository;

        public QualityService()
        {
            _repository = new QualityRepository();
        }

        public SortableObservableCollection<QualityModel> GetAllQuality(int year)
        {
            var list = _repository.GetAllByYear(year);
            list.Sort(x => x.Number, ListSortDirection.Ascending);
            return list;
        }
    }
}
