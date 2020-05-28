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
        public enum SortOption
        {
            None,
            BoughtDate
        }

        [Key]
        public int Id { get; set; }

        public string AppUserRefId { get; set; }
        //public AppUser User { get; set; }

        public int ProductRefId { get; set; }
        //public Product Product { get; set; }

        [Display(Name = "Bought Date")]
        [DataType(DataType.Date)]
        public DateTime BoughtDate { get; set; }
    }
}
