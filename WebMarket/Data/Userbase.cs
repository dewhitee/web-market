using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebMarket.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebMarket.Data
{
    public class Userbase
    {
        private static readonly string userMoneyPartialPath = @"D:\ASP.NET PROJECTS\WebMarket\data\usermoney_";

        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static ClaimsPrincipal User { get; set; }

        public static string GetMoneyPath { get => userMoneyPartialPath + (User.Identity.Name != null ? User.Identity.Name : "") + ".dew"; }


        public static void Set(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ClaimsPrincipal user)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            User = user;
            Console.WriteLine($"SignInManager = {SignInManager != null}, " +
                $"UserManager = {UserManager != null}, User = {User != null}");
            InitCurrentUser();
        }
        public static void InitCurrentUser()
        {
            CatalogViewModel.CurrentUser = new CatalogViewModel.User
            {
                Username = User.Identity.Name,
                ID = UserManager.GetUserId(User),
                Money = GetMoney()
            };
        }
        private static void InitMoneyPath(string username)
        {

        }
        private static decimal GetMoney()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(GetMoneyPath, FileMode.Open, FileAccess.Read);
            var moneyValue = (decimal)bf.Deserialize(stream);
            stream.Close();
            return moneyValue;
        }
    }
}
