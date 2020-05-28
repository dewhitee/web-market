using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models.AppUserModels
{
    public class AddedProductsViewModel
    {
        public enum SortOption
        {
            None,
            ByName,
            ByType,
        }
        public static CatalogViewModel.CatalogViewVariant ViewVariant { get; set; }
        public List<Product> AddedProducts { get; set; }
    }
}
