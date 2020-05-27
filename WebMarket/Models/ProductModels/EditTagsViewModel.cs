using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class EditTagsViewModel
    {
        public int ProductId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> ListOfProductTypes { get; set; }
        public bool EditTagsSuccessful { get; set; }
    }
}
