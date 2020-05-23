using System;
using System.Collections.Generic;
using WebMarket.Models;
using Xunit;

namespace XUnitTestProject3
{
    public class UnitTest1
    {
        private List<UserComment> mockUserComments = new List<UserComment>
        {
            new UserComment
            {
                Rate = 3.5f
            },
            new UserComment
            {
                Rate = 5f
            },
            new UserComment
            {
                Rate = 2f
            },
            new UserComment
            {
                Rate = 0f
            }
        };
        private List<Product> mockProducts = new List<Product>
        {
            new Product
            {
                Price = 20.99M,
                Discount = 40
            },
            new Product
            {
                Price = 14.99M,
                Discount = 90
            },
            new Product
            {
                Price = 7.99M,
                Discount = 10
            },
            new Product
            {
                Price = 2.99M,
                Discount = 70
            },
        };

        [Fact]
        public void ProductCompareByPriceTest()
        {
            int expected = 1;
            int actual = Product.CompareByPrice(mockProducts[0], mockProducts[1]);
            Assert.Equal(expected, actual);

            expected = -1;
            actual = Product.CompareByPrice(mockProducts[3], mockProducts[2]);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProductCompareByDiscountTest()
        {
            int expected = 1;
            int actual = Product.CompareByDiscount(mockProducts[0], mockProducts[1]);
            Assert.Equal(expected, actual);

            expected = -1;
            actual = Product.CompareByDiscount(mockProducts[3], mockProducts[2]);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UserCommentsGetRateSum()
        {
            float expected = 3.5f + 5f + 2f + 0f;
            float actual = Product.GetRateSum(mockUserComments);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UserCommentsGetNotNulled()
        {
            uint expected = 3;
            uint actual = Product.GetTotalCountOfNotNulledComments(mockUserComments);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UserCommentsGetStarsCount()
        {
            float expected = (3.5f + 5f + 2f) / 3;
            float actual = ComparisonViewModel.GetStarsValue(mockUserComments);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Programming")]
        [InlineData("choose type")]
        [InlineData("CHOOSE TYPE")]
        [InlineData("Choose Type")]
        public void CheckTypeString(string str)
        {
            var expected = str;
            var actual = Product.CheckTypeString(str);
            Assert.Equal(expected, actual);
        }
    }
}
