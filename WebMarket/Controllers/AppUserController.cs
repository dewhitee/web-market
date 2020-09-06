using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;
using WebMarket.Models.AppUserModels;

namespace WebMarket.Controllers
{
    using Variant = CatalogViewModel.CatalogViewVariant;

    [Authorize]
    public class AppUserController : Controller
    {
        private readonly IMainRepository        _mainRepository;
        private readonly UserManager<AppUser>   _userManager;

        public AppUserController(
            IMainRepository mainRepository,
            UserManager<AppUser> userManager)
        {
            _mainRepository = mainRepository;
            _userManager = userManager;
        }

        public IActionResult Account()
        {
            return View();
        }
         
        public IActionResult AddMoney(decimal addMoney)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                try
                {
                    user.Money += addMoney;

                    _userManager.UpdateAsync(user).Wait();
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
            var userId = _userManager.GetUserId(User);
            var userProducts = _mainRepository.GetAllProductsOfUser(userId).ToList();

            int commentsGot = 0;
            foreach (var p in userProducts)
            {
                var productComments = _mainRepository.GetUserCommentsByProdID(p.ID).ToList();
                commentsGot += productComments.Count();
            }

            var commentsWritten = _mainRepository.GetUserCommentsByUserID(userId).Count();
            var totalAdded = _mainRepository.GetAllProductsOfUser(userId).Count();
            var totalBought = _mainRepository.GetBoughtProductsByUserId(userId).Count();
            var totalStars = (from p in userProducts select p.GetTotalStarsCount(_mainRepository)).Sum(x => Convert.ToInt32(x));

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
            var products = _mainRepository.GetAllProductsOfUser(_userManager.GetUserId(User)).ToList();
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
            var boughtProducts = _mainRepository.GetBoughtProductsByUserId(_userManager.GetUserId(User)).ToList();
            var products = _mainRepository.GetProductsByBought(boughtProducts).ToList();
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
            var products = _mainRepository.GetAllProductsOfUser(_userManager.GetUserId(User)).ToList();

            SortProducts(sortOptionIndex, ref products);

            var model = new AddedProductsViewModel
            {
                AddedProducts = products
            };
            return View("Added", model);
        }

        public IActionResult BoughtSorted(int sortOptionIndex)
        {
            var boughtProducts = _mainRepository.GetBoughtProductsByUserId(_userManager.GetUserId(User)).ToList();
            var products = _mainRepository.GetProductsByBought(boughtProducts).ToList();

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