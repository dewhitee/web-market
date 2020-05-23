using System;
using WebMarket.Models;
using Xunit;

namespace XUnitTestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void ProductCompareByPriceTest()
        {
            Product product1 = new Product
            {
                Price = 20.99M
            };

            Product product2 = new Product()
            {
                Price = 14.99M
            };
            
            int expected = 1;
            int actual = Product.CompareByPrice(product1, product2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProductCompareByDiscountTest()
        {
            Product product1 = new Product
            {
                Discount = 40
            };

            Product product2 = new Product
            {
                Discount = 90
            };

            int expected = 1;
            int actual = Product.CompareByDiscount(product1, product2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ...
    }
}
