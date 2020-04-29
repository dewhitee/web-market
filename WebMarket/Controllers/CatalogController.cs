using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebMarket.Models;
using WebMarket.Data;

namespace WebMarket.Controllers
{
    public class CatalogController : Controller
    {
        //private string saveUserFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\user_" + CatalogViewModel.CurrentUser.Username + "_.dew"; }
        //System.Security.Claims.ClaimsPrincipal currentUser = User;
        private static List<string> _tags = null;

        public CatalogController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor contextAccessor)
        {
            Userbase.LoadData();
            Userbase.Set(signInManager, userManager, contextAccessor.HttpContext.User);
        }

        public IActionResult Catalog()
        {
            LoadProducts();
            LoadUser();
            //CatalogViewModel.LoadFindTags();
            return View();
        }

        public IActionResult ChangeView(CatalogViewModel.CatalogViewVariant viewVariant)
        {
            CatalogViewModel.ViewVariant = viewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main;

            return RedirectToAction("Catalog");
        }

        public IActionResult AddProduct(
            string productName,
            string productType,
            string[] tags,
            decimal productCost,
            float productDiscount,
            string productDescription,
            string productImageLink,
            string productImageDescription,
            string secondImageLink,
            string secondImageDescription,
            string thirdImageLink,
            string thirdImageDescription,
            string productLink,
            string productFileName,
            int condition)
        {
            int integralCost = (int)Math.Truncate(productCost);
            int fractionalCost = (int)(productCost - integralCost);

            if (!CatalogViewModel.ContainsName(productName) && productName != null && condition != 0)
            {
                CatalogViewModel.ListOfProducts.Add(new Product
                {
                    ID = Product.MakeNewID(),
                    Name = productName,
                    Type = Product.CheckTypeString(productType),
                    Tags = _tags,
                    Price = productCost,
                    CostIntegral = integralCost,
                    CostFractional = fractionalCost,
                    Discount = productDiscount,
                    Description = (productDescription != null && productDescription.Length > 0) ? productDescription : "test description",
                    //CardImageLink = (productImageLink != null && productImageLink.Length > 0) ? productImageLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                    //SecondImageLink = secondImageLink,
                    //ThirdImageLink = thirdImageLink,
                    FirstImage = new Product.Image {
                        Link = (productImageLink != null && productImageLink.Length > 0) ? productImageLink
                        : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                        Description = productImageDescription
                    },
                    SecondImage = new Product.Image {
                        Link = secondImageLink,
                        Description = secondImageDescription
                    },
                    ThirdImage = new Product.Image {
                        Link = thirdImageLink,
                        Description = thirdImageDescription
                    },
                    Link = productLink,
                    FileName = productFileName,
                    AddedDate = DateTime.Today,
                    OwnerID = CatalogViewModel.CurrentUser.ID
                });
                _tags = null;
            }
            else if (/*CatalogViewModel.ContainsName(productName) && Userbase.UserModel.HasProductBought(productName)*/condition == 0)
            {
                //var prod = CatalogViewModel.GetProduct(productName);
                //prod.ID = prod.HasValidID() ? prod.ID : Product.MakeNewID();
                //prod.Name = productName ?? prod.Name;
                //prod.Type = productType ?? prod.Type;
                //prod.Tags = prod.Tags.Count < 0 ? new List<string>(tags) : prod.Tags;
                //prod.Discount = productDiscount;
                if (!CatalogViewModel.ContainsName(productName))
                {
                    _tags = new List<string>(tags);
                }
                else
                {
                    CatalogViewModel.GetProduct(productName).Tags = new List<string>(_tags);
                    _tags = null;
                }
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
                    LoadUser();
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
                    LoadUser();
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
                    UserID = /*CatalogViewModel.CurrentUser.Username != "" ? */CatalogViewModel.CurrentUser.ID/* : "Unknown"*/,
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
            return Ok();
        }
        public IActionResult SaveUser()
        {
            Userbase.SaveUser();
            return Ok();
        }

        public IActionResult LoadProducts()
        {
            while (!Userbase.IsInitialized) { }
            CatalogViewModel.LoadProducts();
            LoadUser();

            return Ok();
        }
        public IActionResult LoadUser()
        {
            while (!Userbase.IsInitialized) { }
            Userbase.LoadUser();
            return Ok();
        }

        //public IActionResult GetBlobDownload([FromQuery] string link)
        //{
        //    var net = new System.Net.WebClient();
        //    var data = net.DownloadData(link);
        //    var content = new System.IO.MemoryStream(data);
        //    var contentType = "APPLICATION/octet-stream";
        //    var fileName = "test_Downloaded"
        //}

        public IActionResult AddToCart(string productName, int productIndex)
        {
            Userbase.LoadUser();
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
        }

        public IActionResult SortProducts(int sortOptionIndex)
        {
            switch (sortOptionIndex)
            {
                case (int)CatalogViewModel.ProductSort.None:
                    return Ok();
                case (int)CatalogViewModel.ProductSort.Name:
                    return SortByName();
                case (int)CatalogViewModel.ProductSort.Type:
                    return SortByType();
                case (int)CatalogViewModel.ProductSort.Price:
                    return SortByPrice();
                case (int)CatalogViewModel.ProductSort.Discount:
                    return SortByDiscount();
                case (int)CatalogViewModel.ProductSort.FinalPrice:
                    return SortByFinalPrice();
                default:
                    return Ok();
            }
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

        public IActionResult SubmitTags(string[] findTags)
        {
            if (findTags != null)
                CatalogViewModel.FindTags = new List<string>(findTags);
            else return View("Error");
            //CatalogViewModel.SaveFindTags();
            return RedirectToAction("Catalog");
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