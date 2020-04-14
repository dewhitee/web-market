using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Models;

namespace WebMarket.Data
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string ID { get; set; }
        public decimal Money { get; set; }
        public string MoneyString { get => Money.ToString("0.##") + "€"; }

        public void BuyProduct(Product product)
        {
            if (Money >= product.FinalPrice && !product.IsBought)
            {
                // todo: add and save product to profile
                Money -= product.FinalPrice;
                product.IsBought = true;
                Console.WriteLine($"{product.Name} is bought!");
            }
            else
            {
                Console.WriteLine("User don't have enough money or product is already bought!");
            }
        }
        public void SellProduct(Product product)
        {
            if (product.IsBought)
            {
                Money += product.FinalPrice;
                product.IsBought = false;
                Console.WriteLine($"{product.Name} is sold!");
            }
        }
        public void AddInitMoney()
        {
            Money = 100M;
        }
        public void ResetID()
        {
            Random random = new Random();
            bool success;
            do
            {
                ID = random.Next(0, int.MaxValue).ToString();
                success = CatalogViewModel.ListOfUsers.Find(x => x.ID == ID) == null;
            } while (!success);
            Console.WriteLine($"Your new ID is {ID}");
        }
    }
}
