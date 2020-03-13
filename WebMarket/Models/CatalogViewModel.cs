using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    [Serializable]
    public class CatalogViewModel
    {
        [Serializable]
        public class Product
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public decimal Price { get; set; }
            public int CostIntegral { get; set; }
            public int CostFractional { get; set; }
            public float Discount { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }

            public string PriceString { get => Price > 0 ? Price.ToString() + "€" : "free"; }
            public string DiscountString { get => Discount > 0 ? Discount.ToString() + "%" : "no"; }

            public static string CheckTypeString(string type)
            {
                if (type == "Choose type")
                    return "No type specified";
                else return type;
            }

            public string GetPriceTableClassString()
            {
                if (Price == 0)
                    return "bg-primary";
                if (Price < 10)
                    return "bg-success";
                if (Price > 250)
                    return "bg-danger";
                return "";
            }

            public static int CompareByName(Product x, Product y)
            {
                return x.Name.CompareTo(y.Name);
            }
            public static int CompareByType(Product x, Product y)
            {
                return x.Type.CompareTo(y.Type);
            }
            public static int CompareByPrice(Product x, Product y)
            {
                return x.Price.CompareTo(y.Price);
            }
            public static int CompareByDiscount(Product x, Product y)
            {
                return y.Discount.CompareTo(x.Discount);
            }
        }

        public static List<Product> ListOfProducts = new List<Product>();

        public List<Product> Products { get; set; }
        public Product ToAdd { get; set; }

        public static bool ContainsID(int ID)
        {
            foreach (var i in ListOfProducts)
            {
                if (i.ID == ID)
                    return true;
            }
            return false;
        }
        public static bool ContainsName(string Name)
        {
            foreach (var i in ListOfProducts)
            {
                if (i.Name == Name)
                    return true;
            }
            return false;
        }
    }
}
