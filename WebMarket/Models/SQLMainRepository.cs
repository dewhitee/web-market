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

        public UserComment AddUserComment(UserComment comment)
        {
            context.Comments.Add(comment);
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

        public UserComment GetUserComment(int id)
        {
            return context.Comments.Find(id);
        }

        public UserComment GetUserCommentByIndex(int index)
        {
            return context.Comments.ToList()[index];
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
    }
}
