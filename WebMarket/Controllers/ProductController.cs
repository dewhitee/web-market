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
using Microsoft.AspNetCore.Identity;

namespace WebMarket.Controllers
{
    public class ProductController : Controller
    {
        private static List<string> _tags = null;
        private static Product _addedProduct = null;
        private readonly UserManager<AppUser> userManager;

        //private static Product _editProduct = null;

        private readonly IMainRepository mainRepository;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;

        [Obsolete]
        public ProductController(
            UserManager<AppUser> userManager,
            IMainRepository mainRepository,
            IHostingEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.mainRepository = mainRepository;
            this.hostingEnvironment = hostingEnvironment;
        }


        // GET: AddProduct
        public ActionResult Add()
        {
            //LoadProducts();
            //LoadUser();
            List<string> listOfProductTypes = new List<string>();
            listOfProductTypes = (from pt in mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name).ToList();

            ViewBag.ListOfProductTypes = listOfProductTypes;

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

        public IActionResult EditImages(int prodId)
        {
            var images = mainRepository.GetImagesByProductID(prodId).ToList();
            return View("EditImagesView", new EditImagesViewModel
            {
                ProductId = prodId,
                FirstImageLink = images[0].Link,
                FirstImageDescription = images[0].Description,
                SecondImageLink = images[1].Link,
                SecondImageDescription = images[1].Description,
                ThirdImageLink = images[2].Link,
                ThirdImageDescription = images[2].Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditImages([FromForm]EditImagesViewModel model, int prodid)
        {
            try
            {
                var images = mainRepository.GetImagesByProductID(prodid).ToList();

                var firstImage = mainRepository.GetImageByOrderIndex(prodid, 0);
                firstImage.Link = model.FirstImageLink;
                firstImage.Description = model.FirstImageDescription;

                var secondImage = mainRepository.GetImageByOrderIndex(prodid, 1);
                secondImage.Link = model.SecondImageLink;
                secondImage.Description = model.SecondImageDescription;

                var thirdImage = mainRepository.GetImageByOrderIndex(prodid, 2);
                thirdImage.Link = model.ThirdImageLink;
                thirdImage.Description = model.ThirdImageDescription;

                mainRepository.UpdateImage(firstImage);
                mainRepository.UpdateImage(secondImage);
                mainRepository.UpdateImage(thirdImage);
                ///mainRepository.UpdateProduct(product);
                return RedirectToAction("Catalog", "Catalog");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([FromForm]AddProductViewModel model)
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

                int newID = Product.MakeNewID(mainRepository);

                bool attached = AttachTags(newID);

                Product newProduct = new Product
                {
                    ID = newID,
                    Name = model.Name,
                    Type = Product.CheckTypeString(model.Type),
                    //Tags = _tags != null ? _tags : new List<string>(),
                    Price = model.Price,
                    Discount = model.Discount,
                    Description = model.Description,
                    //FirstImage = new Product.Image
                    //{
                    //    Link = (model.FirstImageLink != null && model.FirstImageLink.Length > 0) ? model.FirstImageLink
                    //    : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                    //    Description = model.FirstImageDescription
                    //},
                    //SecondImage = new Product.Image
                    //{
                    //    Link = model.SecondImageLink,
                    //    Description = model.SecondImageDescription
                    //},
                    //ThirdImage = new Product.Image
                    //{
                    //    Link = model.ThirdImageLink,
                    //    Description = model.ThirdImageDescription
                    //},
                    Link = model.Link,
                    FileName = uniqueFileName,
                    AddedDate = DateTime.Today,
                    OwnerID = /*Userbase.CurrentAppUser.Id*/userManager.GetUserId(User)
                };

                mainRepository.AddProduct(newProduct);

                AttachImages(newID,
                    model.FirstImageLink, model.FirstImageDescription,
                    model.SecondImageLink, model.SecondImageDescription,
                    model.ThirdImageLink, model.ThirdImageDescription);

                _tags = null;
                _addedProduct = !attached ? newProduct : null;
                //SaveProducts();

                return RedirectToAction("Catalog", "Catalog");
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

        private bool AttachTags(int productID)
        {
            if (_tags != null)
            {
                foreach (var tag in _tags)
                {
                    mainRepository.AddTag(new Tag
                    {
                        ProductID = productID.ToString(),
                        //Text = tag,
                        TypeId = mainRepository.GetProductTypeByName(tag).ID
                    });
                }
                _tags = null;
                return true;
            }
            return false;
        }

        private void AttachImages(
            int productID,
            string firstLink,
            string firstDesc,
            string secondLink,
            string secondDesc,
            string thirdLink,
            string thirdDesc)
        {
            string id = productID.ToString();
            mainRepository.AddImage(new Image
            {
                ProductID = id,
                Link = (firstLink != null && firstLink.Length > 0) ? firstLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                Description = firstDesc,
                OrderIndex = 0
            });
            mainRepository.AddImage(new Image
            {
                ProductID = id,
                Link = secondLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                Description = secondDesc,
                OrderIndex = 1
            });
            mainRepository.AddImage(new Image
            {
                ProductID = id,
                Link = thirdLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                Description = thirdDesc,
                OrderIndex = 2
            });
        }

        public IActionResult OpenEditProduct(int prodId)
        {
            var product = mainRepository.GetProduct(prodId);
            return View("Edit", product);
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
        public ActionResult Edit([FromForm]Product product)
        {
            try
            {
                mainRepository.UpdateProduct(product);
                return RedirectToAction("Catalog", "Catalog");
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
        public ActionResult Delete(string deleteId)
        {
            try
            {
                int id = int.Parse(deleteId);
                CleanTags(id);

                mainRepository.DeleteProduct(int.Parse(deleteId));
                return RedirectToAction("Catalog", "Catalog");
            }
            catch
            {
                return RedirectToAction("Catalog", "Catalog");
            }
        }

        private void CleanTags(int id)
        {
            foreach (var i in mainRepository.GetTagsByProductID(id).ToList())
            {
                mainRepository.DeleteTag(i.ID);
            }
        }

        public IActionResult Page()
        {
            return View();
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