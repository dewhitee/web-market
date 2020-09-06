using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class CatalogController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMainRepository _mainRepository;

        private static int _catalogLength = 0;
        private static bool _fullyMatching;
        private static List<string> _findTags;

        public CatalogController(
            UserManager<AppUser> userManager,
            IMainRepository productRepository)
        {
           _userManager = userManager;
           _mainRepository = productRepository;
        }

        public IActionResult Catalog()
        {
            var products = _mainRepository.GetAllProducts();

            var listOfProducts = products.Take(_catalogLength > 0 ? _catalogLength : products.Count()).ToList();

            var listOfProductTypes = (from pt in _mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name);

            var model = new CatalogViewModel
            {
                ListOfProducts = listOfProducts != null ? listOfProducts : new List<Product>(),
                ListOfProductTypes = listOfProductTypes,
                FindTags = _findTags != null ? _findTags : new List<string>(),
                FullyMatching = _fullyMatching,
                CatalogLength = _catalogLength
            };

            return View(model);
        }

        public IActionResult Sorted(int sortOptionIndex)
        {
            var products = _mainRepository.GetAllProducts();
            var listOfProducts = products.Take(_catalogLength > 0 ? _catalogLength : products.Count()).ToList();

            SortProducts(sortOptionIndex, ref listOfProducts);

            var listOfProductTypes = (from pt in _mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name);

            var model = new CatalogViewModel
            {
                ListOfProducts = listOfProducts,
                ListOfProductTypes = listOfProductTypes,
                FindTags = _findTags != null ? _findTags : new List<string>(),
                FullyMatching = _fullyMatching,
                CatalogLength = _catalogLength
            };
            return View("Catalog", model);
        }

        public IActionResult ChangeView()
        {
            CatalogViewModel.ViewVariant = (CatalogViewModel.ViewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main);

            return RedirectToAction("Catalog");
        }

        [HttpPost]
        public IActionResult AddComment(string commentSection, int productID, float rating)
        {
            Console.WriteLine("Adding comment");
            var product = _mainRepository.GetProduct(productID);

            bool canAdd = product.OnlyOneCommentPerUser ? _mainRepository.GetUserCommentsByProdID(product.ID).FirstOrDefault() == null : true;
            if (canAdd)
            {
                UserComment newComment = new UserComment
                {
                    Text = commentSection,
                    ProductID = product.ID.ToString(),
                    UserID = _userManager.GetUserId(User),
                    Rate = rating
                };
                _mainRepository.AddUserComment(newComment);
            }
            return RedirectToAction("Page", "Product", product);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _mainRepository.GetProduct(productId);

            CatalogViewModel.ChoosenProductID = product.ID;
            return RedirectToAction("Page", "Product", product);
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
                case (int)Product.SortOption.Price:
                    products.Sort(Product.CompareByPrice);
                    return;
                case (int)Product.SortOption.Discount:
                    products.Sort(Product.CompareByDiscount);
                    return;
                case (int)Product.SortOption.FinalPrice:
                    products.Sort(Product.CompareByFinalPrice);
                    return;
                default:
                    return;
            }
        }

        public IActionResult SubmitTags(string[] findTags)
        {
            if (findTags != null)
                _findTags = new List<string>(findTags);
            else return View("Error");
            return RedirectToAction("Catalog");
        }

        public IActionResult SubmitShowProducts(bool fullyMatching, int catalogLength)
        {
            _fullyMatching = fullyMatching;
            _catalogLength = catalogLength;
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}