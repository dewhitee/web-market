using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebMarket.Models
{
    public class AppUser : IdentityUser
    {

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }
        public string MoneyString => Money.ToString("0.##") + "€";
    }
}
