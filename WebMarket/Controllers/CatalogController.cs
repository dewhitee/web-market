using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    using Product = CatalogViewModel.Product;

    public class CatalogController : Controller
    {
        private readonly string saveProductsFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\products.dew";

        public IActionResult Catalog()
        {
            LoadProducts();
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
            SaveProducts();
            return RedirectToAction("Catalog");
        }

        public IActionResult SaveProducts()
        {
            //byte[] bytes = null;
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveProductsFilePath, FileMode.Open, FileAccess.Write);
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    bf.Serialize(ms, CatalogViewModel.ListOfProducts);
            //    bytes = ms.ToArray();
            //}
            //System.IO.File.WriteAllBytes(saveProductsFilePath, bytes);

            bf.Serialize(stream, CatalogViewModel.ListOfProducts);
            stream.Close();

            return Ok();
        }

        public IActionResult LoadProducts()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveProductsFilePath, FileMode.Open, FileAccess.Read);
            CatalogViewModel.ListOfProducts = (List<Product>)bf.Deserialize(stream);
            stream.Close();

            return Ok();
        }

        public IActionResult SortByName()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByName);
            SaveProducts();
            return RedirectToAction("Catalog");
        }
        public IActionResult SortByType()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByType);
            SaveProducts();
            return RedirectToAction("Catalog");
        }
        public IActionResult SortByPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByPrice);
            SaveProducts();
            return RedirectToAction("Catalog");
        }
        public IActionResult SortByDiscount()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByDiscount);
            SaveProducts();
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}