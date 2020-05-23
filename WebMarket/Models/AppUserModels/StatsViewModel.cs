using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models.AppUserModels
{
    public class StatsViewModel
    {
        public int TotalProductsAdded { get; set; }
        public int TotalProductsBought { get; set; }
        public int TotalStarsAmount { get; set; }
        public int TotalCommentsWritten { get; set; }
        public int TotalCommentsGot { get; set; }
    }
}
