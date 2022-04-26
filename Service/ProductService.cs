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
        public SortableObservableCollection<ProductModel> Products { get; set; }

        public ProductService()
        {
            Products = _repository.GetOnlyActiveProduct();
        }
    }
}
