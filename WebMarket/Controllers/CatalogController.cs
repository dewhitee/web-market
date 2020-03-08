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
            //return View();
            List<Product> products = new List<Product>();
            products.Add(new Product
            {
                ID = 4,
                Name = "Heroes® of Might & Magic® III - HD Edition",
                Type = "Game",
                Price = 14.99M,
                CostIntegral = 14,
                CostFractional = 99,
                Discount = 0f,
                Description = "No description"
            });
            products.Add(new Product
            {
                ID = 4,
                Name = "Gears 5",
                Type = "Game",
                Price = 69.99M,
                CostIntegral = 69,
                CostFractional = 99,
                Discount = 5f,
                Description = "No description"
            });
            products.Add(new Product
            {
                ID = 4,
                Name = "AnotherGame",
                Type = "Game",
                Price = 18.99M,
                CostIntegral = 18,
                CostFractional = 99,
                Discount = 0f,
                Description = "No description"
            });
            products.Add(new Product
            {
                ID = 5,
                Name = "AndAnotherGame",
                Type = "Game",
                Price = 14.50M,
                CostIntegral = 14,
                CostFractional = 50,
                Discount = 2f,
                Description = "And Another game description..."
            });
            products.Add(new Product
            {
                ID = 6,
                Name = "Hobbit Ultimate Game",
                Type = "Game",
                Price = 24.99M,
                CostIntegral = 24,
                CostFractional = 99,
                Discount = 15f,
                Description = "HOBBITS YOHOO..."
            });

            return View(new CatalogViewModel { Products = products });
        }

        public IActionResult AddProduct(string productName, string productType, decimal productCost, float productDiscount)
        {
            int integralCost = (int)Math.Truncate(productCost);
            int fractionalCost = (int)(productCost - integralCost);
            CatalogViewModel.ListOfProducts.Add(new Product
            {
                Name = productName,
                Type = productType,
                Price = productCost,
                CostIntegral = integralCost,
                CostFractional = fractionalCost,
                Discount = productDiscount,
                Description = "test description"
            });
            //return Ok();
            return RedirectToAction("Catalog");
            //return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}