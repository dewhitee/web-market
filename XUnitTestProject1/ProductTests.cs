using System;
using System.Collections.Generic;
using System.Text;
using WebMarket.Models;
using Xunit;

namespace XUnitTestProject1
{
    public class ProductTests
    {
        [Fact]
        public void CompareByNameTest()
        {
            Product product1 = new Product
            {
                Name = "Hello World"
            };

            Product product2 = new Product
            {
                Name = "Bye World!"
            };

            int expected = -1;
            int actual = product1.CompareTo(product2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompareByDiscount()
        {
            Product product1 = new Product
            {
                Discount = 30
            };

            Product product2 = new Product
            {
                Discount = 70
            };

            int expected = 1;
            int actual = product1.CompareTo(product2);

            Assert.Equal(expected, actual);
        }
    }
}
