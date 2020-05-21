using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using WebMarket.Models;
using WebMarket.Data;

namespace WebMarket.Controllers
{
    //using Product = CatalogViewModel.Product;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // background
        private static bool _backgroundDefault { get; set; }
        public static string BackgroundClass { get => !_backgroundDefault ? "" : "gradient-background"; }

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            ///Userbase.LoadData();
            Userbase.Set(signInManager, userManager, contextAccessor.HttpContext.User);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeTheme()
        {
            _backgroundDefault = !_backgroundDefault;
            ViewBag.ErrorMessage = "Theme changing on the web-page is currently in development.";
            return View("NotFound");
        }
    }
}
