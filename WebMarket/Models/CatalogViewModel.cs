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
        }

        public static List<Product> ListOfProducts = new List<Product>();
        public List<Product> Products { get; set; }
        public Product ToAdd { get; set; }
    }
}
