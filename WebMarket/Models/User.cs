using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebMarket.Data;

namespace WebMarket.Models
{
    [Serializable]
    [Obsolete]
    public class User/* : IdentityUser*/
    {
        public string Username { get; set; }
        public string ID { get; set; }
        public decimal Money { get; set; }
        public string MoneyString { get => Money.ToString("0.##") + "€"; }

        public List<string> BoughtProductIDs = new List<string>();

    }
}
