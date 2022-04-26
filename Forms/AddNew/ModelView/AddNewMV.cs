using GalaSoft.MvvmLight.Command;
using Quality_Control.Commons;
using Quality_Control.Forms.AddNew.Model;
using Quality_Control.Forms.Navigation;
using Quality_Control.Service;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Quality_Control.Forms.AddNew.ModelView
{
    public class AddNewMV : INotifyPropertyChanged, INavigation
    {
        private readonly double _startLeftPosition = 32;
        private readonly ProductService _service = new ProductService();
        private ProductModel _actualProduct;
        private int _selectedIndex;
        private string _productNumber;
        private DateTime _productionDate = DateTime.Today;
        private NavigationMV _navigationMV;
        private string _filterIndex = "";
        private string _filterName = "";
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductIndexFilterTextChanged { get; set; }


        public AddNewMV()
        {
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductIndexFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductIndexTextChangedFilter);
        }

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public SortableObservableCollection<ProductModel> Products => _service.FilteredProducts;

        internal NavigationMV SetNavigationMV
        {
            set => _navigationMV = value;
        }

        public Thickness TxtIndexLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);

        public ProductModel ActualProduct
        {
            internal get => _actualProduct;
            set => _actualProduct = value;
        }

        public string ProductNumber
        {
            get => _productNumber;
            set => _productNumber = value;
        }

        public string ProductIndex => ActualProduct != null ? ActualProduct.Index : "";

        public string ProductName => ActualProduct != null ? ActualProduct.Name : "";

        public DateTime ProductionDate
        {
            get => _productionDate;
            set => _productionDate = value;
        }

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
    }
}
