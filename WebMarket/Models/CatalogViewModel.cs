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
            public int ProductIndex { get; set; }
            public string ProductName { get; set; }
            public string ProductType { get; set; }
            public int CostIntegral { get; set; }
            public int CostFractional { get; set; }
            public float Discount { get; set; }
            public string Description { get; set; }
        }

        public List<Product> Products { get; set; }
    }
}
