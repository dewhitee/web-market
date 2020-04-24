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
        [Serializable]
        public struct UserNameIDBinding
        {
            public string id;
            public string name;
        }

        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static ClaimsPrincipal User { get; set; }

        public static List<string> Usernames { get; private set; }
        public static List<UserNameIDBinding> UserNameIDs { get; private set; }

        public static bool IsInitialized = false;

        public static string MoneyFilePath { get => userMoneyPartialPath + (User/*.Identity.Name*/ != null ? User.Identity.Name : "") + "_money.dew"; }

        private static readonly string usernamesFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\allusernames.dew";
        private static readonly string usernameidsFilePath = @"D:\ASP.NET PROJECTS\WebMarket\data\usernameids.dew";
        private static readonly string userMoneyPartialPath = @"D:\ASP.NET PROJECTS\WebMarket\data\user_";
        private static string saveUserFilePath { get => @"D:\ASP.NET PROJECTS\WebMarket\data\user_" + CatalogViewModel.CurrentUser.Username + "_.dew"; }


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
        public static async void LoadData()
        {
            LoadUsernames();
            LoadUserNameIDs();
            await Task.Delay(1);
        }
        public static async void InitCurrentUser()
        {
            if (User != null)
            {
                var id = UserManager.GetUserId(User);
                CatalogViewModel.CurrentUser = new User
                {
                    Username = User.Identity.Name,
                    ID = id,
                    Money = GetMoney()
                };
                //LoadUser();
                AddUserNameIDBinding(User.Identity.Name, id);
                IsInitialized = true;
            }
            //CatalogViewModel.CurrentUser.AddInitMoney();
            //SaveMoney();
            await Task.Delay(1);
        }
        public static void LoadUserNameIDs()
        {
            if (!File.Exists(usernameidsFilePath))
                File.Create(usernameidsFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(usernameidsFilePath, FileMode.Open, FileAccess.Read);
            List<UserNameIDBinding> usernameids = new List<UserNameIDBinding>();
            if (stream.Length != 0)
                usernameids = (List<UserNameIDBinding>)bf.Deserialize(stream);

            stream.Close();
            UserNameIDs = usernameids;
            //await Task.Delay(10);
        }
        public static void SaveUserNameIDs()
        {
            if (!File.Exists(usernameidsFilePath))
                File.Create(usernameidsFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            //! the process cannot access file exception rises here!
            try
            {
                Stream stream = new FileStream(usernameidsFilePath, FileMode.Open, FileAccess.Write);

                bf.Serialize(stream, UserNameIDs);
                stream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static string GetUsername(string id)
        {
            return UserNameIDs.Find(x => x.id == id).name;
        }
        public static string GetID(string username)
        {
            return UserNameIDs.Find(x => x.name == username).id;
        }
        public static void SaveMoney()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(MoneyFilePath, FileMode.Open, FileAccess.Write);

            bf.Serialize(stream, CatalogViewModel.CurrentUser.Money);
            stream.Close();
        }
        public static void SaveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream(saveUserFilePath, FileMode.Open, FileAccess.Write);

                bf.Serialize(stream, CatalogViewModel.CurrentUser);
                stream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            SaveMoney();
        }
        public static void LoadUser()
        {
            if (!File.Exists(saveUserFilePath))
                File.Create(saveUserFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream(saveUserFilePath, FileMode.Open, FileAccess.Read);
                if (stream.Length != 0)
                    CatalogViewModel.CurrentUser = (User)bf.Deserialize(stream);
                stream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            if (CatalogViewModel.CurrentUser.BoughtProductIDs == null)
                CatalogViewModel.CurrentUser.BoughtProductIDs = new List<string>();
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
        private static void LoadUsernames()
        {
            if (!File.Exists(usernamesFilePath))
                File.Create(usernamesFilePath);

            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(usernamesFilePath, FileMode.Open, FileAccess.Read);
            List<string> usernames = new List<string>();
            if (stream.Length != 0)
                usernames = (List<string>)bf.Deserialize(stream);

            stream.Close();
            Usernames = usernames;
            //await Task.Delay(10);
        }
        private static void AddUserNameIDBinding(string username, string id)
        {
            var newBinding = new UserNameIDBinding() { name = username, id = id };
            if (!UserNameIDs.Contains(newBinding))
            {
                UserNameIDs.Add(newBinding);
            }
            SaveUserNameIDs();
        }

    }
}
