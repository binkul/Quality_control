using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Quality_Control.Service
{
    public class QualityService
    {
        private readonly QualityRepository _repository;
        private int _year = DateTime.Today.Year;
        private List<int> _years;

        public QualityService()
        {
            _repository = new QualityRepository();
            Years = GetAllYears();
        }

        public List<int> Years
        {
            get => _years;
            private set => _years = value;
        }

        public int Year
        {
            get => _year;
            set => _year = value;
        }

        public void ReloadYears()
        {
            int tmpYear = Year;
            Years = GetAllYears();
            Year = Years.Contains(tmpYear) ? tmpYear : Years.Count > 0 ? Years[Years.Count - 1] : -1;
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

        public QualityModel Save(QualityModel quality)
        {

            return quality;
        }

        public bool Update(QualityModel quality)
        {
            return _repository.Update(quality);
        }
    }
}
