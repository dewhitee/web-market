using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebMarket.Models
{
    [Serializable]
    public class CatalogViewModel
    {
        private static readonly string addedToCartProductsFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\addedtocartproducts.dew";
        private static readonly string saveProductsFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\products.dew";

        public enum CatalogViewVariant
        {
            Main,
            Table,
        }

        [Serializable]
        public class Product
        {
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
            public string FirstImageLink { get => CardImageLink; }
            public string SecondImageLink { get; set; }
            public string ThirdImageLink { get; set; }
            public bool IsBought { get; set; }
            public bool AddedToCart { get; set; }
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

            public static int MakeNewID()
            {
                Random random = new Random();
                int newID;
                bool success;
                do
                {
                    newID = random.Next(0, int.MaxValue);
                    success = ListOfProducts.Find(x => x.ID == newID) == null;
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
                else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            public string GetFirstImageSrc()
            {
                if (FirstImageLink != null)
                {
                    return FirstImageLink.Length > 0 ? FirstImageLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
                }
                else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            public string GetSecondImageSrc()
            {
                if (SecondImageLink != null)
                {
                    return SecondImageLink.Length > 0 ? SecondImageLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
                }
                else return "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
            }
            public string GetThirdImageSrc()
            {
                if (ThirdImageLink != null)
                {
                    return ThirdImageLink.Length > 0 ? ThirdImageLink : "https://abovethelaw.com/uploads/2019/09/GettyImages-508514140-300x200.jpg";
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
                else if (CurrentUser.Money < FinalPrice)
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

        [Serializable]
        public class User
        {
            public string Username { get; set; }
            public decimal Money { get; set; }
            public string MoneyString { get => Money.ToString("0.##") + "€"; }

            public void BuyProduct(Product product)
            {
                if (Money >= product.FinalPrice && !product.IsBought)
                {
                    // todo: add and save product to profile
                    Money -= product.FinalPrice;
                    product.IsBought = true;
                    Console.WriteLine($"{product.Name} is bought!");
                }
                else
                {
                    Console.WriteLine("User don't have enough money or product is already bought!");
                }
            }
            public void SellProduct(Product product)
            {
                if (product.IsBought)
                {
                    Money += product.FinalPrice;
                    product.IsBought = false;
                    Console.WriteLine($"{product.Name} is sold!");
                }
            }
            public void AddInitMoney()
            {
                Money = 100M;
            }
        }

        [Serializable]
        public class UserComment
        {
            public string Text { get; set; }
            public int UserID { get; set; }
        }

        public static List<Product> ListOfProducts = new List<Product>();
        public static List<Product> AddedToCartProducts = new List<Product>();
        public static User CurrentUser = new User();

        public List<Product> Products { get; set; }
        public Product ToAdd { get; set; }

        public static CatalogViewVariant ViewVariant { get; set; }

        public static bool ContainsID(int ID)
        {
            foreach (var i in ListOfProducts)
            {
                if (i.ID == ID)
                    return true;
            }
            return false;
        }
        public static bool ContainsName(string name)
        {
            foreach (var i in ListOfProducts)
            {
                if (i.Name == name)
                    return true;
            }
            return false;
        }

        public static Product GetProduct(string name)
        {
            foreach (var i in ListOfProducts)
            {
                if (i.Name == name)
                    return i;
            }
            return null;
        }

        public static string GetSelectedBuyProductName()
        {
            foreach (var product in ListOfProducts)
            {
                if (product.AddedToCart && !product.IsBought)
                    return product.Name;
            }
            return "";
        }

        public static string GetSelectedSellProductName()
        {
            foreach (var product in ListOfProducts)
            {
                if (product.IsBought && product.AddedToCart)
                    return product.Name;
            }
            return "";
        }

        public static Product GetSelectedBuyProduct()
        {
            foreach (var product in ListOfProducts)
            {
                if (product.AddedToCart && !product.IsBought)
                    return product;
            }
            return new Product();
        }

        public static Product GetSelectedSellProduct()
        {
            foreach (var product in ListOfProducts)
            {
                if (product.IsBought && product.AddedToCart)
                    return product;
            }
            return new Product();
        }

        public static string GetSelectedSellProductPriceSentence()
        {
            var product = GetSelectedSellProduct();
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                return CurrentUser.MoneyString + " + " + product.FinalPriceString + string.Format(" = {0}€", (CurrentUser.Money + product.FinalPrice).ToString("0.##"));
            }
            else
            {
                return "You have not selected any product to sell!";
            }
        }
        public static string GetSelectedBuyProductPriceSentence()
        {
            var product = GetSelectedBuyProduct();
            var finalCost = CurrentUser.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                return CurrentUser.MoneyString + " - " + product.FinalPriceString + string.Format(" = {0}€", finalCost.ToString("0.##")) + (finalCost < 0 ? " (You don't have enough money!)" : "");
            }
            else
            {
                return "You have not selected any product to buy!";
            }
        }

        public static string GetBuyProductButtonClassString()
        {
            var product = GetSelectedBuyProduct();
            var finalCost = CurrentUser.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                if (finalCost < 0)
                    return "btn btn-danger";
            }
            return "btn btn-outline-primary";
        }
        public static string GetSubmitBuyingButtonClassString()
        {
            var product = GetSelectedBuyProduct();
            var finalCost = CurrentUser.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                if (finalCost < 0)
                {
                    
                    return "btn btn-danger disabled";
                }
            }
            return "btn btn-success";
        }

        public static string GetSubmitBuyingButtonText()
        {
            var product = GetSelectedBuyProduct();
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                return "Submit buying product";
            }
            return "Find";
        }
        public static string GetSubmitSellingButtonText()
        {
            var product = GetSelectedSellProduct();
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                return "Submit selling product";
            }
            return "Find";
        }

        public static void LoadProducts()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveProductsFilePath, FileMode.Open, FileAccess.Read);
            ListOfProducts = (List<Product>)bf.Deserialize(stream);
            stream.Close();

            BinaryFormatter addedToCartFormatter = new BinaryFormatter();
            Stream addedToCartStream = new FileStream(addedToCartProductsFilePath, FileMode.Open, FileAccess.Read);
            if (addedToCartStream.Length != 0)
            {
                AddedToCartProducts = (List<Product>)addedToCartFormatter?.Deserialize(addedToCartStream);
            }
            addedToCartStream.Close();
        }
        public static void SaveProducts()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(saveProductsFilePath, FileMode.Open, FileAccess.Write);

            bf.Serialize(stream, ListOfProducts);
            stream.Close();

            BinaryFormatter addedToCartFormatter = new BinaryFormatter();
            Stream addedToCartStream = new FileStream(addedToCartProductsFilePath, FileMode.Open, FileAccess.Write);
            addedToCartFormatter.Serialize(addedToCartStream, AddedToCartProducts);
            addedToCartStream.Close();
        }
        //public string GetSellProductButtonClassString()
        //{
        //    var product = GetSelectedSellProduct();
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //    {
        //        return CurrentUser.MoneyString + " + " + product.FinalPriceString + string.Format(" = {0}€", (CurrentUser.Money + product.FinalPrice).ToString("0.##"));
        //    }
        //    else
        //    {
        //        return "You have not selected any product to sell!";
        //    }
        //}
    }
}
