using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class AddProductViewModel : Product
    {
        public string FirstImageLink { get; set; }
        public string FirstImageDescription { get; set; }
        public string SecondImageLink { get; set; }
        public string SecondImageDescription { get; set; }
        public string ThirdImageLink { get; set; }
        public string ThirdImageDescription { get; set; }
        public IFormFile ZipFile { get; set; }
    }
}
