using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void FirstFact()
        {
            Assert.Equal(5, 3 + 2);
        }

        [Theory]
        [InlineData(5, 3, 2)]
        [InlineData(7, 3, 4)]
        [InlineData(-1, -3, 2)]
        public void FirstTheory(int expected, int addend1, int addend2)
        {
            Assert.Equal(expected, addend1 + addend2);
        }

        [Fact(DisplayName = "Ignored Test - Custom Display Name", Skip = "this can be anything")]
        public void ThisIsIgnored()
        {
            // todo: fix this this
        }
    }
}
