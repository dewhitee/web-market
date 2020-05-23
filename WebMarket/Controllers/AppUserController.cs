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
            return View();
        }
        
        public IActionResult ChangeAddedView()
        {
            AddedProductsViewModel.ViewVariant = AddedProductsViewModel.ViewVariant == Variant.Main ? Variant.Table : Variant.Main;
            return View("Added");
        }
        
        public IActionResult Bought()
        {
            return View();
        }

        public IActionResult ChangeBoughtView()
        {
            BoughtProductsViewModel.ViewVariant = BoughtProductsViewModel.ViewVariant == Variant.Main ? Variant.Table : Variant.Main;
            return View("Bought");
        }
    }
}