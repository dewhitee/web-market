using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;
using WebMarket.Models.AppUserModels;

namespace WebMarket.Controllers
{
    using Variant = Models.CatalogViewModel.CatalogViewVariant;
    public class AppUserController : Controller
    {
        private readonly IMainRepository mainRepository;
        private readonly UserManager<AppUser> userManager;

        public AppUserController(
            IMainRepository mainRepository,
            UserManager<AppUser> userManager)
        {
            this.mainRepository = mainRepository;
            this.userManager = userManager;
        }

        public IActionResult Account()
        {
            return View();
        }
         
        public IActionResult AddMoney(decimal addMoney)
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                try
                {
                    user.Money += addMoney;

                    userManager.UpdateAsync(user).Wait();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return RedirectToAction("Account");
        }
        public IActionResult Stats()
        {
            var userId = userManager.GetUserId(User);
            var userProducts = mainRepository.GetAllProductsOfUser(userId).ToList();

            int commentsGot = 0;
            foreach (var p in userProducts)
            {
                var productComments = mainRepository.GetUserCommentsByProdID(p.ID).ToList();
                commentsGot += productComments.Count();
            }

            var commentsWritten = mainRepository.GetUserCommentsByUserID(userId).Count();
            var totalAdded = mainRepository.GetAllProductsOfUser(userId).Count();
            var totalBought = mainRepository.GetBoughtProductsByUserId(userId).Count();
            var totalStars = (from p in userProducts select p.GetTotalStarsCount(mainRepository)).Sum(x => Convert.ToInt32(x));

            StatsViewModel model = new StatsViewModel
            {
                TotalCommentsWritten = commentsWritten,
                TotalCommentsGot = commentsGot,
                TotalProductsAdded = totalAdded,
                TotalProductsBought = totalBought,
                TotalStarsAmount = totalStars
            };
            return View(model);
        }

        public IActionResult Added()
        {
            var products = mainRepository.GetAllProductsOfUser(userManager.GetUserId(User)).ToList();
            return View(new AddedProductsViewModel
            {
                AddedProducts = products
            });
        }
        
        public IActionResult ChangeAddedView()
        {
            AddedProductsViewModel.ViewVariant = AddedProductsViewModel.ViewVariant == Variant.Main ? Variant.Table : Variant.Main;
            return RedirectToAction("Added");
        }
        
        public IActionResult Bought()
        {
            var boughtProducts = mainRepository.GetBoughtProductsByUserId(userManager.GetUserId(User)).ToList();
            var products = mainRepository.GetProductsByBought(boughtProducts).ToList();
            return View(new BoughtProductsViewModel
            {
                BoughtProducts = products
            });
        }

        public IActionResult ChangeBoughtView()
        {
            BoughtProductsViewModel.ViewVariant = BoughtProductsViewModel.ViewVariant == Variant.Main ? Variant.Table : Variant.Main;
            return RedirectToAction("Bought");
        }

        public IActionResult AddedSorted(int sortOptionIndex)
        {
            var products = mainRepository.GetAllProductsOfUser(userManager.GetUserId(User)).ToList();

            SortProducts(sortOptionIndex, ref products);

            var model = new AddedProductsViewModel
            {
                AddedProducts = products
            };
            return View("Added", model);
        }

        public IActionResult BoughtSorted(int sortOptionIndex)
        {
            var boughtProducts = mainRepository.GetBoughtProductsByUserId(userManager.GetUserId(User)).ToList();
            var products = mainRepository.GetProductsByBought(boughtProducts).ToList();

            SortProducts(sortOptionIndex, ref products);

            var model = new BoughtProductsViewModel
            {
                BoughtProducts = products
            };
            return View("Bought", model);
        }

        private void SortProducts(int sortOptionIndex, ref List<Product> products)
        {
            switch (sortOptionIndex)
            {
                case (int)Product.SortOption.None:
                    return;
                case (int)Product.SortOption.Name:
                    products.Sort(Product.CompareByName);
                    return;
                case (int)Product.SortOption.Type:
                    products.Sort(Product.CompareByType);
                    return;
                case (int)Product.SortOption.AddedDate:
                    products.Sort(Product.CompareByDate);
                    return;
                default:
                    return;
            }
        }
    }
}