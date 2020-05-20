using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebMarket.Models;

namespace WebMarket.Models
{
    public class AppUser : IdentityUser
    {

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }
        public string MoneyString { get => Money.ToString("0.##") + "€"; }
    }
}
