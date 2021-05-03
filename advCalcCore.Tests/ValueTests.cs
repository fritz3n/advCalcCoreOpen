using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace advCalcCore.Tests
{
    public class ValueTests
    {
        [Fact]
        public void IntValueTest()
        {
            Value a = new IntValue(3);
            Value b = new IntValue(6);
            Value c = new IntValue(-4);

            Assert.Equal(new IntValue(3 + 6), a + b);
            Assert.Equal(new IntValue(-4 * 3), a * c);
            Assert.Equal(new FractionValue(3, 6), a / b);
            Assert.Equal(new IntValue(3 % 6), a % b);
            Assert.Equal(new DecimalValue((decimal)Math.Pow(3, -4)), a ^ c);
        }

        [Fact]
        public void FractionValueTest()
        {
            Assert.Equal(new FractionValue(BigInteger.Parse("79228162514264337593543950335")), (FractionValue)decimal.MaxValue);

            Assert.Equal(new FractionValue(BigInteger.Parse("-15845632502852867518708790067"), BigInteger.Parse("2000000000000000000000000000")),
                         (FractionValue)(-7.9228162514264337593543950335M)); //smallest negative decimal
        }
    }
}
