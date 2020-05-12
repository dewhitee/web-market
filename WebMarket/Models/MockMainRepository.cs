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

        public BoughtProduct AddBoughtProduct(BoughtProduct boughtProduct)
        {
            throw new NotImplementedException();
        }

        public Image AddImage(Image image)
        {
            throw new NotImplementedException();
        }

        public Product AddProduct(Product product)
        {
            _productList.Add(product);
            return product;
        }

        public ProductType AddProductType(ProductType productType)
        {
            throw new NotImplementedException();
        }

        public Tag AddTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public UserComment AddUserComment(UserComment comment)
        {
            throw new NotImplementedException();
        }

        public BoughtProduct DeleteBoughtProduct(int id)
        {
            throw new NotImplementedException();
        }

        public BoughtProduct DeleteBoughtProduct(string userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Image DeleteImage(int id)
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

        public ProductType DeleteProductType(int id)
        {
            throw new NotImplementedException();
        }

        public Tag DeleteTag(int id)
        {
            throw new NotImplementedException();
        }

        public UserComment DeleteUserComment(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BoughtProduct> GetAllBoughtProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Image> GetAllImages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productList;
        }

        public IEnumerable<ProductType> GetAllProductTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetAllTags()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserComment> GetAllUserComments()
        {
            throw new NotImplementedException();
        }

        public BoughtProduct GetBoughtProduct(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BoughtProduct> GetBoughtProductsByUserId(string id)
        {
            throw new NotImplementedException();
        }

        public Image GetImage(int id)
        {
            throw new NotImplementedException();
        }

        public Image GetImageByOrderIndex(int productID, int orderIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Image> GetImagesByProductID(int id)
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

        public IEnumerable<Product> GetProductsByName(string name)
        {
            throw new NotImplementedException();
        }

        public ProductType GetProductType(int id)
        {
            throw new NotImplementedException();
        }

        public ProductType GetProductTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public Tag GetTag(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetTagNamesByProductId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetTagsByProductID(int id)
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

        public IEnumerable<UserComment> GetUserCommentsByProdID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserComment> GetUserCommentsByUserID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public BoughtProduct UpdateBoughtProduct(BoughtProduct boughtProductChanges)
        {
            throw new NotImplementedException();
        }

        public UserComment UpdateComment(UserComment commentChanges)
        {
            throw new NotImplementedException();
        }

        public Image UpdateImage(Image imageChanges)
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

        public ProductType UpdateProductType(ProductType productTypeChanges)
        {
            throw new NotImplementedException();
        }

        public Tag UpdateTag(Tag tagChanges)
        {
            throw new NotImplementedException();
        }
    }
}
