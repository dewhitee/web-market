using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class ComparisonViewModel
    {
        public static List<CatalogViewModel.Product> Products = new List<CatalogViewModel.Product>();
        public static CatalogViewModel.Product LeftProduct;
        public static CatalogViewModel.Product RightProduct;

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
    }
}
