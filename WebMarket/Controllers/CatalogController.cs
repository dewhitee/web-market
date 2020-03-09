using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    using Product = CatalogViewModel.Product;

    public class CatalogController : Controller
    {
        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult AddProduct(string productName, string productType, decimal productCost, float productDiscount, string productLink)
        {
            int integralCost = (int)Math.Truncate(productCost);
            int fractionalCost = (int)(productCost - integralCost);

            if (!CatalogViewModel.ContainsName(productName))
            {
                CatalogViewModel.ListOfProducts.Add(new Product
                {
                    Name = productName,
                    Type = Product.CheckTypeString(productType),
                    Price = productCost,
                    CostIntegral = integralCost,
                    CostFractional = fractionalCost,
                    Discount = productDiscount,
                    Description = "test description",
                    Link = productLink
                });
            }
            //return Ok();
            //return View();
            return RedirectToAction("Catalog");
        }

        public IActionResult SortByName()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByName);
            return RedirectToAction("Catalog");
        }
        public IActionResult SortByType()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByType);
            return RedirectToAction("Catalog");
        }
        public IActionResult SortByPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByPrice);
            return RedirectToAction("Catalog");
        }
        public IActionResult SortByDiscount()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByDiscount);
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}