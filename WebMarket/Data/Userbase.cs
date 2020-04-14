using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebMarket.Data
{
    public class Userbase
    {
        public static void Set(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ClaimsPrincipal user)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            User = user;
            Console.WriteLine($"SignInManager = {SignInManager != null}, " +
                $"UserManager = {UserManager != null}, User = {User != null}");
        }
        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static ClaimsPrincipal User { get; set; }
    }
}
