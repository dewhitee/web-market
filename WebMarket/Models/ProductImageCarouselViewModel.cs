using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class ProductImageCarouselViewModel : Product
    {
        
        // catalog-comp for Comparison page
        public string carouselImageClass = "catalog-buy";
        public string carouselImageId = "img-right";
        public int carouselIndex = 0;

        public ProductImageCarouselViewModel(
            Product product,
            string carouselImageClass,
            string carouselImageId = "img-right",
            int carouselIndex = 0)
        {
            ID = product.ID;
            Name = product.Name;
            this.carouselImageClass = carouselImageClass;
            this.carouselImageId = carouselImageId;
            this.carouselIndex = carouselIndex;
        }
    }
}
