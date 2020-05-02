using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    [Serializable]
    public class Image
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(256)]
        public string Link { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        [Required]
        public string ProductID { get; set; }
        public int OrderIndex { get; set; }
    }
}
