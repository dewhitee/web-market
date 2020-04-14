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
                : $"{LeftProduct.Name} is cheaper than {RightProduct.Name}";
        }
        public static string DiscountComparisonText()
        {
            return (LeftProduct.Discount > RightProduct.Discount) ? $"{LeftProduct.Name} has larger discount than {RightProduct.Name}"
                : $"{LeftProduct.Name} has smaller discount than {RightProduct.Name}";
        }
        public static string AddedDateComparisonText()
        {
            return (LeftProduct.AddedDate > RightProduct.AddedDate) ? $"{LeftProduct.Name} was added later than {RightProduct.Name}"
                : $"{LeftProduct.Name} was added earlier than {RightProduct.Name}";
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
    }
}
