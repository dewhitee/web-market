using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class CatalogViewModel
    {
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

            public string PriceString { get => Price > 0 ? Price.ToString() + "€" : "free"; }

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
