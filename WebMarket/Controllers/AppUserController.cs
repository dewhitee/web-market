using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebMarket.Controllers
{
    public class AppUserController : Controller
    {
        public IActionResult Stats()
        {
            return View();
        }

        public IActionResult Added()
        {
            return View();
        }
        // more
    }
}