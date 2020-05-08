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
        private readonly IMainRepository mainRepository;
        //private bool _productsListInitialized => //Enumerable.SequenceEqual(mainRepository.GetAllProducts().OrderBy(p => p),
        //CatalogViewModel.ListOfProducts.OrderBy(t => t));
        //mainRepository.GetAllProducts().All(CatalogViewModel.ListOfProducts.Contains);

        public CatalogController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor contextAccessor,
            IMainRepository productRepository)
        {
            Userbase.LoadData();
            Userbase.Set(signInManager, userManager, contextAccessor.HttpContext.User);
            this.mainRepository = productRepository;
        }

        public IActionResult Catalog()
        {
            //LoadProducts();
            LoadUser();
            //if (!/*_productsListInitialized*/ProductsInitialized())
            //{
                CatalogViewModel.ListOfProducts = mainRepository.GetAllProducts().ToList();
                ///_productsListInitialized = true;
            //}
            //CatalogViewModel.LoadFindTags();
            return View();
        }

        //private bool ProductsInitialized()
        //{
        //    var sortedProducts = new List<Product>(CatalogViewModel.ListOfProducts);
        //    return mainRepository.GetAllProducts().All(sortedProducts.Contains);
        //}

        public IActionResult Sorted()
        {
            return View("Catalog");
        }

        public IActionResult ChangeView(CatalogViewModel.CatalogViewVariant viewVariant)
        {
            CatalogViewModel.ViewVariant = viewVariant == CatalogViewModel.CatalogViewVariant.Main ?
                CatalogViewModel.CatalogViewVariant.Table : CatalogViewModel.CatalogViewVariant.Main;

            return RedirectToAction("Catalog");
        }

        [Obsolete]
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
            byte[] productZipFile,
            int condition)
        {
            //int integralCost = (int)Math.Truncate(productCost);
            //int fractionalCost = (int)(productCost - integralCost);

            if (!CatalogViewModel.ContainsName(productName) && productName != null && condition != 0)
            {
                CatalogViewModel.ListOfProducts.Add(new Product
                {
                    ID = Product.MakeNewID(),
                    Name = productName,
                    Type = Product.CheckTypeString(productType),
                    Tags = _tags,
                    Price = productCost,
                    //CostIntegral = integralCost,
                    //CostFractional = fractionalCost,
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
                    //OldFileName = productFileName,
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
                ///if (!CatalogViewModel.ContainsName(productName))
                ///{
                    ///_tags = new List<string>(tags);
                ///}
                ///else
                ///{
                    ///CatalogViewModel.GetProduct(productName).Tags = new List<string>(_tags);
                    ///_tags = null;
                ///}
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
            foreach (var product in /*CatalogViewModel.ListOfProducts*/mainRepository.GetAllProducts())
            {
                if (product.ID == productID || product.Name == productName)
                {
                    LoadUser();
                    CatalogViewModel.CurrentUser.BuyProduct(product);
                    break;
                }
            }
        }

        [Obsolete]
        private void FindAndBuyProduct(string productName)
        {
            foreach (var product in CatalogViewModel.ListOfProducts)
            {
                //product.AddedToCart = false;
                if (product.Name == productName && !product.IsBought)
                {
                    //product.AddedToCart = true;
                    CatalogViewModel.ChoosenProductID = product.ID;
                    Buy(product.Name, product.ID);
                    break;
                }
            }
            //SaveUser();
            //SaveProducts();
        }

        [Obsolete]
        public IActionResult SellProduct(string productName, int productID)
        {
            Console.WriteLine("Selling Product...");
            Product toSell = new Product();
            foreach (var product in /*CatalogViewModel.ListOfProducts*/mainRepository.GetAllProducts())
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
            var product = /*CatalogViewModel.GetProduct(productID)*/mainRepository.GetProduct(productID);
            ///if (product.Comments == null) // is needed for old products that do not have comments list instantiated
            ///    product.Comments = new List<UserComment>();

            bool canAdd = product.OnlyOneCommentPerUser ? mainRepository.GetUserCommentsByProdID(product.ID).FirstOrDefault() == null : true;
            if (canAdd)
            {
                UserComment newComment = new UserComment
                {
                    Text = commentSection,
                    ProductID = product.ID.ToString(),
                    UserID = CatalogViewModel.CurrentUser.ID,
                    Rate = rating
                };
                //product.Comments.Add(newComment);
                mainRepository.AddUserComment(newComment);
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

        //[HttpPost]
        //public string[] UploadFiles()
        //{
            //HttpFileCollection
        //}

        public IActionResult AddToCart(/*string productName, int productIndex, */int productId)
        {
            Userbase.LoadUser();
            var product = mainRepository.GetProduct(productId);
            if (product != null)
            {
                product.AddedToCart = true;
            }

            if (!CatalogViewModel.AddedToCartProducts.Contains(product))
                CatalogViewModel.AddedToCartProducts.Add(product);

            CatalogViewModel.ChoosenProduct = product;
            CatalogViewModel.ChoosenProductID = product.ID;

            if (!product.IsBought)
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
                    return RedirectToAction("Catalog");
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
                    return RedirectToAction("Catalog");
            }
        }

        public IActionResult SortByName()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByName);
            SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByType()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByType);
            SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByPrice);
            SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByDiscount()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByDiscount);
            SaveProducts();
            return RedirectToAction("Sorted");
        }
        public IActionResult SortByFinalPrice()
        {
            CatalogViewModel.ListOfProducts.Sort(Product.CompareByFinalPrice);
            SaveProducts();
            return RedirectToAction("Sorted");
        }

        //private void UpdateAllExistedProducts()
        //{
        //    foreach (var prod in CatalogViewModel.ListOfProducts)
        //    {
        //        prod.ID = Product.MakeNewID();
        //    }
        //}

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