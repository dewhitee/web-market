using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WebMarket.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace WebMarket.Models
{
    [Serializable]
    public class CatalogViewModel
    {
        public enum CatalogViewVariant
        {
            Main,
            Table,
        }
        public enum ProductSort
        {
            None,
            Name,
            Type,
            Price,
            Discount,
            FinalPrice,
        }

        public static List<Product> ListOfProducts = new List<Product>();
        public static IEnumerable<string> ListOfProductTypes { get; set; }
        ///public static List<User> ListOfUsers = new List<User>();
        public static List<Product> AddedToCartProducts = new List<Product>();
        public static Product ChoosenProduct = new Product();
        public static int ChoosenProductID { get; set; }
        ///public static User CurrentUser = new User();

        public List<Product> Products { get; set; }
        public Product ToAdd { get; set; }

        public static AppUser CurrentAppUser { get; set; }

        ///private static string addedToCartProductsFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\addedtocartproducts_" + CurrentUser.Username + "_.dew"; }
        private static string saveProductsFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\products.dew"; }
        private static string findTagsFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\findtags.dew"; }

        public static List<string> FindTags { get; set; }

        public static CatalogViewVariant ViewVariant { get; set; }

        public static Product GetSelectedBuyProduct()
        {
            return ChoosenProduct ?? new Product();
        }

        public static Product GetSelectedSellProduct(IMainRepository repository, AppUser user)
        {
            return ChoosenProduct != null && ChoosenProduct.IsBought(repository, user) ? ChoosenProduct : new Product();
        }
        public static string GetSelectedSellProductPriceSentence(IMainRepository repository, AppUser user)
        {
            var product = GetSelectedSellProduct(repository, user);
            if (!string.IsNullOrWhiteSpace(product.Name) && user != null)
                return user.MoneyString + " + " + product.FinalPriceString + string.Format(" = {0}€", (user.Money + product.FinalPrice).ToString("0.##"));
            else
                return "You have not selected any product to sell!";
        }
        public static string GetSelectedBuyProductPriceSentence(AppUser user)
        {
            var product = GetSelectedBuyProduct();
            var finalCost = user?.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name) && user != null)
                return user.MoneyString + " - " + product.FinalPriceString + string.Format(" = {0}€", finalCost?.ToString("0.##")) + (finalCost < 0 ? " (You don't have enough money!)" : "");
            else
                return "You have not selected any product to buy!";
        }
        public static string GetBuyProductButtonClassString(AppUser user, bool outline = true)
        {
            var product = GetSelectedBuyProduct();
            var finalCost = user?.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                if (finalCost < 0)
                    return "btn btn-danger";
            }
            return outline ? "btn btn-outline-primary" : "btn btn-primary";
        }
    }
}
