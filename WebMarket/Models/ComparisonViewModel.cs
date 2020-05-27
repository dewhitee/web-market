using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    public class ComparisonViewModel
    {
        public List<Product> Products { get; set; }
        public Product LeftProduct { get; set; }
        public Product RightProduct { get; set; }
        [BindProperty(SupportsGet = true)]
        public string RightSearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LeftSearchTerm { get; set; }

        public bool RightProductNotFound { get; set; }
        public string RightProductName { get; set; }
        public bool LeftProductNotFound { get; set; }
        public string LeftProductName { get; set; }

        public int RightProductTagsCount { get; set; }
        public int LeftProductTagsCount { get; set; }

        public int RightProductBoughtTimes { get; set; }
        public int LeftProductBoughtTimes { get; set; }

        public long LeftProductFileSize { get; set; }
        public long RightProductFileSize { get; set; }

        public string PriceComparisonText()
        {
            return TextHelper(LeftProduct.Price, RightProduct.Price,
                "is more expensive than",
                "is cheaper than",
                "are equal in terms of price", true);
        }
        public string DiscountComparisonText()
        {
            return TextHelper(LeftProduct.Discount, RightProduct.Discount,
                "has larger discount than",
                "has smaller discount than",
                "has same discount as");
        }
        public string AddedDateComparisonText()
        {
            return TextHelper(LeftProduct.AddedDate, RightProduct.AddedDate,
                "was added later than",
                "was added earlier than",
                "was added at the same day with");
        }
        public string TagsText()
        {
            return TextHelper(LeftProductTagsCount, RightProductTagsCount,
                "has more tags than",
                "has less tags than",
                "has same amount of tags as");
        }
        public string PopularityText()
        {
            return TextHelper(LeftProductBoughtTimes, RightProductBoughtTimes,
                "is more popular than",
                "is less popular then",
                "are equally popular",
                true);
        }
        public string FileSizeText()
        {
            if (LeftProductFileSize <= 0 && RightProductFileSize > 0)
            {
                return $"{LeftProduct.Name} don't have files in our database, but {RightProduct.Name} has.";
            }
            else if (LeftProductFileSize > 0 && RightProductFileSize <= 0)
            {
                return $"{LeftProduct.Name} has files in our database, while {RightProduct.Name} don't.";
            }
            else if (LeftProductFileSize <= 0 && RightProductFileSize <= 0)
            {
                return $"Nor {LeftProduct.Name}, nor {RightProduct.Name} don't have any files in our database.";
            }
            else
            {
                return TextHelper(LeftProductFileSize, RightProductFileSize,
                    "file size is larger than of",
                    "file size is smaller than of",
                    "file sizes are equal", true);
            }
        }

        private string TextHelper<T>(T left, T right, string leftMoreText, string rightMoreText, string equalText, bool and = false)
        {
            return (Comparer<T>.Default.Compare(left, right) > 0) ? $"{LeftProduct.Name} {leftMoreText} {RightProduct.Name}"
                : Comparer<T>.Default.Compare(left, right) < 0 ? $"{LeftProduct.Name} {rightMoreText} {RightProduct.Name}"
                : (and ? $"{LeftProduct.Name} and {RightProduct.Name} {equalText}" : $"{LeftProduct.Name} {equalText} {RightProduct.Name}");
        }

        public static float GetStarsValue(Product product, IMainRepository repository)
        {
            return product.GetRate(repository);
        }
        public static float GetStarsValue(IEnumerable<UserComment> userComments)
        {
            return Product.GetRate(userComments);
        }
    }
}
