using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public interface IMainRepository
    {
        Product GetProduct(int id);
        Product GetProductByIndex(int index);
        IEnumerable<Product> GetAllProducts();
        Product AddProduct(Product product);
        Product UpdateProduct(Product productChanges);
        Product DeleteProduct(int id);

        UserComment GetUserComment(int id);
        UserComment GetUserCommentByIndex(int index);
        IEnumerable<UserComment> GetAllUserComments();
        UserComment AddUserComment(UserComment comment);
        UserComment UpdateComment(UserComment commentChanges);
        UserComment DeleteUserComment(int id);
    }
}
