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

        //public void BuyProduct(Product product, IMainRepository repository)
        //{
        //    if (Money >= product.FinalPrice && !product.IsBought(repository, this))
        //    {
        //        Money -= product.FinalPrice;

        //        Userbase.UserManager.UpdateAsync(this);

        //        repository.AddBoughtProduct(new BoughtProduct
        //        {
        //            AppUserRefId = this.Id,
        //            ProductRefId = product.ID
        //        });
        //        Console.WriteLine($"{product.Name} is bought!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("User don't have enough money or product is already bought!");
        //    }
        //}
        //public void SellProduct(Product product, IMainRepository repo)
        //{
        //    if (product.IsBought(repo, this))
        //    {
        //        Money += product.FinalPrice;

        //        Userbase.UserManager.UpdateAsync(this);

        //        repo.DeleteBoughtProduct(this.Id, product.ID);
        //        Console.WriteLine($"{product.Name} is sold!");
        //    }
        //}
    }
}
