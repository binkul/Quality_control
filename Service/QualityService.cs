using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

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

        public List<int> GetAllYears()
        {
            return _repository.GetAllYears();
        }

        public bool Delete(QualityModel quality)
        {
            long id = quality.Id;
            QualityDataRepository qualityDataRepository = new QualityDataRepository();

            bool result;
            if (MessageBox.Show("Czy na pewno usunąć produkcję P" + quality.Number + " '" + quality.ProductName + "' z listy?", "Usuwanie",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                result = _repository.DeleteQualityById(id) && qualityDataRepository.DeleteQualityDataByQualityId(id);
            }
            else
                return false;

            return result;
        }
    }
}
