using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class MockMainRepository : IMainRepository
    {
        private List<Product> _productList;

        public MockMainRepository()
        {
            _productList = new List<Product>()
            {
                new Product() { ID = 2, Name = "Hi", Price = 20M, Description = "Desc"},
                new Product() { ID = 4, Name = "Hello", Price = 12M, Description = "No desc"}
            };
        }

        public Product AddProduct(Product product)
        {
            _productList.Add(product);
            return product;
        }

        public UserComment AddUserComment(UserComment comment)
        {
            throw new NotImplementedException();
        }

        public Product DeleteProduct(int id)
        {
            Product product =_productList.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                _productList.Remove(product);
            }
            return product;
        }

        public UserComment DeleteUserComment(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productList;
        }

        public IEnumerable<UserComment> GetAllUserComments()
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(int id)
        {
            return _productList.FirstOrDefault(p => p.ID == id);
        }

        public Product GetProductByIndex(int index)
        {
            throw new NotImplementedException();
        }

        public UserComment GetUserComment(int id)
        {
            throw new NotImplementedException();
        }

        public UserComment GetUserCommentByIndex(int index)
        {
            throw new NotImplementedException();
        }

        public UserComment UpdateComment(UserComment commentChanges)
        {
            throw new NotImplementedException();
        }

        public Product UpdateProduct(Product productChanges)
        {
            Product product = _productList.FirstOrDefault(p => p.ID == productChanges.ID);
            if (product != null)
            {
                product.Name = productChanges.Name;
                product.Price = productChanges.Price;
                product.Discount = productChanges.Discount;
            }
            return product;
        }
    }
}
