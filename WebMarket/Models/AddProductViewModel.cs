using Microsoft.AspNetCore.Http;

namespace WebMarket.Models
{
    public class AddProductViewModel : Product
    {
        public string FirstImageLink { get; set; }
        public IFormFile FirstImageFile { get; set; }
        public string FirstImageDescription { get; set; }
        public string SecondImageLink { get; set; }
        public IFormFile SecondImageFile { get; set; }
        public string SecondImageDescription { get; set; }
        public string ThirdImageLink { get; set; }
        public IFormFile ThirdImageFile { get; set; }
        public string ThirdImageDescription { get; set; }
        public IFormFile ZipFile { get; set; }
    }
}
