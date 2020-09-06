using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    [Serializable]
    public class Product : IComparable<Product>
    {
        public enum SortOption
        {
            None,
            Name,
            Type,
            Price,
            Discount,
            FinalPrice,
            AddedDate
        }

        public int ID { get; set; }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [Required, StringLength(32)]
        public string Type { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [NotMapped]
        public int CostIntegral => (int)Math.Truncate(Price);

        [NotMapped]
        public decimal CostFractional => Price - CostIntegral;
        public float Discount { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        [StringLength(128)]
        public string Link { get; set; }

        [DisplayName("Only registered user can comment")]
        public bool OnlyRegisteredCanComment { get; set; }

        [DisplayName("Only one comment per user")]
        public bool OnlyOneCommentPerUser { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Obsolete]
        public List<UserComment> Comments = new List<UserComment>();

        [Display(Name = "Added Date")]
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Current Version")]
        public string Version { get; set; }

        public string OwnerID { get; set; }

        public decimal FinalPrice       => Price - (Price * (decimal)Discount * 0.01M);
        public string FinalPriceString  => FinalPrice > 0 ? FinalPrice.ToString("0.##") + "€" : "free";
        public string PriceString       => Price > 0 ? Price.ToString() + "€" : "free";
        public string DiscountString    => Discount > 0 ? Discount.ToString() + "%" : "no";
        public string DiscountSupString => Discount > 0 ? Discount.ToString() + "%" : "";
        public string LinkTableString   => string.IsNullOrWhiteSpace(Link) ? "no link" : "yes";

        public bool HasValidId => ID < 0;

        // Property for disabling buying/selling actions on product page
        [NotMapped]
        public bool NonTradableMode { get; set; }

        public string IsBoughtString(IMainRepository repo, AppUser user)
        {
            return IsBought(repo, user) ? "Bought" : "+";
        }

        public bool IsBought(IMainRepository repository, AppUser byUser)
        {
            var boughtProductIds = repository.GetBoughtProductsByUserId(byUser?.Id);
            if (boughtProductIds.Any())
            {
                return (from bp in boughtProductIds where bp.ProductRefId == ID select bp.ProductRefId).Contains(ID);
            }
            return false;
        }

        public bool ContainsTags(List<string> findTags, IMainRepository repository, bool fullyMatching)
        {
            var repoTags = repository.GetTagNamesByProductId(ID);
            if (findTags != null && repoTags != null)
            {
                if (fullyMatching)
                {
                    foreach (var tag in findTags)
                    {
                        if (!repoTags.Contains(tag))
                            return false;
                    }
                    return true;
                }
                else
                {
                    foreach (var tag in findTags)
                    {
                        if (repoTags.Contains(tag))
                            return true;
                    }
                    return false;
                }
            }
            return false;
        }
        public static bool ContainsTags(List<string> findTags, IEnumerable<string> repoTags, bool fullyMatching)
        {
            if (findTags != null && repoTags != null)
            {
                if (fullyMatching)
                {
                    foreach (var tag in findTags)
                    {
                        if (!repoTags.Contains(tag))
                            return false;
                    }
                    return true;
                }
                else
                {
                    foreach (var tag in findTags)
                    {
                        if (repoTags.Contains(tag))
                            return true;
                    }
                    return false;
                }
            }
            return false;
        }
        public float GetRateAvg(IMainRepository repository)
        {
            return GetRateSum(repository) / repository.GetUserCommentsByProdID(ID).Count();
        }
        public float GetRate(IMainRepository repository)
        {
            float sum = GetRateSum(repository);
            return sum / GetTotalCountOfNotNulledComments(repository);
        }
        public static float GetRate(IEnumerable<UserComment> userComments)
        {
            float sum = GetRateSum(userComments);
            return sum / GetTotalCountOfNotNulledComments(userComments);
        }
        public float GetRateSum(IMainRepository repository)
        {
            float sum = 0;
            foreach (var i in repository.GetUserCommentsByProdID(ID))
            {
                sum += i.Rate;
            }
            return sum;
        }

        // might be used instead of get rate sum later
        public static float GetRateSum(IEnumerable<UserComment> userComments)
        {
            float sum = 0;
            foreach (var i in userComments)
            {
                sum += i.Rate;
            }
            return sum;
        }
        public int GetStarsCount(int stars, IMainRepository repository)
        {
            if (stars > 5) return -1;
            int count = 0;
            foreach (var i in repository.GetUserCommentsByProdID(ID))
            {
                if (Math.Truncate((decimal)i.Rate) == stars)
                    count++;
            }
            return count;
        }
        public static int GetStarsCount(int stars, IEnumerable<UserComment> userComments)
        {
            if (stars > 5 || stars <= 0) return -1;
            int count = 0;
            foreach (var i in userComments)
            {
                if (Math.Truncate((decimal)i.Rate) == stars)
                    count++;
            }
            return count;
        }
        public uint GetTotalCountOfNotNulledComments(IMainRepository repository)
        {
            uint count = 0;
            foreach (var i in repository.GetUserCommentsByProdID(ID))
            {
                if (i.Rate > 0f)
                    count++;
            }
            return count;
        }
        public static uint GetTotalCountOfNotNulledComments(IEnumerable<UserComment> userComments)
        {
            uint count = 0;
            foreach (var i in userComments)
            {
                if (i.Rate > 0f)
                    count++;
            }
            return count;
        }
        public float GetStarsPercent(int stars, IMainRepository repository)
        {
            Console.WriteLine($"Getting stars percent for {stars} stars...");
            if (stars > 5) return 0f;
            float starsCount = GetStarsCount(stars, repository);
            float totalComments = GetTotalCountOfNotNulledComments(repository);
            Console.WriteLine($"Final stars count is {starsCount}. This is {(starsCount <= 0 ? starsCount : starsCount / totalComments)} percent from the total amount of stars.");
            return starsCount <= 0 ? 0f : starsCount / totalComments;
        }
        public uint GetTotalStarsCount(IMainRepository repository)
        {
            uint count = 0;
            foreach (var i in repository.GetUserCommentsByProdID(ID))
            {
                count += i.Stars;
            }
            return count;
        }

        public static int MakeNewID(IMainRepository repository)
        {
            int newID = repository.GetAllProducts().Count() + 1;
            while (repository.GetAllProducts().Any(p => p.ID == newID))
            {
                newID++;
            }
            return newID;
        }

        public long GetFileSize(IWebHostEnvironment hostingEnvironment)
        {
            if (FileName?.Length > 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "file");
                string filePath = Path.Combine(uploadsFolder, FileName);
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            return 0;
        }
        public string FormatFileSize(IWebHostEnvironment hostingEnvironment)
        {
            var fileSize = GetFileSize(hostingEnvironment);
            if (fileSize < 0) return "";
            if (fileSize >= 1000000000) return new string(Math.Round(fileSize / 1000000000f, 4).ToString() + " Gb");
            if (fileSize >= 1000000) return new string(Math.Round(fileSize / 1000000f, 4).ToString() + " Mb");
            if (fileSize >= 1000) return new string(Math.Round(fileSize / 1000f, 4).ToString() + " Kb");
            if (fileSize == 0) return "Empty file";
            return new string(fileSize.ToString() + " bytes");
        }
        public static string FormatFileSize(long fileSize)
        {
            if (fileSize < 0) return "";
            if (fileSize >= 1000000000) return new string(Math.Round(fileSize / 1000000000f, 4).ToString() + " Gb");
            if (fileSize >= 1000000) return new string(Math.Round(fileSize / 1000000f, 4).ToString() + " Mb");
            if (fileSize >= 1000) return new string(Math.Round(fileSize / 1000f, 4).ToString() + " Kb");
            if (fileSize == 0) return "Empty file";
            return new string(fileSize.ToString() + " bytes");
        }


        public string GetAddToCartButtonString(IMainRepository repo, AppUser user)
        {
            if (OwnerID == user?.Id)
                return "Yours";
            else if (IsBought(repo, user))
                return "Bought";
            else if (CatalogViewModel.ChoosenProductID == this.ID)
                return "Selected";
            else return "+";
        }

        public string GetImageSrc(IMainRepository repository, int? index)
        {
            Image image;
            if (index == null)
                image = repository.GetImagesByProductID(ID).FirstOrDefault();
            else
                image = repository.GetImageByOrderIndex(ID, index.Value);

            if (image != null)
            {
                return image.Link.Length > 0 ? image.Link : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
        }

        public static string CheckTypeString(string type)
        {
            if (type == "Choose type")
                return "No type specified";
            else return type;
        }

        public string GetPriceTableClassString(IMainRepository repo, AppUser user)
        {
            if (CatalogViewModel.ChoosenProductID == this.ID || OwnerID == user?.Id)
                return "bg-dark text-white";
            if (IsBought(repo, user))
                return "bg-primary text-dark";
            if (Price == 0 || FinalPrice == 0)
                return "bg-success text-dark";
            if (Price < 10 || FinalPrice < 10 || Discount > 50)
                return "bg-success text-dark";
            if (Price > 250 || FinalPrice > 250)
                return "bg-danger text-dark";
            return "";
        }

        public string GetProductTableLinkClassString(IMainRepository repo, AppUser user)
        {
            if (IsBought(repo, user) || CatalogViewModel.ChoosenProductID == this.ID || OwnerID == user?.Id)
                return "text-white";
            else
                return "text-dark";
        }

        public string GetAddToCartButtonClassString(IMainRepository repo, AppUser user)
        {
            if (CatalogViewModel.ChoosenProductID == this.ID || OwnerID == user?.Id)
            {
                return "btn btn-outline-light";
            }
            else if (user?.Money < FinalPrice)
                return "btn btn-outline-danger";
            else if (!IsBought(repo, user))
                return "btn btn-outline-success";
            else return "btn btn-primary";
        }

        public string GetTableHeaderClassString(IMainRepository repo, AppUser user)
        {
            if (CatalogViewModel.ChoosenProductID == this.ID || OwnerID == user?.Id)
            {
                return "bg-dark text-white";
            }
            else if (IsBought(repo, user))
            {
                return "bg-primary text-white";
            }
            else return "";
        }

        public string GetBuyButtonClassString(AppUser user, bool outline = true)
        {
            var finalCost = user?.Money - FinalPrice;
            if (!string.IsNullOrWhiteSpace(Name))
            {
                if (finalCost < 0)
                    return "btn btn-danger";
            }
            return outline ? "btn btn-outline-primary" : "btn btn-primary";
        }

        public string GetBuyPriceSentence(AppUser user)
        {
            var finalCost = user?.Money - FinalPrice;
            if (!string.IsNullOrWhiteSpace(Name) && user != null)
                return user.MoneyString + " - " + FinalPriceString + string.Format(" = {0}€", finalCost?.ToString("0.##")) + (finalCost < 0 ? " (You don't have enough money!)" : "");
            else
                return "You have not selected any product to buy!";
        }

        public string GetSellPriceSentence(AppUser user)
        {
            if (!string.IsNullOrWhiteSpace(Name) && user != null)
                return user.MoneyString + " + " + FinalPriceString + string.Format(" = {0}€", (user.Money + FinalPrice).ToString("0.##"));
            else
                return "You have not selected any product to sell!";
        }

        public static int CompareByName(Product x, Product y)
        {
            return x.Name.CompareTo(y.Name);
        }
        public static int CompareByType(Product x, Product y)
        {
            return x.Type.CompareTo(y.Type);
        }
        public static int CompareByPrice(Product x, Product y)
        {
            return x.Price.CompareTo(y.Price);
        }

        // note that this comparison is reversed as we need descending sort
        public static int CompareByDiscount(Product x, Product y)
        {
            return y.Discount.CompareTo(x.Discount);
        }
        public static int CompareByFinalPrice(Product x, Product y)
        {
            return x.FinalPrice.CompareTo(y.FinalPrice);
        }

        public static int CompareByDate(Product x, Product y)
        {
            return x.AddedDate.CompareTo(y.AddedDate);
        }
        public int CompareTo([AllowNull] Product other)
        {
            if (this.ID < other.ID)
                return 1;
            else if (this.ID > other.ID)
                return -1;
            else
                return 0;
        }
    }
}
