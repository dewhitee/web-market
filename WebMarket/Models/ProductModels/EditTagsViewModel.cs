using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class EditTagsViewModel
    {
        public int ProductId { get; set; }
        // IEnumerable<string> Tags { get; set; }

        //! there will be max 8 tags approx. available to the product in the end
        public string FirstTag { get; set; }
        public string SecondTag { get; set; }
        public string ThirdTag { get; set; }
        public string FourthTag { get; set; }
        public string FifthTag { get; set; }
        public string SixthTag { get; set; }
        public string SeventhTag { get; set; }
        public string EighthTag { get; set; }
    }
}
