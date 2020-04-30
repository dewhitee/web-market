using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;
using WebMarket.Data;

namespace WebMarket.Controllers
{
    public class AddProductController : Controller
    {
        private static List<string> _tags = null;

        // GET: AddProduct
        public ActionResult AddProductView()
        {
            LoadProducts();
            LoadUser();
            return View();
        }

        // GET: AddProduct/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddProduct/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddProduct/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddProduct/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddProduct/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
            byte[] productZipFile,
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
                    FirstImage = new Product.Image
                    {
                        Link = (productImageLink != null && productImageLink.Length > 0) ? productImageLink
                        : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                        Description = productImageDescription
                    },
                    SecondImage = new Product.Image
                    {
                        Link = secondImageLink,
                        Description = secondImageDescription
                    },
                    ThirdImage = new Product.Image
                    {
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
            return RedirectToAction("AddProductView");
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
    }
}