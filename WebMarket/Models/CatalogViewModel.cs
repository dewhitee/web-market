using System;
using System.Collections.Generic;

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
        public static int ChoosenProductID { get; set; }
        public bool FullyMatching { get; set; }
        public int CatalogLength { get; set; }

        public List<string> FindTags { get; set; }
        public string SortBy { get; set; }

        public static CatalogViewVariant ViewVariant { get; set; }
    }
}
