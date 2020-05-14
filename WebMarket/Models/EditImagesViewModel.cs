using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class EditImagesViewModel
    {
        public int ProductId { get; set; }
        public string FirstImageLink { get; set; }
        public string FirstImageDescription { get; set; }
        //
        public string SecondImageLink { get; set; }
        public string SecondImageDescription { get; set; }
        //
        public string ThirdImageLink { get; set; }
        public string ThirdImageDescription { get; set; }
        // 
        public string FourthImageLink { get; set; }
        public string FourthImageDescription { get; set; }
        //
        public string FifthImageLink { get; set; }
        public string FifthImageDescription { get; set; }
        //
        public string SixthImageLink { get; set; }
        public string SixthImageDescription { get; set; }
    }
}
