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

            var listOfProductTypes = (from pt in mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name);

            var model = new CatalogViewModel
            {
                listOfProducts = CatalogViewModel.ListOfProducts != null ? CatalogViewModel.ListOfProducts : new List<Product>(),
                listOfProductTypes = listOfProductTypes,
                findTags = CatalogViewModel.FindTags != null ? CatalogViewModel.FindTags : new List<string>(),
                fullyMatching = CatalogViewModel.FullyMatching
            };

            return View(model);
        }

        public IActionResult Sorted(int sortOptionIndex)
        {
            var products = mainRepository.GetAllProducts();
            var listOfProducts = products.Take(_catalogLength > 0 ? _catalogLength : products.Count()).ToList();

            SortProducts(sortOptionIndex, ref listOfProducts);

            var listOfProductTypes = (from pt in mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name);

            var model = new CatalogViewModel
            {
                listOfProducts = listOfProducts,
                listOfProductTypes = listOfProductTypes,
                findTags = CatalogViewModel.FindTags != null ? CatalogViewModel.FindTags : new List<string>(),
                fullyMatching = CatalogViewModel.FullyMatching
            };
            return View("Catalog", model);
        }

        public IActionResult ChangeView(CatalogViewModel.CatalogViewVariant viewVariant)
        {
            CatalogViewModel.ViewVariant = (CatalogViewModel.ViewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main);

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

        private void SortProducts(int sortOptionIndex, ref List<Product> products)
        {
            switch (sortOptionIndex)
            {
                case (int)CatalogViewModel.ProductSort.None:
                    return;
                case (int)CatalogViewModel.ProductSort.Name:
                    products.Sort(Product.CompareByName);
                    return;
                case (int)CatalogViewModel.ProductSort.Type:
                    products.Sort(Product.CompareByType);
                    return;
                case (int)CatalogViewModel.ProductSort.Price:
                    products.Sort(Product.CompareByPrice);
                    return;
                case (int)CatalogViewModel.ProductSort.Discount:
                    products.Sort(Product.CompareByDiscount);
                    return;
                case (int)CatalogViewModel.ProductSort.FinalPrice:
                    products.Sort(Product.CompareByFinalPrice);
                    return;
                default:
                    return;
            }
        }

        public IActionResult SortByName()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByName);
            return RedirectToAction("Sorted", new { sortBy = "by Name" });;
        }
        public IActionResult SortByType()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByType);
            return RedirectToAction("Sorted", new { sortBy = "by Type" });
        }
        public IActionResult SortByPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByPrice);
            return RedirectToAction("Sorted", new { sortBy = "by Price" });
        }
        public IActionResult SortByDiscount()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByDiscount);
            return RedirectToAction("Sorted", new { sortBy = "by Discount" });
        }
        public IActionResult SortByFinalPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByFinalPrice);
            return RedirectToAction("Sorted", new { sortBy = "by Final Price", products = CatalogViewModel.ListOfProducts });
        }

        public IActionResult SubmitTags(string[] findTags)
        {
            if (findTags != null)
                CatalogViewModel.FindTags = new List<string>(findTags);
            else return View("Error");
            return RedirectToAction("Catalog", new { findTags = CatalogViewModel.FindTags });
        }

        public IActionResult SubmitShowProducts(int fullyMatching, int catalogLength)
        {
            CatalogViewModel.FullyMatching = fullyMatching > 0 ? true : false;
            _catalogLength = catalogLength;
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}