using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using WebMarket.Models;
using WebMarket.Data;
using System.Web;

namespace WebMarket.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ComparisonController : Controller
    {
        private readonly IMainRepository mainRepository;

        public ComparisonController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor contextAccessor,
            IMainRepository mainRepository)
        {
            Userbase.LoadData();
            Userbase.Set(signInManager, userManager, contextAccessor.HttpContext.User);
            this.mainRepository = mainRepository;
        }

        [HttpGet]
        public IActionResult Comparison()
        {
            CatalogViewModel.LoadProducts();
            return View();
        }

        public IActionResult FindProducts(string lproductName, string rproductName)
        {
            ComparisonViewModel.LeftProduct = /*CatalogViewModel.GetProduct(lproductName)*/mainRepository.GetProductsByName(lproductName).FirstOrDefault();
            ComparisonViewModel.RightProduct = /*CatalogViewModel.GetProduct(rproductName)*/mainRepository.GetProductsByName(rproductName).FirstOrDefault();
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

        //[Produces("application/json")]
        //[HttpGet("search")]
        //public async Task<IActionResult> Search()
        //{
        //    try
        //    {
        //        string term = HttpContext.Request.Query["term"].ToString();
        //        var prodName = mainRepository.Search(term);
        //        return Ok(prodName);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}