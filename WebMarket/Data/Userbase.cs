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
        public struct UserNameIDBinding
        {
            string id;
            string name;
        }

        private static readonly string usernamesFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\allusernames.dew";
        private static readonly string usernameidsFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\usernameids.dew";
        private static readonly string userMoneyPartialPath = @"D:\ASP.NET PROJECTS\WebMarket\data\user_";

        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static ClaimsPrincipal User { get; set; }

        private static List<string> Usernames;
        private static List<UserNameIDBinding> userIDNameBindings;

        public static string MoneyFilePath { get => userMoneyPartialPath + (User.Identity.Name != null ? User.Identity.Name : "") + "_money.dew"; }


        public static void Set(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ClaimsPrincipal user)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            User = user;
            Console.WriteLine($"SignInManager = {SignInManager != null}, " +
                $"UserManager = {UserManager != null}, User = {User != null}");
            if (!File.Exists(MoneyFilePath))
                File.Create(MoneyFilePath);
            InitCurrentUser();
        }
        public static async void InitCurrentUser()
        {
            CatalogViewModel.CurrentUser = new CatalogViewModel.User
            {
                Username = User.Identity.Name,
                ID = UserManager.GetUserId(User),
                Money = GetMoney()
            };
            //CatalogViewModel.CurrentUser.AddInitMoney();
            //SaveMoney();
            await Task.Delay(10);
        }
        private static decimal GetMoney()
        {
            if (!File.Exists(MoneyFilePath))
                File.Create(MoneyFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(MoneyFilePath, FileMode.Open, FileAccess.Read);
            decimal moneyValue = 0.0M;
            if (stream.Length != 0)
                moneyValue = (decimal)bf.Deserialize(stream);

            stream.Close();
            return moneyValue;
        }
        private static List<string> GetUsernames()
        {
            if (!File.Exists(MoneyFilePath))
                File.Create(MoneyFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(usernamesFilePath, FileMode.Open, FileAccess.Read);
            List<string> usernames = new List<string>();
            if (stream.Length != 0)
                usernames = (List<string>)bf.Deserialize(stream);

            stream.Close();
            return usernames;
        }
        public static List<UserNameIDBinding> GetUserNameIDs()
        {
            if (!File.Exists(MoneyFilePath))
                File.Create(MoneyFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(usernameidsFilePath, FileMode.Open, FileAccess.Read);
            List<UserNameIDBinding> usernameids = new List<UserNameIDBinding>();
            if (stream.Length != 0)
                usernameids = (List<UserNameIDBinding>)bf.Deserialize(stream);

            stream.Close();
            return usernameids;
        }
        public static void SaveMoney()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(MoneyFilePath, FileMode.Open, FileAccess.Write);

            bf.Serialize(stream, CatalogViewModel.CurrentUser.Money);
            stream.Close();
        }
    }
}
