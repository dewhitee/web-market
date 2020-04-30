using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public interface IProductRepository
    {
        public Product GetProduct(int id);
    }
}
