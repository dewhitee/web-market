using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebMarket.Models;
using WebMarket.Data;

namespace WebMarket.Controllers
{
    //using Product = CatalogViewModel.Product;
    //using User = CatalogViewModel.User;

    public class CatalogController : Controller
    {
        private string saveUserFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\user_" + CatalogViewModel.CurrentUser.Username + "_.dew"; }

        public IActionResult Catalog()
        {
            LoadProducts();
            LoadUser();
            //UpdateAllExistedProducts();
            //SaveProducts();
            return View();
        }

        public IActionResult ChangeView(CatalogViewModel.CatalogViewVariant viewVariant)
        {
            CatalogViewModel.ViewVariant = viewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main;

            return RedirectToAction("Catalog");
        }

        public IActionResult AddProduct(string productName, string productType, decimal productCost, float productDiscount, string productDescription,
            string productImageLink, string secondImageLink, string thirdImageLink, string productLink)
        {
            int integralCost = (int)Math.Truncate(productCost);
            int fractionalCost = (int)(productCost - integralCost);

            if (!CatalogViewModel.ContainsName(productName))
            {
                CatalogViewModel.ListOfProducts.Add(new Product
                {
                    ID = Product.MakeNewID(),
                    Name = productName,
                    Type = Product.CheckTypeString(productType),
                    Price = productCost,
                    CostIntegral = integralCost,
                    CostFractional = fractionalCost,
                    Discount = productDiscount,
                    Description = (productDescription != null && productDescription.Length > 0) ? productDescription : "test description",
                    CardImageLink = (productImageLink != null && productImageLink.Length > 0) ? productImageLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                    SecondImageLink = secondImageLink,
                    ThirdImageLink = thirdImageLink,
                    Link = productLink,
                    AddedDate = DateTime.Today
                });
            }
            SaveProducts();
            return RedirectToAction("Catalog");
        }

        public IActionResult BuyProduct(string productName, int productID)
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
            Buy(productName, productID);
            SaveUser();
            SaveProducts();
            return RedirectToAction("Catalog");

        }
        private void Buy(string productName, int productID)
        {
            foreach (var product in CatalogViewModel.ListOfProducts)
            {
                if (product.ID == productID || product.Name == productName)
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

        public IActionResult SellProduct(string productName, int productID)
        {
            Console.WriteLine("Selling Product...");
            Product toSell = new Product();
            foreach (var product in CatalogViewModel.ListOfProducts)
            {
                if (product.ID == productID || product.Name == productName)
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

        public IActionResult AddComment(string commentSection, int productID, float rating)
        {
            Console.WriteLine("Adding comment");
            var product = CatalogViewModel.GetProduct(productID);
            if (product.Comments == null) // is needed for old products that do not have comments list instantiated
                product.Comments = new List<UserComment>();

            bool canAdd = product.OnlyOneCommentPerUser ? product.Comments.Find(x => x.UserID == CatalogViewModel.CurrentUser.ID) == null : true;
            if (canAdd)
            {
                product.Comments.Add(new UserComment
                {
                    Text = commentSection,
                    UserID = CatalogViewModel.CurrentUser.ID,
                    Rate = rating
                });
            }

            SaveProducts();
            if (!product.IsBought)
            {
                return RedirectToAction("Buying");
            }
            else
            {
                return RedirectToAction("Selling");
            }
        }

        public IActionResult SaveProducts()
        {
            CatalogViewModel.SaveProducts();
            SaveUser();

            return Ok();
        }
        public IActionResult SaveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveUserFilePath, FileMode.Open, FileAccess.Write);

            bf.Serialize(stream, CatalogViewModel.CurrentUser);
            stream.Close();

            Userbase.SaveMoney();

            return Ok();
        }

        public IActionResult LoadProducts()
        {
            CatalogViewModel.LoadProducts();
            LoadUser();

            return Ok();
        }
        public IActionResult LoadUser()
        {
            if (CatalogViewModel.CurrentUser == null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                Stream stream = new FileStream(saveUserFilePath, FileMode.Open, FileAccess.Read);
                CatalogViewModel.CurrentUser = (User)bf.Deserialize(stream);
                stream.Close();
            }
            return Ok();
        }

        public IActionResult AddToCart(string productName, int productIndex)
        {
            for (int i = 0; i < CatalogViewModel.ListOfProducts.Count; i++)
            {
                CatalogViewModel.ListOfProducts[i].AddedToCart = false;
            }
            CatalogViewModel.ListOfProducts[productIndex].AddedToCart = true;

            if (!CatalogViewModel.AddedToCartProducts.Contains(CatalogViewModel.ListOfProducts[productIndex]))
                CatalogViewModel.AddedToCartProducts.Add(CatalogViewModel.ListOfProducts[productIndex]);

            // temporally will be redirecting to the Buying page
            if (!CatalogViewModel.ListOfProducts[productIndex].IsBought)
            {
                SaveProducts();
                return RedirectToAction("Buying");
            }
            else
            {
                SaveProducts();
                return RedirectToAction("Selling");
            }

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

        private void UpdateAllExistedProducts()
        {
            foreach (var prod in CatalogViewModel.ListOfProducts)
            {
                prod.ID = Product.MakeNewID();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Buying()
        {
            LoadProducts();
            LoadUser();
            return View();
        }

        public IActionResult Selling()
        {
            LoadProducts();
            LoadUser();
            return View();
        }
    }
}