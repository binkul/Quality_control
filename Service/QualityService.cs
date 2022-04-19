using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public bool Delete(DataRowView quality)
        {
            bool result = true;
            long id = Convert.ToInt64(quality["id"]);
            int number = Convert.ToInt32(quality["number"]);
            string name = quality["product_name"].ToString();

            if (MessageBox.Show("Czy na pewno usunąć produkcję P" + number + " '" + name + "' z listy?", "Usuwanie", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

            }

            return result;
        }
    }
}
