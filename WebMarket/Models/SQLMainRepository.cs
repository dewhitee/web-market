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

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return context.Tags;
        }

        public IEnumerable<UserComment> GetAllUserComments()
        {
            return context.Comments;
        }

        public Product GetProduct(int id)
        {
            return context.Products.Find(id);
        }

        public Product GetProductByIndex(int index)
        {
            return context.Products.ToList()[index];
        }

        public Tag GetTag(int id)
        {
            return context.Tags.Find(id);
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

        public UserComment UpdateComment(UserComment commentChanges)
        {
            var comment = context.Comments.Attach(commentChanges);
            comment.State = EntityState.Modified;
            context.SaveChanges();
            return commentChanges;
        }

        public Product UpdateProduct(Product productChanges)
        {
            var product = context.Products.Attach(productChanges);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return productChanges;
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
