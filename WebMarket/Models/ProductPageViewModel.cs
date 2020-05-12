using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    public class ProductPageViewModel : Product
    {
        public static AppUser CurrentAppUser { get; set; }
    }
}
