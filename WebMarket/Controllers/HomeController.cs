using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    using Product = CatalogViewModel.Product;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            //return View();
            List<Product> products = new List<Product>();
            products.Add(new Product {
                ProductIndex = 4,
                ProductName = "AnotherGame",
                ProductType = "Game",
                CostIntegral = 18,
                CostFractional = 99,
                Discount = 0f,
                Description = "No description"
            });

            return View(new CatalogViewModel { Products = products });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
