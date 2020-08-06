using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models.ProductModels
{
    public class EditDownloadsViewModel
    {
        public int ProductId { get; set; }
        public IFormFile ZipFile { get; set; }
        public bool Successful { get; set; }
    }
}
