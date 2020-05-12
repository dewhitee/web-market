using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WebMarket.Data;
using Microsoft.AspNetCore.Http;

namespace WebMarket.Models
{
    [Serializable]
    public class CatalogViewModel
    {
        public enum CatalogViewVariant
        {
            Main,
            Table,
        }
        public enum ProductSort
        {
            None,
            Name,
            Type,
            Price,
            Discount,
            FinalPrice,
        }

        public static List<Product> ListOfProducts = new List<Product>();
        public static List<User> ListOfUsers = new List<User>();
        public static List<Product> AddedToCartProducts = new List<Product>();
        public static Product ChoosenProduct = new Product();
        public static int ChoosenProductID { get; set; }
        public static User CurrentUser = new User();

        public List<Product> Products { get; set; }
        public Product ToAdd { get; set; }

        private static string addedToCartProductsFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\addedtocartproducts_" + CurrentUser.Username + "_.dew"; }
        private static string saveProductsFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\products.dew"; }
        private static string findTagsFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\findtags.dew"; }

        public static List<string> FindTags { get; set; }

        public static CatalogViewVariant ViewVariant { get; set; }

        public static void SaveFindTags()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(findTagsFilePath, FileMode.Open, FileAccess.Write);
                bf.Serialize(stream, FindTags);
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message}, Line: {Utilities.LineNumber()}");
            }
            finally
            {
                stream?.Close();
            }
        }
        public static void LoadFindTags()
        {
            if (!File.Exists(findTagsFilePath))
                File.Create(findTagsFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream(findTagsFilePath, FileMode.Open, FileAccess.Read);
                if (stream.Length != 0)
                    FindTags = (List<string>)bf.Deserialize(stream);
                stream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message}, Line: {Utilities.LineNumber()}");
            }
        }

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

        public static string AutocompleteProduct(string byName)
        {
            foreach (var i in ListOfProducts)
            {
                if (i.Name.Contains(byName))
                    return i.Name;
            }
            return "";
        }

        //public static Product GetProduct(string name)
        //{
        //    foreach (var i in ListOfProducts)
        //    {
        //        if (i.Name == name)
        //            return i;
        //    }
        //    return null;
        //}
        
        //public static Product GetProduct(int id)
        //{
        //    foreach (var i in ListOfProducts)
        //    {
        //        if (i.ID == id)
        //            return i;
        //    }
        //    return null;
        //}

        public static string GetSelectedBuyProductName(IMainRepository repo)
        {
            foreach (var product in ListOfProducts)
            {
                if (product.AddedToCart && !product.IsBought(repo))
                    return product.Name;
            }
            return "";
        }

        public static string GetSelectedSellProductName(IMainRepository repo)
        {
            foreach (var product in ListOfProducts)
            {
                if (product.IsBought(repo) && product.AddedToCart)
                    return product.Name;
            }
            return "";
        }

        public static Product GetSelectedBuyProduct(IMainRepository repo)
        {
            //foreach (var product in ListOfProducts)
            //{
            //    if (product.AddedToCart && !product.IsBought(repo))
            //        return product;
            //}
            //return new Product();
            return ChoosenProduct ?? new Product();
        }
        //public static Product GetSelectedBuyProduct(IMainRepository repository)
        //{
        //    return ChoosenProduct ?? new Product();
        //}

        //public static Product GetSelectedSellProduct(IMainRepository repo)
        //{
        //    foreach (var product in ListOfProducts)
        //    {
        //        if (product.IsBought(repo) && product.AddedToCart)
        //            return product;
        //    }
        //    return new Product();
        //}

        public static Product GetSelectedSellProduct(IMainRepository repository)
        {
            return ChoosenProduct != null && ChoosenProduct.IsBought(repository) ? ChoosenProduct : new Product();
        }

        //public static string GetSelectedSellProductPriceSentence()
        //{
        //    var product = GetSelectedSellProduct();
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //        return CurrentUser.MoneyString + " + " + product.FinalPriceString + string.Format(" = {0}€", (CurrentUser.Money + product.FinalPrice).ToString("0.##"));
        //    else
        //        return "You have not selected any product to sell!";
        //}
        public static string GetSelectedSellProductPriceSentence(IMainRepository repository)
        {
            var product = GetSelectedSellProduct(repository);
            if (!string.IsNullOrWhiteSpace(product.Name))
                return CurrentUser.MoneyString + " + " + product.FinalPriceString + string.Format(" = {0}€", (CurrentUser.Money + product.FinalPrice).ToString("0.##"));
            else
                return "You have not selected any product to sell!";
        }
        //public static string GetSelectedBuyProductPriceSentence()
        //{
        //    var product = GetSelectedBuyProduct();
        //    var finalCost = CurrentUser.Money - product.FinalPrice;
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //        return CurrentUser.MoneyString + " - " + product.FinalPriceString + string.Format(" = {0}€", finalCost.ToString("0.##")) + (finalCost < 0 ? " (You don't have enough money!)" : "");
        //    else
        //        return "You have not selected any product to buy!";
        //}
        public static string GetSelectedBuyProductPriceSentence(IMainRepository repository)
        {
            var product = GetSelectedBuyProduct(repository);
            var finalCost = CurrentUser.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
                return CurrentUser.MoneyString + " - " + product.FinalPriceString + string.Format(" = {0}€", finalCost.ToString("0.##")) + (finalCost < 0 ? " (You don't have enough money!)" : "");
            else
                return "You have not selected any product to buy!";
        }

        //public static string GetBuyProductButtonClassString(bool outline = true)
        //{
        //    var product = GetSelectedBuyProduct();
        //    var finalCost = CurrentUser.Money - product.FinalPrice;
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //    {
        //        if (finalCost < 0)
        //            return "btn btn-danger";
        //    }
        //    return outline ? "btn btn-outline-primary" : "btn btn-primary";
        //}
        public static string GetBuyProductButtonClassString(IMainRepository repository, bool outline = true)
        {
            var product = GetSelectedBuyProduct(repository);
            var finalCost = CurrentUser.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                if (finalCost < 0)
                    return "btn btn-danger";
            }
            return outline ? "btn btn-outline-primary" : "btn btn-primary";
        }
        //public static string GetSubmitBuyingButtonClassString()
        //{
        //    var product = GetSelectedBuyProduct();
        //    var finalCost = CurrentUser.Money - product.FinalPrice;
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //    {
        //        if (finalCost < 0)
        //            return "btn btn-danger disabled";
        //    }
        //    return "btn btn-success";
        //}
        public static string GetSubmitBuyingButtonClassString(IMainRepository repository)
        {
            var product = GetSelectedBuyProduct(repository);
            var finalCost = CurrentUser.Money - product.FinalPrice;
            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                if (finalCost < 0)
                    return "btn btn-danger disabled";
            }
            return "btn btn-success";
        }

        //public static string GetSubmitBuyingButtonText()
        //{
        //    var product = GetSelectedBuyProduct();
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //        return "Submit buying product";

        //    return "Find";
        //}
        public static string GetSubmitBuyingButtonText(IMainRepository repository)
        {
            var product = GetSelectedBuyProduct(repository);
            if (!string.IsNullOrWhiteSpace(product.Name))
                return "Submit buying product";

            return "Find";
        }
        //public static string GetSubmitSellingButtonText()
        //{
        //    var product = GetSelectedSellProduct();
        //    if (!string.IsNullOrWhiteSpace(product.Name))
        //        return "Submit selling product";

        //    return "Find";
        //}
        public static string GetSubmitSellingButtonText(IMainRepository repository)
        {
            var product = GetSelectedSellProduct(repository);
            if (!string.IsNullOrWhiteSpace(product.Name))
                return "Submit selling product";

            return "Find";
        }

        public static void LoadProducts()
        {
            if (!File.Exists(saveProductsFilePath))
                File.Create(saveProductsFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream(saveProductsFilePath, FileMode.Open, FileAccess.Read);
                if (stream.Length != 0)
                    ListOfProducts = (List<Product>)bf.Deserialize(stream);
                stream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message}, Line: {Utilities.LineNumber()}");
            }

            if (!File.Exists(addedToCartProductsFilePath))
                File.Create(addedToCartProductsFilePath);

            BinaryFormatter addedToCartFormatter = new BinaryFormatter();
            try
            {
                Stream addedToCartStream = new FileStream(addedToCartProductsFilePath, FileMode.Open, FileAccess.Read);
                if (addedToCartStream.Length != 0)
                    AddedToCartProducts = (List<Product>)addedToCartFormatter?.Deserialize(addedToCartStream);

                addedToCartStream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message}, Line: {Utilities.LineNumber()}");
            }
        }
        public static void SaveProducts()
        {
            //if (!File.Exists(saveProductsFilePath))
            //    File.Create(saveProductsFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(saveProductsFilePath, FileMode.Open, FileAccess.Write);
                bf.Serialize(stream, ListOfProducts);
                //stream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message}, Line: {Utilities.LineNumber()}");
            }
            finally
            {
                stream?.Close();
            }

            //if (!File.Exists(addedToCartProductsFilePath))
            //    File.Create(addedToCartProductsFilePath);
            BinaryFormatter addedToCartFormatter = new BinaryFormatter();
            Stream addedToCartStream = null;
            try
            {
                addedToCartStream = new FileStream(addedToCartProductsFilePath, FileMode.Open, FileAccess.Write);
                addedToCartFormatter.Serialize(addedToCartStream, AddedToCartProducts);
                //addedToCartStream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message}, Line: {Utilities.LineNumber()}");
            }
            finally
            {
                addedToCartStream?.Close();
            }
        }
    }
}
