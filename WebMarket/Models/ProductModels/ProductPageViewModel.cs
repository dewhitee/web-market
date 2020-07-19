using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models.ProductModels
{
    [Obsolete("This ViewModel is no longer used.")]
    public class ProductPageViewModel : Product
    {
        public Product Product { get; set; }
    }
}
