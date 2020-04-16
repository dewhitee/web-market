using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    public class ComparisonViewModel
    {
        public static List<Product> Products = new List<Product>();
        public static Product LeftProduct;
        public static Product RightProduct;

        private static List<string> ProductNames;

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
        public static string[] GetProductNames()
        {
            if (ProductNames == null || ProductNames.Count() == 0)
            {
                ProductNames = new List<string>();
                foreach (var i in CatalogViewModel.ListOfProducts)
                {
                    ProductNames.Add(i.Name);
                }
            }
            return ProductNames.ToArray();
        }

        public static float GetStarsValue(Product product)
        {
            return product.GetRate() * 5f;
        }
    }
}
