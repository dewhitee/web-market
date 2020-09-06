using System.ComponentModel.DataAnnotations;

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
