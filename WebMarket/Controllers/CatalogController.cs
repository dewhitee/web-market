using System;
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

        public IActionResult AddComment(string commentSection, int productID, float rating)
        {
            Console.WriteLine("Adding comment");
            var product = mainRepository.GetProduct(productID);

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
                mainRepository.AddUserComment(newComment);
            }
            return RedirectToAction("Page", "Product");
        }

        public IActionResult AddToCart(int productId)
        {
            var product = mainRepository.GetProduct(productId);

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
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByType()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByType);
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByPrice);
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByDiscount()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByDiscount);
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByFinalPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByFinalPrice);
            return RedirectToAction("Sorted");
        }

        public IActionResult SubmitTags(string[] findTags)
        {
            if (findTags != null)
                CatalogViewModel.FindTags = new List<string>(findTags);
            else return View("Error");
            return RedirectToAction("Catalog");
        }

        public IActionResult SubmitShowProducts(int catalogLength)
        {
            _catalogLength = catalogLength;
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}