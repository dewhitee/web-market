using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;
using System.Web;

namespace WebMarket.Controllers
{
    public class ComparisonController : Controller
    {
        [HttpGet]
        public IActionResult Comparison()
        {
            CatalogViewModel.LoadProducts();
            return View();
        }

        public IActionResult FindProducts(string lproductName, string rproductName)
        {
            ComparisonViewModel.LeftProduct = CatalogViewModel.GetProduct(lproductName);
            ComparisonViewModel.RightProduct = CatalogViewModel.GetProduct(rproductName);
            return RedirectToAction("Comparison");
        }

       [HttpPost]
       public JsonResult Comparison(string prefix)
       {
           var ProductList = (from N in ComparisonViewModel.GetProductNames()
                              where N.StartsWith(prefix)
                              select new { N });
           return Json(ProductList);
       }
    }
}