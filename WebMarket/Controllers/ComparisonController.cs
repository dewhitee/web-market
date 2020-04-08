using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class ComparisonController : Controller
    {
        public IActionResult Comparison()
        {
            return View();
        }

        public IActionResult FindProducts(string lproductName, string rproductName)
        {
            ComparisonViewModel.LeftProduct = CatalogViewModel.GetProduct(lproductName);
            ComparisonViewModel.RightProduct = CatalogViewModel.GetProduct(rproductName);
            return RedirectToAction("Comparison");
        }
    }
}