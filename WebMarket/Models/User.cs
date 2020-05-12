using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebMarket.Data;

namespace WebMarket.Models
{
    [Serializable]
    public class User/* : IdentityUser*/
    {
        public string Username { get; set; }
        public string ID { get; set; }
        public decimal Money { get; set; }
        public string MoneyString { get => Money.ToString("0.##") + "€"; }

        public List<string> BoughtProductIDs = new List<string>();

        // public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        // public bool EmailConfirmed { get; set; }

        public void BuyProduct(Product product, IMainRepository repository)
        {
            //Userbase.LoadUser();
            if (Money >= product.FinalPrice && !product.IsBought(repository))
            {
                // todo: add and save product to profile
                Money -= product.FinalPrice;
                
                BoughtProductIDs.Add(product.ID.ToString());

                repository.AddBoughtProduct(new BoughtProduct
                {
                    AppUserRefId = CatalogViewModel.CurrentUser.ID,
                    ProductRefId = product.ID
                });
                Console.WriteLine($"{product.Name} is bought!");
            }
            else
            {
                Console.WriteLine("User don't have enough money or product is already bought!");
            }
            Userbase.SaveUser();
        }
        public void SellProduct(Product product, IMainRepository repo)
        {
            //Userbase.LoadUser();
            if (product.IsBought(repo))
            {
                Money += product.FinalPrice;
                
                BoughtProductIDs.Remove(product.ID.ToString());

                repo.DeleteBoughtProduct(ID, product.ID);
                Console.WriteLine($"{product.Name} is sold!");
            }
            Userbase.SaveUser();
        }
        [Obsolete]
        public bool HasProductBought(int productID)
        {
            return BoughtProductIDs.Contains(productID.ToString());
        }
        public bool HasProductBought(int productID, IMainRepository repository)
        {
            return BoughtProductIDs.Contains(productID.ToString());
        }
        public bool HasProductAdded(string productID)
        {
            return false;
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
