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

        public bool Modified => Products.Any(x => x.Modified);

        public void Filter(string index, string name)
        {
            if (!string.IsNullOrEmpty(index) || !string.IsNullOrEmpty(name))
            {
                List<ProductModel> result = Products
                    .Where(x => x.Index.ToLower().Contains(index))
                    .Where(x => x.Name.ToLower().Contains(name))
                    .OrderBy(x => x.Name)
                    .ToList();
                FilteredProducts = result;
            }
            else
            {
                FilteredProducts = Products;
            }
        }

        public void Filter(int number, string name)
        {
            if (number > 0 || !string.IsNullOrEmpty(name))
            {
                List<ProductModel> result = Products
                    .Where(x => x.LabBookId >= number)
                    .Where(x => x.Name.ToLower().Contains(name))
                    .OrderBy(x => x.Name)
                    .ToList();
                FilteredProducts = result;
            }
            else
            {
                FilteredProducts = Products;
            }
        }

        public bool Save(List<ProductModel> products)
        {
            foreach(ProductModel product in products)
            {
                if (!_repository.ExistFieldsByLabBookId(product.LabBookId))
                {
                    if (!_repository.SaveFields(product.ActiveFields, product.LabBookId))
                        return false;
                    product.Modified = false;
                }
                else
                {
                    if (!_repository.UpdateFields(product.ActiveFields, product.LabBookId))
                        return false;
                    product.Modified = false;
                }
            }

            return true;
        }
    }
}
