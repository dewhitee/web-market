using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Models;
using WebMarket.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebMarket.Controllers
{
    public class AddProductController : Controller
    {
        private static List<string> _tags = null;
        private readonly IProductRepository productRepository;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;

        [Obsolete]
        public AddProductController(
            IProductRepository productRepository,
            IHostingEnvironment hostingEnvironment)
        {
            this.productRepository = productRepository;
            this.hostingEnvironment = hostingEnvironment;
        }


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
        //public ActionResult AddProductView()
        //{
        //    return View();
        //}

        [HttpPost]
        public IActionResult AddProductView([FromForm]AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.ZipFile != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "file");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ZipFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.ZipFile.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                //int integralCost = (int)Math.Truncate(model.Price);
                //int fractionalCost = (int)(model.Price - integralCost);

                //if (!CatalogViewModel.ContainsName(model.Name))
                //{
                    Product newProduct = new Product
                    {
                        ID = Product.MakeNewID(productRepository),
                        Name = model.Name,
                        Type = Product.CheckTypeString(model.Type),
                        Tags = _tags != null ? _tags : new List<string>(),
                        Price = model.Price,
                        Discount = model.Discount,
                        Description = model.Description,
                        FirstImage = new Product.Image
                        { 
                            Link = (model.FirstImageLink != null && model.FirstImageLink.Length > 0) ? model.FirstImageLink
                            : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                            Description = model.FirstImageDescription
                        },
                        SecondImage = new Product.Image
                        {
                            Link = model.SecondImageLink,
                            Description = model.SecondImageDescription
                        },
                        ThirdImage = new Product.Image
                        {
                            Link = model.ThirdImageLink,
                            Description = model.ThirdImageDescription
                        },
                        Link = model.Link,
                        FileName = model.FileName,
                        ZipFilePath = uniqueFileName,
                        AddedDate = DateTime.Today,
                        OwnerID = CatalogViewModel.CurrentUser.ID
                    };
                    //CatalogViewModel.ListOfProducts.Add(newProduct);
                    productRepository.Add(newProduct);
                    _tags = null;
                    SaveProducts();
                //}
                return RedirectToAction("AddProductView");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddTags(string productName, string[] tags)
        {
            if (!CatalogViewModel.ContainsName(productName))
            {
                _tags = new List<string>(tags);
            }
            else
            {
                CatalogViewModel.GetProduct(productName).Tags = new List<string>(_tags);
                _tags = null;
            }
            SaveProducts();
            return RedirectToAction("AddProductView");
        }

        // POST: AddProduct/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        collection.Keys

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

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