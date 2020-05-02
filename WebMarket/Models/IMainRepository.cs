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
        IEnumerable<Product> GetProductsByName(string name);
        IEnumerable<Product> GetAllProducts();
        Product AddProduct(Product product);
        Product UpdateProduct(Product productChanges);
        Product DeleteProduct(int id);

        UserComment GetUserComment(int id);
        UserComment GetUserCommentByIndex(int index);
        IEnumerable<UserComment> GetUserCommentsByProdID(int id);
        IEnumerable<UserComment> GetUserCommentsByUserID(int id);
        IEnumerable<UserComment> GetAllUserComments();
        UserComment AddUserComment(UserComment comment);
        UserComment UpdateComment(UserComment commentChanges);
        UserComment DeleteUserComment(int id);

        Tag GetTag(int id);
        IEnumerable<Tag> GetTagsByProductID(int id);
        IEnumerable<Tag> GetAllTags();
        Tag AddTag(Tag tag);
        Tag UpdateTag(Tag tagChanges);
        Tag DeleteTag(int id);

        Image GetImage(int id);
        IEnumerable<Image> GetImagesByProductID(int id);
        IEnumerable<Image> GetAllImages();
        Image AddImage(Image image);
        Image UpdateImage(Image imageChanges);
        Image DeleteImage(int id);
    }
}
