using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    public class SQLMainRepository : IMainRepository
    {
        private readonly MainDbContext context;

        public SQLMainRepository(MainDbContext context)
        {
            this.context = context;
        }

        public Image AddImage(Image image)
        {
            context.Images.Add(image);
            context.SaveChanges();
            return image;
        }

        public Product AddProduct(Product product)
        {
            context.Products.Add(product);
            context.Database.OpenConnection();
            try
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Products ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Products OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }
            return product;
        }

        public ProductType AddProductType(ProductType productType)
        {
            context.ProductTypes.Add(productType);
            context.SaveChanges();
            return productType;
        }

        public Tag AddTag(Tag tag)
        {
            context.Tags.Add(tag);
            context.SaveChanges();
            return tag;
        }

        public UserComment AddUserComment(UserComment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();
            return comment;
        }

        public Image DeleteImage(int id)
        {
            Image image = context.Images.Find(id);
            if (image != null)
            {
                context.Images.Remove(image);
                context.SaveChanges();
            }
            return image;
        }

        public Product DeleteProduct(int id)
        {
            Product product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
            return product;
        }

        public ProductType DeleteProductType(int id)
        {
            ProductType productType = context.ProductTypes.Find(id);
            if (productType != null)
            {
                context.ProductTypes.Remove(productType);
                context.SaveChanges();
            }
            return productType;
        }

        public Tag DeleteTag(int id)
        {
            Tag tag = context.Tags.Find(id);
            if (tag != null)
            {
                context.Tags.Remove(tag);
                context.SaveChanges();
            }
            return tag;
        }

        public UserComment DeleteUserComment(int id)
        {
            UserComment comment = context.Comments.Find(id);
            if (comment != null)
            {
                context.Comments.Remove(comment);
                context.SaveChanges();
            }
            return comment;
        }

        public IEnumerable<Image> GetAllImages()
        {
            return context.Images;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products;
        }

        public IEnumerable<ProductType> GetAllProductTypes()
        {
            return context.ProductTypes;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return context.Tags;
        }

        public IEnumerable<UserComment> GetAllUserComments()
        {
            return context.Comments;
        }

        public Image GetImage(int id)
        {
            return context.Images.Find(id);
        }

        public Image GetImageByOrderIndex(int productID, int orderIndex)
        {
            return (from i in context.Images
                    where i.ProductID == productID.ToString() && i.OrderIndex == orderIndex
                    select i).FirstOrDefault();
        }

        public IEnumerable<Image> GetImagesByProductID(int id)
        {
            return from i in context.Images
                   where i.ProductID == id.ToString()
                   select i;
        }

        public Product GetProduct(int id)
        {
            return context.Products.Find(id);
        }

        public Product GetProductByIndex(int index)
        {
            return context.Products.ToList()[index];
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            return from p in context.Products
                   where p.Name == name
                   select p;
        }

        public ProductType GetProductType(int id)
        {
            return context.ProductTypes.Find(id);
        }

        public ProductType GetProductTypeByName(string name)
        {
            return (from pt in context.ProductTypes
                   where pt.Name == name
                   select pt).FirstOrDefault();
        }

        public Tag GetTag(int id)
        {
            return context.Tags.Find(id);
        }

        public IEnumerable<string> GetTagNamesByProductId(int id)
        {
            return from prodType in context.ProductTypes
                   join tag in context.Tags on prodType.ID equals tag.TypeId 
                   where tag.ProductID == id.ToString()
                   select prodType.Name;
        }

        public IEnumerable<Tag> GetTagsByProductID(int id)
        {
            return from t in context.Tags
                   where t.ProductID == id.ToString()
                   select t;
        }

        public UserComment GetUserComment(int id)
        {
            return context.Comments.Find(id);
        }

        public UserComment GetUserCommentByIndex(int index)
        {
            return context.Comments.ToList()[index];
        }

        public IEnumerable<UserComment> GetUserCommentsByProdID(int id)
        {
            return from c in context.Comments
                           where c.ProductID == id.ToString()
                           select c;
        }

        public IEnumerable<UserComment> GetUserCommentsByUserID(int id)
        {
            return from c in context.Comments
                           where c.UserID == id.ToString()
                           select c;
        }

        public IEnumerable<Product> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return context.Products;
            }
            return context.Products.Where(p => p.Name.Contains(searchTerm));
        }

        public UserComment UpdateComment(UserComment commentChanges)
        {
            var comment = context.Comments.Attach(commentChanges);
            comment.State = EntityState.Modified;
            context.SaveChanges();
            return commentChanges;
        }

        public Image UpdateImage(Image imageChanges)
        {
            var image = context.Images.Attach(imageChanges);
            image.State = EntityState.Modified;
            context.SaveChanges();
            return imageChanges;
        }

        public Product UpdateProduct(Product productChanges)
        {
            var product = context.Products.Attach(productChanges);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return productChanges;
        }

        public ProductType UpdateProductType(ProductType productTypeChanges)
        {
            var productType = context.ProductTypes.Attach(productTypeChanges);
            productType.State = EntityState.Modified;
            context.SaveChanges();
            return productTypeChanges;
        }

        public Tag UpdateTag(Tag tagChanges)
        {
            var tag = context.Tags.Attach(tagChanges);
            tag.State = EntityState.Modified;
            context.SaveChanges();
            return tagChanges;
        }
    }
}
