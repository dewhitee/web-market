using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebMarket.Data
{
    public class AppUser : IdentityUser
    {
        public AppUser(string name)
        {
            Id = Guid.NewGuid().ToString();
            UserName = name;
        }
    }
}
