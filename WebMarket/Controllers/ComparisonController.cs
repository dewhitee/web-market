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

        public ComparisonController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IHttpContextAccessor contextAccessor,
            IMainRepository mainRepository)
        {
            ///Userbase.LoadData();
            ///Userbase.Set(signInManager, userManager, contextAccessor.HttpContext.User);
            this.mainRepository = mainRepository;
        }

        [HttpGet]
        public IActionResult Comparison()
        {
            ///CatalogViewModel.LoadProducts();
            return View();
        }

        public IActionResult FindProducts(string lproductName, string rproductName)
        {
            ComparisonViewModel.LeftProduct = mainRepository.GetProductsByName(lproductName).FirstOrDefault();
            ComparisonViewModel.RightProduct = mainRepository.GetProductsByName(rproductName).FirstOrDefault();
            return RedirectToAction("Comparison");
        }
    }
}