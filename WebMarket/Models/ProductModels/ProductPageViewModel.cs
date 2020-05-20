using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models.ProductModels
{
    public class ProductPageViewModel : Product
    {
        public static AppUser CurrentAppUser { get; set; }
    }
}
