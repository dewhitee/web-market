using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    public class ComparisonViewModel
    {
        public List<Product> Products { get; set; }
        public static Product LeftProduct;
        public static Product RightProduct;
        [BindProperty(SupportsGet = true)]
        public string RightSearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LeftSearchTerm { get; set; }

        public static string PriceComparisonText()
        {
            return (LeftProduct.Price > RightProduct.Price) ? $"{LeftProduct.Name} is more expensive than {RightProduct.Name}"
                : LeftProduct.Price < RightProduct.Price ? $"{LeftProduct.Name} is cheaper than {RightProduct.Name}"
                : $"{LeftProduct.Name} and {RightProduct.Name} are equal in terms of price ";
        }
        public static string DiscountComparisonText()
        {
            return (LeftProduct.Discount > RightProduct.Discount) ? $"{LeftProduct.Name} has larger discount than {RightProduct.Name}"
                : LeftProduct.Discount < RightProduct.Discount ? $"{LeftProduct.Name} has smaller discount than {RightProduct.Name}"
                : $"{LeftProduct.Name} has same discount as {RightProduct.Name}";
        }
        public static string AddedDateComparisonText()
        {
            return (LeftProduct.AddedDate > RightProduct.AddedDate) ? $"{LeftProduct.Name} was added later than {RightProduct.Name}"
                : LeftProduct.AddedDate < RightProduct.AddedDate ? $"{LeftProduct.Name} was added earlier than {RightProduct.Name}"
                : $"{LeftProduct.Name} was added at the same day with {RightProduct.Name}";
        }

        public static float GetStarsValue(Product product, IMainRepository repository)
        {
            return product.GetRate(repository);
        }
        public static float GetStarsValue(IEnumerable<UserComment> userComments)
        {
            return Product.GetRate(userComments);
        }
    }
}
