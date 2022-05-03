using GalaSoft.MvvmLight.Command;
using Quality_Control.Forms.AddNew.Model;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quality_Control.Forms.Setting.ModelView
{
    class SettingMV : INotifyPropertyChanged
    {
        private readonly double _startLeftPosition = 32;
        private readonly ProductService _service = new ProductService();
        public ProductModel ActualProduct { get; set; }
        public int SelectedIndex { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNumberFilterTextChanged { get; set; }

        public SettingMV()
        {
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductNumberFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductIndexTextChangedFilter);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<ProductModel> Products => _service.FilteredProducts;

        public Thickness TxtIndexLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);


        #region Filtering

        public string FilterNameTxt { get; set; } = "";

        public string NumberFilterTxt { get; set; } = "";

        public void OnProductNameTextChangedFilter(TextChangedEventArgs e)
        {
            int number = !string.IsNullOrEmpty(NumberFilterTxt) ? Convert.ToInt32(NumberFilterTxt) : -1;
            _service.Filter(number, FilterNameTxt);
            OnPropertyChanged(nameof(Products));
        }

        public void OnProductIndexTextChangedFilter(TextChangedEventArgs e)
        {
            int number = !string.IsNullOrEmpty(NumberFilterTxt) ? Convert.ToInt32(NumberFilterTxt) : -1;
            _service.Filter(number, FilterNameTxt);
            OnPropertyChanged(nameof(Products));
        }

        #endregion

    }
}
