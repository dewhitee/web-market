using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models.AppUserModels
{
    public class BoughtProductsViewModel
    {
        public static CatalogViewModel.CatalogViewVariant ViewVariant { get; set; }
        public List<Product> BoughtProducts { get; set; }
    }
}
