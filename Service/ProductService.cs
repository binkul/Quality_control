using Quality_Control.Commons;
using Quality_Control.Forms.AddNew.Model;
using Quality_Control.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quality_Control.Service
{
    public class ProductService
    {
        private readonly ProductRepository _repository = new ProductRepository();
        private readonly SortableObservableCollection<ProductModel> Products;
        public SortableObservableCollection<ProductModel> FilteredProducts { get; set; }

        public ProductService()
        {
            Products = _repository.GetOnlyActiveProduct();
            FilteredProducts = Products;
        }

        public void Filter(string index, string name)
        {
            if (!string.IsNullOrEmpty(index) || !string.IsNullOrEmpty(name))
            {
                List<ProductModel> result = Products
                    .Where(x => x.Index.ToLower().Contains(index))
                    .Where(x => x.Name.ToLower().Contains(name))
                    .ToList();

                SortableObservableCollection<ProductModel> newList = new SortableObservableCollection<ProductModel>();
                foreach(ProductModel product in result)
                {
                    newList.Add(product);
                }

                newList.Sort(x => x.Name, System.ComponentModel.ListSortDirection.Ascending);
                FilteredProducts = newList;
            }
            else
            {
                FilteredProducts = Products;
            }
        }
    }
}
