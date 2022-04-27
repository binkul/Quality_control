using Quality_Control.Forms.AddNew.Model;
using Quality_Control.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Quality_Control.Service
{
    public class ProductService
    {
        private readonly ProductRepository _repository = new ProductRepository();
        private readonly List<ProductModel> Products;
        
        public List<ProductModel> FilteredProducts { get; set; }

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
                    .OrderBy(x => x.Name)
                    .ToList();
                FilteredProducts = result; // newList;
            }
            else
            {
                FilteredProducts = Products;
            }
        }
    }
}
