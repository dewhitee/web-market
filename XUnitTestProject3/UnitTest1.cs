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
        private List<string> mockProductTags = new List<string>
        {
            "Programming", "Testing", "3D", "2D", "C++", "Python"
        };
        private List<string> mockChoosenTags = new List<string>
        {
            "Programming", "3D", "Python", "", "nothing"
        };

        [Fact]
        public void CompareByPriceTest()
        {
            Assert.Equal(1, Product.CompareByPrice(mockProducts[0], mockProducts[1]));
            Assert.Equal(-1, Product.CompareByPrice(mockProducts[3], mockProducts[2]));
        }

        [Fact]
        public void CompareByDiscountTest()
        {
            Assert.Equal(1, Product.CompareByDiscount(mockProducts[0], mockProducts[1]));
            Assert.Equal(-1, Product.CompareByDiscount(mockProducts[3], mockProducts[2]));
        }

        [Fact]
        public void GetRateSumTest()
        {
            Assert.Equal(3.5f + 5f + 2f + 0f, Product.GetRateSum(mockUserComments));
        }

        [Fact]
        public void GetTotalCountOfNotNulledCommentsTest()
        {
            Assert.Equal(3u, Product.GetTotalCountOfNotNulledComments(mockUserComments));
        }

        [Fact]
        public void GetStarsValueTest()
        {
            Assert.Equal((3.5f + 5f + 2f) / 3, ComparisonViewModel.GetStarsValue(mockUserComments));
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(1, 2)]
        [InlineData(1, 4)]
        [InlineData(0, 1)]
        [InlineData(-1, 8)]
        [InlineData(-1, -2)]
        [InlineData(-1, 0)]
        public void GetStarsCountTest(int expected, int stars)
        {
            var productComments = new List<UserComment>
            {
                new UserComment
                {
                    Rate = 3.5f
                },
                new UserComment
                {
                    Rate = 3.2f
                },
                new UserComment
                {
                    Rate = 3.8f
                },
                new UserComment
                {
                    Rate = 5f
                },
                new UserComment
                {
                    Rate = 4.1f
                },
                new UserComment
                {
                    Rate = 2f
                },
                new UserComment
                {
                    Rate = 8f
                },
                new UserComment
                {
                    Rate = -2f
                }
            };
            Assert.Equal(expected, Product.GetStarsCount(stars, productComments));
        }

        [Theory]
        [InlineData("1.2 Kb", 1200)]
        [InlineData("1 Kb", 1000)]
        [InlineData("420 bytes", 420)]
        [InlineData("2.5 Mb", 2500000)]
        [InlineData("2 Mb", 2000000)]
        [InlineData("4.7 Gb", 4700000000)]
        [InlineData("4 Gb", 4000000000)]
        [InlineData("Empty file", 0)]
        [InlineData("", -20)]
        public void FormalFileSizeTest(string expected, long fileSize)
        {
            Assert.Equal(expected, Product.FormatFileSize(fileSize));
        }

        [Theory]
        [InlineData("Programming")]
        [InlineData("choose type")]
        [InlineData("CHOOSE TYPE")]
        [InlineData("Choose Type")]
        public void CheckTypeStringTest(string str)
        {
            Assert.Equal(str, Product.CheckTypeString(str));
        }

        [Fact]
        public void ContainsTagsTest()
        {
            //! true cases
            Assert.True(Product.ContainsTags(new List<string> { "Programming" }, mockProductTags, false));
            Assert.True(Product.ContainsTags(new List<string> { "Programming" }, mockProductTags, true));
            Assert.True(Product.ContainsTags(mockChoosenTags, mockProductTags, false));

            //! false cases
            Assert.False(Product.ContainsTags(mockChoosenTags, mockProductTags, true));
            Assert.False(Product.ContainsTags(new List<string> { "nothing" }, mockProductTags, true));

            //! null reference testing

            Assert.False(Product.ContainsTags(null, mockProductTags, false));
            Assert.False(Product.ContainsTags(null, null, false));
            Assert.False(Product.ContainsTags(mockChoosenTags, null, false));

            //! null reference with fully matching

            Assert.False(Product.ContainsTags(null, mockProductTags, true));
            Assert.False(Product.ContainsTags(null, null, true));
            Assert.False(Product.ContainsTags(mockChoosenTags, null, true));
        }
    }
}
