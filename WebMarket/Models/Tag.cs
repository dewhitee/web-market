using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarket.Models
{
    [Serializable]
    public class Tag
    {
        [Key]
        public int ID { get; set; }
        
        public int TypeId { get; set; }

        [Required, MaxLength(32)]
        public string ProductID { get; set; }
    }
}
