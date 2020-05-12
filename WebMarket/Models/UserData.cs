using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class UserData
    {
        [Key]
        public int ID { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }
    }
}
