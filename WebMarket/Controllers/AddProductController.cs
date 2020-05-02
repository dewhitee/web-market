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
        private static Product _addedProduct = null;
        private readonly IMainRepository mainRepository;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;

        [Obsolete]
        public AddProductController(
            IMainRepository mainRepository,
            IHostingEnvironment hostingEnvironment)
        {
            this.mainRepository = mainRepository;
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

                int newID = Product.MakeNewID(mainRepository);

                bool attached = AttachTags(newID);

                Product newProduct = new Product
                {
                    ID = newID,
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
                    //OldFileName = model.OldFileName,
                    FileName = uniqueFileName,
                    AddedDate = DateTime.Today,
                    OwnerID = CatalogViewModel.CurrentUser.ID
                };
                //CatalogViewModel.ListOfProducts.Add(newProduct);
                mainRepository.AddProduct(newProduct);

                _tags = null;
                _addedProduct = !attached ? newProduct : null;
                SaveProducts();
                //}
                return RedirectToAction("AddProductView");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddTags(string productName, string[] tags)
        {
            _tags = new List<string>(tags);
            if (_addedProduct != null)
            {
                AttachTags(_addedProduct.ID);
                _tags = null;
                _addedProduct = null;
            }

            return RedirectToAction("AddProductView");
        }

        bool AttachTags(int productID)
        {
            if (_tags != null)
            {
                foreach (var tag in _tags)
                {
                    mainRepository.AddTag(new Tag
                    {
                        ProductID = productID.ToString(),
                        Text = tag
                    });
                }
                return true;
            }
            return false;
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