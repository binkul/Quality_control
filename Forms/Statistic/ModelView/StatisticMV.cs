using GalaSoft.MvvmLight.Command;
using Quality_Control.Forms.AddNew.Model;
using Quality_Control.Forms.Navigation;
using Quality_Control.Forms.Statistic.Command;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Statistic.ModelView
{
    internal class StatisticMV : INotifyPropertyChanged, INavigation
    {
        private ICommand _todayButton;
        private ICommand _productButton;
        private ICommand _rangeButton;

        private readonly double _startLeftPosition = 32;
        private readonly ProductService _service = new ProductService();
        private NavigationMV _navigationMV;
        private int _selectedIndex;
        private string _filterIndex = "";
        private string _filterName = "";
        public DateTime DateStart { get; set; } = DateTime.Today;
        public DateTime DateEnd { get; set; } = DateTime.Today;
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductIndexFilterTextChanged { get; set; }

        public StatisticMV()
        {
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductIndexFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductIndexTextChangedFilter);
        }

        private void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        internal NavigationMV SetNavigationMV
        {
            set => _navigationMV = value;
        }

        public List<ProductModel> Products => _service.FilteredProducts;

        public Thickness TxtIndexLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);

        #region Navigation

        public int GetRowCount => _service.FilteredProducts.Count;

        public int DgRowIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(DgRowIndex));
                Refresh();
            }
        }

        public void Refresh()
        {
            if (_navigationMV != null)
                _navigationMV.Refresh();
        }

        #endregion

        #region Filtering

        public string FilterNameTxt
        {
            get => _filterName;
            set => _filterName = value;
        }

        public string IndexFilterTxt
        {
            get => _filterIndex;
            set => _filterIndex = value;
        }

        public void OnProductNameTextChangedFilter(TextChangedEventArgs e)
        {
            _service.Filter(IndexFilterTxt, FilterNameTxt);
            OnPropertyChanged(nameof(Products));
            Refresh();
        }

        public void OnProductIndexTextChangedFilter(TextChangedEventArgs e)
        {
            _service.Filter(IndexFilterTxt, FilterNameTxt);
            OnPropertyChanged(nameof(Products));
            Refresh();
        }

        #endregion

        public ICommand TodayButton
        {
            get
            {
                if (_todayButton == null) _todayButton = new TodayButton(this);
                return _todayButton;
            }
        }

        public ICommand ProductButton
        {
            get
            {
                if (_productButton == null) _productButton = new ProductButton(this);
                return _productButton;
            }
        }

        public ICommand RangeButton
        {
            get
            {
                if (_rangeButton == null) _rangeButton = new RangeButton(this);
                return _rangeButton;
            }
        }


        internal void ShowToday()
        {
            StatisticTodayForm form = new StatisticTodayForm();
            _ = form.ShowDialog();
        }

        internal void ShowProduct()
        {

        }

        internal void ShowRange()
        {

        }
    }

}
