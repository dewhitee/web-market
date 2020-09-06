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
using Microsoft.AspNetCore.Authorization;
using WebMarket.Models.ProductModels;
using Microsoft.EntityFrameworkCore;

namespace WebMarket.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly UserManager<AppUser>   _userManager;
        private readonly IMainRepository        _mainRepository;
        private readonly IWebHostEnvironment    _hostingEnvironment;

        private static List<string> _tags = null;
        private static Product      _addedProduct = null;

        public ProductController(
            UserManager<AppUser> userManager,
            IMainRepository mainRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _mainRepository = mainRepository;
            _hostingEnvironment = hostingEnvironment;
        }


        // GET: AddProduct
        public ActionResult Add()
        {
            //LoadProducts();
            //LoadUser();
            List<string> listOfProductTypes = new List<string>();
            listOfProductTypes = (from pt in _mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name).ToList();

            ViewBag.ListOfProductTypes = listOfProductTypes;

            return View();
        }

        // GET: AddProduct/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult EditImages(int prodId)
        {
            var images = _mainRepository.GetImagesByProductID(prodId).ToList();
            if (images.Count() >= 3)
            {
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
            else
            {
                return View("EditImagesView", new EditImagesViewModel
                { 
                    ProductId = prodId
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditImages([FromForm]EditImagesViewModel model, int prodid)
        {
            try
            {
                var images = _mainRepository.GetImagesByProductID(prodid).ToList();

                var firstImage = _mainRepository.GetImageByOrderIndex(prodid, 0);
                if (model.FirstImageFile != null)
                {
                    firstImage.Link = GetImageFilePath(model.FirstImageFile);
                }
                else
                {
                    firstImage.Link = model.FirstImageLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
                }
                firstImage.Description = model.FirstImageDescription;

                var secondImage = _mainRepository.GetImageByOrderIndex(prodid, 1);
                if (model.SecondImageFile != null)
                {
                    secondImage.Link = GetImageFilePath(model.SecondImageFile);
                }
                else
                {
                    secondImage.Link = model.SecondImageLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
                }
                secondImage.Description = model.SecondImageDescription;

                var thirdImage = _mainRepository.GetImageByOrderIndex(prodid, 2);
                if (model.ThirdImageFile != null)
                {
                    thirdImage.Link = GetImageFilePath(model.ThirdImageFile);
                }
                else
                {
                    thirdImage.Link = model.ThirdImageLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
                }
                thirdImage.Description = model.ThirdImageDescription;

                _mainRepository.UpdateImage(firstImage);
                _mainRepository.UpdateImage(secondImage);
                _mainRepository.UpdateImage(thirdImage);
                return RedirectToAction("Page");
            }
            catch
            {
                return RedirectToAction("Page");
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
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "file");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ZipFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.ZipFile.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                int newID = Product.MakeNewID(_mainRepository);

                bool attached = AttachTags(newID);

                Product newProduct = new Product
                {
                    ID = newID,
                    Name = model.Name,
                    Type = Product.CheckTypeString(model.Type),
                    Price = model.Price,
                    Discount = model.Discount,
                    Description = model.Description,
                    Link = model.Link,
                    FileName = uniqueFileName,
                    AddedDate = DateTime.Today,
                    Version = model.Version,
                    OwnerID = _userManager.GetUserId(User)
                };

                _mainRepository.AddProduct(newProduct);

                string firstImageFileName = GetImageFilePath(model.FirstImageFile);
                string secondImageFileName = GetImageFilePath(model.SecondImageFile);
                string thirdImageFileName = GetImageFilePath(model.ThirdImageFile);

                AttachImages(newID,
                    firstImageFileName != null ? firstImageFileName : model.FirstImageLink, model.FirstImageDescription,
                    secondImageFileName != null ? secondImageFileName : model.SecondImageLink, model.SecondImageDescription,
                    thirdImageFileName != null ? thirdImageFileName : model.ThirdImageLink, model.ThirdImageDescription);

                _tags = null;
                _addedProduct = !attached ? newProduct : null;

                return RedirectToAction("Catalog", "Catalog");
            }
            return View();
        }

        private string GetImageFilePath(IFormFile formFile)
        {
            string outFileName = null;
            string imageUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "imguploads");
            if (formFile != null)
            {
                outFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                string filePath = Path.Combine(imageUploadsFolder, outFileName);
                formFile.CopyTo(new FileStream(filePath, FileMode.Create));
                outFileName = "/imguploads/" + outFileName;
            }
            return outFileName;
        }

        [HttpPost]
        public IActionResult AddTags(string[] tags)
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
                    _mainRepository.AddTag(new Tag
                    {
                        ProductID = productID.ToString(),
                        TypeId = _mainRepository.GetProductTypeByName(tag).ID
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
            _mainRepository.AddImage(new Image
            {
                ProductID = id,
                Link = (firstLink != null && firstLink.Length > 0) ? firstLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                Description = firstDesc,
                OrderIndex = 0
            });
            _mainRepository.AddImage(new Image
            {
                ProductID = id,
                Link = secondLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                Description = secondDesc,
                OrderIndex = 1
            });
            _mainRepository.AddImage(new Image
            {
                ProductID = id,
                Link = thirdLink ?? "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg",
                Description = thirdDesc,
                OrderIndex = 2
            });
        }

        public IActionResult OpenEditProduct(int prodId)
        {
            var product = _mainRepository.GetProduct(prodId);
            return View("Edit", product);
        }

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
                _mainRepository.UpdateProduct(product);
                return View("Page", product);
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return View("Page", product);
            }
        }

        public IActionResult EditTags(int prodId)
        {
            var tagNames = _mainRepository.GetTagNamesByProductId(prodId).ToList();

            List<string> listOfProductTypes = (from pt in _mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name).ToList();

            return View("EditTagsView", new EditTagsViewModel
            { 
                ProductId = prodId,
                Tags = tagNames,
                ListOfProductTypes = listOfProductTypes
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTags([FromForm]EditTagsViewModel model)
        {
            // waiting the UpdateTags result to properly show the tags in the EditTagsView
            await Task.Delay(10);

            var tagNames = _mainRepository.GetTagNamesByProductId(model.ProductId).ToList();

            List<string> listOfProductTypes = (from pt in _mainRepository.GetAllProductTypes() orderby pt.Name select pt.Name).ToList();

            return View("EditTagsView", new EditTagsViewModel
            {
                ProductId = model.ProductId,
                Tags = tagNames,
                ListOfProductTypes = listOfProductTypes,
                EditTagsSuccessful = true
            });
        }

        [HttpPost]
        public IActionResult UpdateTags(int prodId, string[] tags)
        {
            AttachUpdatedTags(prodId, tags);
            return Ok();
        }

        private bool AttachUpdatedTags(int productId, string[] tags)
        {
            if (tags != null)
            {
                var oldTags = _mainRepository.GetTagsByProductID(productId).ToList();
                foreach (var tag in oldTags)
                {
                    _mainRepository.DeleteTag(tag.ID);
                }

                foreach (var tag in tags)
                {
                    _mainRepository.AddTag(new Tag
                    {
                        ProductID = productId.ToString(),
                        TypeId = _mainRepository.GetProductTypeByName(tag).ID
                    });
                }
                return true;
            }
            return false;
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

                var imagesOfProduct = _mainRepository.GetImagesByProductID(id).ToList();

                foreach (var image in imagesOfProduct)
                {
                    _mainRepository.DeleteImage(image.ID);
                }

                var boughtProducts = _mainRepository.GetBoughtProductsByProductId(id).ToList();

                foreach (var bp in boughtProducts)
                {
                    _mainRepository.DeleteBoughtProduct(bp.Id);
                }

                var comments = _mainRepository.GetUserCommentsByProdID(id).ToList();

                foreach (var comment in comments)
                {
                    _mainRepository.DeleteUserComment(comment.ID);
                }

                _mainRepository.DeleteProduct(int.Parse(deleteId));

                return RedirectToAction("Catalog", "Catalog");
            }
            catch
            {
                return RedirectToAction("Catalog", "Catalog");
            }
        }

        private void CleanTags(int id)
        {
            foreach (var i in _mainRepository.GetTagsByProductID(id).ToList())
            {
                _mainRepository.DeleteTag(i.ID);
            }
        }

        [AllowAnonymous]
        public IActionResult Page(Product product, int productId)
        {
            if (product != null && (product.ID == productId || productId == 0))
            {
                return View(product);
            }
            Product model = _mainRepository.GetProduct(productId);
            return View(model);
        }

        public IActionResult EditPage()
        {
            ViewBag.ErrorMessage = "Page editing on the web-page is currently in development.";
            return View("NotFound");
        }

        public IActionResult Buy(int productId)
        {
            var productToBuy = _mainRepository.GetProduct(productId);
            if (productToBuy != null)
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user != null)
                {
                    if (user.Money >= productToBuy.FinalPrice && !productToBuy.IsBought(_mainRepository, user))
                    {
                        try
                        {
                            user.Money -= productToBuy.FinalPrice;
                            _userManager.UpdateAsync(user).Wait();

                            _mainRepository.AddBoughtProduct(new BoughtProduct
                            {
                                AppUserRefId = user.Id,
                                ProductRefId = productToBuy.ID
                            });
                            Console.WriteLine($"{productToBuy.Name} is bought!");
                        }
                        catch (DbUpdateException)
                        {
                            Console.WriteLine("Something went wrong!");
                            throw;
                        }
                    }
                    else
                    {
                        Console.WriteLine("User don't have enough money or product is already bought!");
                    }
                }
            }
            return RedirectToAction("Page", productToBuy);

        }

        public IActionResult Sell(int productId)
        {
            var productToSell = _mainRepository.GetProduct(productId);
            if (productToSell != null)
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user != null && productToSell.IsBought(_mainRepository, user))
                {
                    try
                    {
                        user.Money += productToSell.FinalPrice;
                        _userManager.UpdateAsync(user).Wait();

                        _mainRepository.DeleteBoughtProduct(user.Id, productToSell.ID);
                        Console.WriteLine($"{productToSell.Name} is sold!");
                    }
                    catch (DbUpdateException)
                    {
                        Console.WriteLine("Something went wrong!");
                        throw;
                    }
                }
            }
            return RedirectToAction("Page", productToSell);
        }
    }
}