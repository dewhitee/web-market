using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    [Serializable]
    public class Product
    {
        [Serializable]
        public struct Image
        {
            public string Link;
            public string Description;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int CostIntegral { get; set; }
        public int CostFractional { get; set; }
        public float Discount { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string CardImageLink { get; set; }
        public Image FirstImage { get; set; }
        public Image SecondImage { get; set; }
        public Image ThirdImage { get; set; }
        public string FirstImageLink { get => CardImageLink; }
        public string SecondImageLink { get; set; }
        public string ThirdImageLink { get; set; }
        public bool IsBought { get => CatalogViewModel.CurrentUser.BoughtProductIDs.Contains(ID.ToString());/* set;*/}
        public bool OnlyRegisteredCanComment { get; set; }
        public bool OnlyOneCommentPerUser { get; set; }
        public bool AddedToCart { get; set; }
        public List<string> Tags = new List<string>();
        public List<UserComment> Comments = new List<UserComment>();

        public DateTime AddedDate { get; set; }

        public decimal FinalPrice { get => Price - (Price * (decimal)Discount * 0.01M); }
        public string FinalPriceString { get => FinalPrice > 0 ? FinalPrice.ToString("0.##") + "€" : "free"; }
        public string PriceString { get => Price > 0 ? Price.ToString() + "€" : "free"; }
        public string DiscountString { get => Discount > 0 ? Discount.ToString() + "%" : "no"; }
        public string DiscountSupString { get => Discount > 0 ? Discount.ToString() + "%" : ""; }
        public string LinkTableString { get => string.IsNullOrWhiteSpace(Link) ? "no link" : "yes"; }
        public string IsBoughtString { get => IsBought ? "Bought" : "+"; }
        public string IsAddedToCartString { get => AddedToCart ? "Added" : "+"; }

        public bool HasValidID()
        {
            return ID < 0;
        }


        public float GetRateAvg()
        {
            return GetRateSum() / Comments.Count;
        }
        public float GetRate()
        {
            float sum = GetRateSum();
            float max = Comments.Count * 5f;
            return sum / max;
        }
        public float GetRateSum()
        {
            float sum = 0;
            foreach (var i in Comments)
            {
                sum += i.Rate;
            }
            return sum;
        }
        public int GetStarsCount(int stars)
        {
            if (stars > 5) return -1;
            int count = 0;
            foreach (var i in Comments)
            {
                if (Math.Truncate((decimal)i.Rate) == stars)
                    count++;
            }
            return count;
        }
        public uint GetTotalCountOfNotNulledComments()
        {
            uint count = 0;
            foreach (var i in Comments)
            {
                if (i.Rate != 0f)
                    count++;
            }
            return count;
        }
        public float GetStarsPercent(int stars)
        {
            Console.WriteLine($"Getting stars percent for {stars} stars...");
            if (stars > 5) return 0f;
            float starsCount = GetStarsCount(stars);
            //float totalStarsCount = GetTotalStarsCount();
            float totalComments = GetTotalCountOfNotNulledComments();
            Console.WriteLine($"Final stars count is {starsCount}. This is {(starsCount <= 0 ? starsCount : starsCount / totalComments)} percent from the total amount of stars.");
            return starsCount <= 0 ? 0f : starsCount / totalComments;
        }
        public uint GetTotalStarsCount()
        {
            uint count = 0;
            foreach (var i in Comments)
            {
                count += i.Stars;
            }
            return count;
        }
        
        public static int MakeNewID()
        {
            Random random = new Random();
            int newID;
            bool success;
            do
            {
                newID = random.Next(1, int.MaxValue);
                success = CatalogViewModel.ListOfProducts.Find(x => x.ID == newID) == null;
            } while (!success);
            return newID;
        }

        public string GetAddToCartButtonString()
        {
            if (IsBought)
                return "Bought";
            else if (AddedToCart)
                return "Selected";
            else return "+";
        }

        public string GetCardImageSrc()
        {
            if (CardImageLink != null)
            {
                return CardImageLink.Length > 0 ? CardImageLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            else if (FirstImage.Link.Length > 0)
            {
                return FirstImage.Link;
            }
            else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
        }
        public string GetFirstImageSrc()
        {
            if (FirstImage.Link != null)
            {
                return FirstImage.Link.Length > 0 ? FirstImage.Link : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
        }
        public string GetSecondImageSrc()
        {
            if (SecondImage.Link != null)
            {
                return SecondImage.Link.Length > 0 ? SecondImage.Link : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
        }
        public string GetThirdImageSrc()
        {
            if (ThirdImage.Link != null)
            {
                return ThirdImage.Link.Length > 0 ? ThirdImage.Link : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
        }

        public static string CheckTypeString(string type)
        {
            if (type == "Choose type")
                return "No type specified";
            else return type;
        }

        public string GetPriceTableClassString()
        {
            if (AddedToCart)
                return "bg-dark text-white";
            if (IsBought)
                return "bg-primary text-dark";
            if (Price == 0 || FinalPrice == 0)
                return "bg-success text-dark";
            if (Price < 10 || FinalPrice < 10 || Discount > 50)
                return "bg-success text-dark";
            if (Price > 250 || FinalPrice > 250)
                return "bg-danger text-dark";
            return "";
        }

        public string GetProductTableLinkClassString()
        {
            if (AddedToCart || IsBought)
                return "text-white";
            else
                return "text-dark";
        }

        public string GetAddToCartButtonClassString()
        {
            if (AddedToCart)
            {
                //if (ViewVariant != CatalogViewVariant.Main)
                return "btn btn-outline-light";
                //else return "btn btn-outline-dark";
            }
            else if (CatalogViewModel.CurrentUser.Money < FinalPrice)
                return "btn btn-outline-danger";
            else if (!IsBought)
                return "btn btn-outline-success";
            else return "btn btn-primary";
        }

        public string GetTableHeaderClassString()
        {
            if (AddedToCart)
            {
                //if ()
                return "bg-dark text-white";
            }
            else if (IsBought)
            {
                return "bg-primary text-white";
            }
            else return "";
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
        public static int CompareByDiscount(Product x, Product y)
        {
            return y.Discount.CompareTo(x.Discount);
        }
        public static int CompareByFinalPrice(Product x, Product y)
        {
            return x.FinalPrice.CompareTo(y.FinalPrice);
        }
    }
}
