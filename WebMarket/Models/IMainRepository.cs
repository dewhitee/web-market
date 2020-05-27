using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public interface IMainRepository
    {
        DbContext GetDbContext();
        Product GetProduct(int id);
        Product GetProductByIndex(int index);
        IEnumerable<Product> GetProductsByName(string name);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsOfUser(string id);
        IEnumerable<Product> GetProductsByBought(IEnumerable<BoughtProduct> boughtProducts);
        IEnumerable<Product> Search(string searchTerm);
        Product AddProduct(Product product);
        Product UpdateProduct(Product productChanges);
        Product DeleteProduct(int id);

        UserComment GetUserComment(int id);
        UserComment GetUserCommentByIndex(int index);
        IEnumerable<UserComment> GetUserCommentsByProdID(int id);
        IEnumerable<UserComment> GetUserCommentsByUserID(string id);
        IEnumerable<UserComment> GetAllUserComments();
        UserComment AddUserComment(UserComment comment);
        UserComment UpdateComment(UserComment commentChanges);
        UserComment DeleteUserComment(int id);

        Tag GetTag(int id);
        string GetTagNameByProductType(int id);
        IEnumerable<Tag> GetTagsByProductID(int id);
        IEnumerable<string> GetTagNamesByProductId(int id);
        IEnumerable<Tag> GetAllTags();
        Tag AddTag(Tag tag);
        Tag UpdateTag(Tag tagChanges);
        Tag DeleteTag(int id);

        Image GetImage(int id);
        IEnumerable<Image> GetImagesByProductID(int id);
        Image GetImageByOrderIndex(int productID, int orderIndex);
        IEnumerable<Image> GetAllImages();
        Image AddImage(Image image);
        Image UpdateImage(Image imageChanges);
        Image DeleteImage(int id);

        ProductType GetProductType(int id);
        ProductType GetProductTypeByName(string name);
        IEnumerable<ProductType> GetAllProductTypes();
        ProductType AddProductType(ProductType productType);
        ProductType UpdateProductType(ProductType productTypeChanges);
        ProductType DeleteProductType(int id);

        BoughtProduct GetBoughtProduct(int id);
        IEnumerable<BoughtProduct> GetBoughtProductsByUserId(string id);
        IEnumerable<BoughtProduct> GetBoughtProductsByProductId(int id);
        IEnumerable<BoughtProduct> GetAllBoughtProducts();
        BoughtProduct AddBoughtProduct(BoughtProduct boughtProduct);
        BoughtProduct UpdateBoughtProduct(BoughtProduct boughtProductChanges);
        BoughtProduct DeleteBoughtProduct(int id);
        BoughtProduct DeleteBoughtProduct(string userId, int productId);

    }
}
