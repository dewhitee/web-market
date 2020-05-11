using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class ProductType
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Type name")]
        public string Name { get; set; }
    }
}
