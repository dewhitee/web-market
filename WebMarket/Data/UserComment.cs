using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Data
{
    [Serializable]
    public class UserComment
    {
        public string Text { get; set; }
        public string UserID { get; set; }
        public byte Rate { get; set; }
    }
}
