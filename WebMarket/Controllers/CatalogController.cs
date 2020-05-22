﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebMarket.Models;
using WebMarket.Data;

namespace WebMarket.Controllers
{
    public class CatalogController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        private readonly IMainRepository mainRepository;
        private static int _catalogLength = 0;

        public CatalogController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IHttpContextAccessor contextAccessor,
            IMainRepository productRepository)
        {
            ///Userbase.LoadData();
            Userbase.Set(signInManager, userManager, contextAccessor.HttpContext.User);
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mainRepository = productRepository;
        }

        public IActionResult Catalog()
        {
            var products = mainRepository.GetAllProducts();
            CatalogViewModel.ListOfProducts = products.Take(_catalogLength > 0 ? _catalogLength : products.Count()).ToList();

            List<string> listOfProductTypes = new List<string>();
            listOfProductTypes = (from pt in mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name).ToList();

            ViewBag.ListOfProductTypes = listOfProductTypes;

            CatalogViewModel.ListOfProductTypes = (from pt in mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name);

            return View();
        }

        //private bool ProductsInitialized()
        //{
        //    var sortedProducts = new List<Product>(CatalogViewModel.ListOfProducts);
        //    return mainRepository.GetAllProducts().All(sortedProducts.Contains);
        //}

        public IActionResult Sorted()
        {
            return View("Catalog");
        }

        public IActionResult ChangeView(CatalogViewModel.CatalogViewVariant viewVariant)
        {
            CatalogViewModel.ViewVariant = viewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main;

            return RedirectToAction("Catalog");
        }

        //public IActionResult BuyProduct(string productName, int productID)
        //{
        //    Console.WriteLine("Buying Product...");
        //    var product = from p in mainRepository.GetAllProducts() where p.ID == productID select p;
        //    if (product.Any())
        //    {
        //        var user = userManager.GetUserAsync(User).Result;
        //        var productToBuy = product.FirstOrDefault();
        //        if (user != null)
        //        {
        //            if (user.Money >= productToBuy.FinalPrice && !productToBuy.IsBought(mainRepository, user))
        //            {
        //                user.Money -= productToBuy.FinalPrice;

        //                userManager.UpdateAsync(user);

        //                mainRepository.AddBoughtProduct(new BoughtProduct
        //                {
        //                    AppUserRefId = user.Id,
        //                    ProductRefId = productToBuy.ID
        //                });
        //                Console.WriteLine($"{productToBuy.Name} is bought!");
        //            }
        //            else
        //            {
        //                Console.WriteLine("User don't have enough money or product is already bought!");
        //            }
        //        }
        //    }
        //    return RedirectToAction("Catalog");

        //}

        //public IActionResult SellProduct(string productName, int productID)
        //{
        //    var product = from p in mainRepository.GetAllProducts() where p.ID == productID select p;
        //    if (product.Any())
        //    {
        //        var user = userManager.GetUserAsync(User).Result;
        //        var productToSell = product.FirstOrDefault();
        //        if (user != null && productToSell.IsBought(mainRepository, user))
        //        {
        //            user.Money += productToSell.FinalPrice;

        //            userManager.UpdateAsync(user);

        //            mainRepository.DeleteBoughtProduct(user.Id, productToSell.ID);
        //            Console.WriteLine($"{productToSell.Name} is sold!");
        //        }
        //    }

        //    return RedirectToAction("Catalog");
        //}

        public IActionResult AddComment(string commentSection, int productID, float rating)
        {
            Console.WriteLine("Adding comment");
            var product = /*CatalogViewModel.GetProduct(productID)*/mainRepository.GetProduct(productID);

            bool canAdd = product.OnlyOneCommentPerUser ? mainRepository.GetUserCommentsByProdID(product.ID).FirstOrDefault() == null : true;
            if (canAdd)
            {
                UserComment newComment = new UserComment
                {
                    Text = commentSection,
                    ProductID = product.ID.ToString(),
                    UserID = userManager.GetUserId(User),
                    Rate = rating
                };
                //product.Comments.Add(newComment);
                mainRepository.AddUserComment(newComment);
            }

            ///SaveProducts();
            return RedirectToAction("Page", "Product");
        }

        public IActionResult SaveProducts()
        {
            CatalogViewModel.SaveProducts();
            return Ok();
        }
        public IActionResult SaveUser()
        {
            Userbase.SaveUser();
            return Ok();
        }

        public IActionResult LoadProducts()
        {
            while (!Userbase.IsInitialized) { }
            CatalogViewModel.LoadProducts();
            LoadUser();

            return Ok();
        }
        public IActionResult LoadUser()
        {
            while (!Userbase.IsInitialized) { }
            ///Userbase.LoadUser();
            return Ok();
        }

        //public IActionResult GetBlobDownload([FromQuery] string link)
        //{
        //    var net = new System.Net.WebClient();
        //    var data = net.DownloadData(link);
        //    var content = new System.IO.MemoryStream(data);
        //    var contentType = "APPLICATION/octet-stream";
        //    var fileName = "test_Downloaded"
        //}

        //[HttpPost]
        //public string[] UploadFiles()
        //{
            //HttpFileCollection
        //}

        public IActionResult AddToCart(/*string productName, int productIndex, */int productId)
        {
            Userbase.LoadUser();
            var product = mainRepository.GetProduct(productId);
            if (product != null)
            {
                product.AddedToCart = true;
            }

            if (!CatalogViewModel.AddedToCartProducts.Contains(product))
                CatalogViewModel.AddedToCartProducts.Add(product);

            CatalogViewModel.ChoosenProduct = product;
            CatalogViewModel.ChoosenProductID = product.ID;
            return RedirectToAction("Page", "Product");
        }

        public IActionResult SortProducts(int sortOptionIndex)
        {
            switch (sortOptionIndex)
            {
                case (int)CatalogViewModel.ProductSort.None:
                    return RedirectToAction("Catalog");
                case (int)CatalogViewModel.ProductSort.Name:
                    return SortByName();
                case (int)CatalogViewModel.ProductSort.Type:
                    return SortByType();
                case (int)CatalogViewModel.ProductSort.Price:
                    return SortByPrice();
                case (int)CatalogViewModel.ProductSort.Discount:
                    return SortByDiscount();
                case (int)CatalogViewModel.ProductSort.FinalPrice:
                    return SortByFinalPrice();
                default:
                    return RedirectToAction("Catalog");
            }
        }

        public IActionResult SortByName()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByName);
            ///SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByType()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByType);
            ///SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByPrice);
            ///SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByDiscount()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByDiscount);
            ///SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByFinalPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByFinalPrice);
            ///SaveProducts();
            return RedirectToAction("Sorted");
        }

        //private void UpdateAllExistedProducts()
        //{
        //    foreach (var prod in CatalogViewModel.ListOfProducts)
        //    {
        //        prod.ID = Product.MakeNewID();
        //    }
        //}

        public IActionResult SubmitTags(string[] findTags)
        {
            if (findTags != null)
                CatalogViewModel.FindTags = new List<string>(findTags);
            else return View("Error");
            //CatalogViewModel.SaveFindTags();
            return RedirectToAction("Catalog");
        }

        public IActionResult SubmitShowProducts(int catalogLength)
        {
            ///var products = mainRepository.GetAllProducts();
            ///CatalogViewModel.ListOfProducts = (from p in products select p).Take(catalogLength > 0 ? catalogLength : products.Count()).ToList();
            _catalogLength = catalogLength;
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Buying()
        {
            LoadProducts();
            LoadUser();
            return View();
        }

        public IActionResult Selling()
        {
            LoadProducts();
            LoadUser();
            return View();
        }
    }
}