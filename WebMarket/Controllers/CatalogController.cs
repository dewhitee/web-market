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
    using User = CatalogViewModel.User;

    public class CatalogController : Controller
    {
        private readonly string saveProductsFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\products.dew";
        private readonly string saveUserFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\user.dew";

        public IActionResult Catalog()
        {
            LoadProducts();
            LoadUser();
            return View();
        }

        public IActionResult ChangeView(CatalogViewModel.CatalogViewVariant viewVariant)
        {
            CatalogViewModel.ViewVariant = viewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main;

            return RedirectToAction("Catalog");
        }

        public IActionResult AddProduct(string productName, string productType, decimal productCost, float productDiscount, string productDescription, string productLink)
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
                    Description = productDescription.Length > 0 ? productDescription : "test description",
                    Link = productLink,
                    AddedDate = DateTime.Today
                });
            }
            SaveProducts();
            return RedirectToAction("Catalog");
        }

        public IActionResult BuyProduct(string productName)
        {
            Console.WriteLine("Buying Product...");
            if (CatalogViewModel.GetSubmitBuyingButtonText() == "Find")
            {
                FindAndBuyProduct(productName);
                return RedirectToAction("Catalog");
            }
            //Product toBuy = new Product();
            //foreach (var product in CatalogViewModel.ListOfProducts)
            //{
            //    if (product.Name == productName)
            //    {
            //        toBuy = product;
            //        CatalogViewModel.CurrentUser.BuyProduct(toBuy);
            //        break;
            //    }
            //}
            Buy(productName);
            SaveUser();
            SaveProducts();
            return RedirectToAction("Catalog");

        }
        private void Buy(string productName)
        {
            foreach (var product in CatalogViewModel.ListOfProducts)
            {
                if (product.Name == productName)
                {
                    CatalogViewModel.CurrentUser.BuyProduct(product);
                    break;
                }
            }
        }

        private void FindAndBuyProduct(string productName)
        {
            foreach (var product in CatalogViewModel.ListOfProducts)
            {
                product.AddedToCart = false;
                if (product.Name == productName && !product.IsBought)
                {
                    product.AddedToCart = true;
                    break;
                }
            }
            SaveUser();
            SaveProducts();
        }

        public IActionResult SellProduct(string productName)
        {
            Console.WriteLine("Selling Product...");
            Product toSell = new Product();
            foreach (var product in CatalogViewModel.ListOfProducts)
            {
                if (product.Name == productName)
                {
                    toSell = product;
                    CatalogViewModel.CurrentUser.SellProduct(toSell);
                    break;
                }
            }
            SaveUser();
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
        public IActionResult SaveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveUserFilePath, FileMode.Open, FileAccess.Write);

            bf.Serialize(stream, CatalogViewModel.CurrentUser);
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
        public IActionResult LoadUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveUserFilePath, FileMode.Open, FileAccess.Read);
            CatalogViewModel.CurrentUser = (User)bf.Deserialize(stream);
            stream.Close();

            return Ok();
        }

        public IActionResult AddToCart(string productName, int productIndex)
        {
            ///productAdded.AddedToCart = true;
            //foreach (var product in CatalogViewModel.ListOfProducts)
            //{
            //    product.AddedToCart = false;
            //    if (product.Name == productName)
            //    {
            //        product.AddedToCart = true;
            //        break;
            //    }
            //}
            for (int i = 0; i < CatalogViewModel.ListOfProducts.Count; i++)
            {
                CatalogViewModel.ListOfProducts[i].AddedToCart = false;
            }
            CatalogViewModel.ListOfProducts[productIndex].AddedToCart = true;
            SaveProducts();
            return RedirectToAction("Catalog");
        }

        //public IActionResult LoadLink()
        //{

        //}

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
        public IActionResult SortByFinalPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByFinalPrice);
            SaveProducts();
            return RedirectToAction("Catalog");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}