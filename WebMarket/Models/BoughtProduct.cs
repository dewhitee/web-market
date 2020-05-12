using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebMarket.Data;

namespace WebMarket.Models
{
    public class BoughtProduct
    {
        [Key]
        public int Id { get; set; }

        public string AppUserRefId { get; set; }
        //public AppUser User { get; set; }

        public int ProductRefId { get; set; }
        //public Product Product { get; set; }
    }
}
