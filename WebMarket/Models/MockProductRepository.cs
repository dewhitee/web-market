using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class MockProductRepository : IProductRepository
    {
        private List<Product> _productList;

        public MockProductRepository()
        {
            _productList = new List<Product>()
            {
                new Product() { ID = 2, Name = "Hi", Price = 20M, Description = "Desc"},
                new Product() { ID = 4, Name = "Hello", Price = 12M, Description = "No desc"}
            };
        }

        public Product Add(Product product)
        {
            _productList.Add(product);
            return product;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productList;
        }

        public Product GetProduct(int id)
        {
            return _productList.FirstOrDefault(e => e.ID == id);
        }
    }
}
