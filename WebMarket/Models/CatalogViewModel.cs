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

        public List<Product> ListOfProducts { get; set; }
        public IEnumerable<string> ListOfProductTypes { get; set; }
        public static Product ChoosenProduct = new Product();
        public static int ChoosenProductID { get; set; }
        public bool FullyMatching { get; set; }
        public int CatalogLength { get; set; }

        public List<string> FindTags { get; set; }
        public string SortBy { get; set; }

        public static CatalogViewVariant ViewVariant { get; set; }
    }
}
