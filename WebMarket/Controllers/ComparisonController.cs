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
using Microsoft.AspNetCore.Hosting;

namespace WebMarket.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ComparisonController : Controller
    {
        private readonly IMainRepository mainRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ComparisonController(
            IMainRepository mainRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.mainRepository = mainRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Comparison()
        {
            return View(new ComparisonViewModel
            {
                LeftProduct = null,
                RightProduct = null
            });
        }

        public IActionResult FindProducts(string lproductName, string rproductName)
        {
            var leftProduct = mainRepository.GetProductsByName(lproductName).FirstOrDefault();
            var rightProduct = mainRepository.GetProductsByName(rproductName).FirstOrDefault();

            int leftProductTagsCount = 0;
            int leftProductBoughtTimes = 0;
            long leftProductFileSize = 0;
            if (leftProduct != null)
            {
                leftProductTagsCount = mainRepository.GetTagsByProductID(leftProduct.ID).Count();
                leftProductBoughtTimes = (from bp in mainRepository.GetAllBoughtProducts()
                                          where bp.ProductRefId == leftProduct.ID
                                          select bp).Count();
                leftProductFileSize = leftProduct.GetFileSize(webHostEnvironment);
            }

            int rightProductTagsCount = 0;
            int rightProductBoughtTimes = 0;
            long rightProductFileSize = 0;
            if (rightProduct != null)
            {
                rightProductTagsCount = mainRepository.GetTagsByProductID(rightProduct.ID).Count();
                rightProductBoughtTimes = (from bp in mainRepository.GetAllBoughtProducts()
                                           where bp.ProductRefId == rightProduct.ID
                                           select bp).Count();
                rightProductFileSize = rightProduct.GetFileSize(webHostEnvironment);
            }

            var model = new ComparisonViewModel
            {
                LeftProduct = leftProduct,
                LeftProductName = lproductName,
                LeftProductTagsCount = leftProductTagsCount,
                LeftProductBoughtTimes = leftProductBoughtTimes,
                LeftProductFileSize = leftProductFileSize,

                RightProduct = rightProduct,
                RightProductName = rproductName,
                RightProductTagsCount = rightProductTagsCount,
                RightProductBoughtTimes = rightProductBoughtTimes,
                RightProductFileSize = rightProductFileSize
            };
            if (rightProduct == null)
                model.RightProductNotFound = true;
            if (leftProduct == null)
                model.LeftProductNotFound = true;

            return View("Comparison", model);
        }
    }
}