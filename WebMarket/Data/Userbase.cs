using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebMarket.Data
{
    public class Userbase
    {
        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
    }
}
