using System;
using TerrainQuest.Generator.Helpers;
using Xunit;

namespace TerrainQuest.Tests.Generator.Helpers
{
    public class MathHelperTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        [InlineData(10)]
        public void Clamp_ValueWithinRangeIsReturnedAsIs(int expected)
        {
            // Arrange
            var min = 0;
            var max = 10;

            // Act
            var actual = MathHelper.Clamp(expected, min, max);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-10, 0)]
        [InlineData(-1, 0)]
        [InlineData(11, 10)]
        [InlineData(20, 10)]
        public void Clamp_ValueOutsideRangeReturnsMinOrMax(int value, int expected)
        {
            // Arrange
            var min = 0;
            var max = 10;

            // Act
            var actual = MathHelper.Clamp(value, min, max);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Clamp_RandomValueIsAlwaysWithinMinOrMax()
        {
            // Arrange
            var min = 0;
            var max = 10;
            var iterations = 1000000;
            var rand = new Random();

            for(int i = 0; i < iterations; i++)
            {
                var value = rand.Next(int.MinValue, int.MaxValue);

                // Act
                var actual = MathHelper.Clamp(value, min, max);

                // Assert
                Assert.InRange(actual, min, max);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(31)]
        public void IsPowerOfTwo_ReturnsTrueForExpectedPositivePowersOfTwo(int exponent)
        {
            // Arrange
            var expected = (int)Math.Pow(2, exponent);

            // Act
            var result = MathHelper.IsPowerOfTwo(expected);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(31)]
        public void IsPowerOfTwo_ReturnsTrueForExpectedNegativePowersOfTwo(int exponent)
        {
            // Arrange
            var expected = -(int)Math.Pow(2, exponent);

            // Act
            var result = MathHelper.IsPowerOfTwo(expected);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(30)]
        public void NextPowerOfTwo_ReturnsExpectedPowerOfTwo(int previousExp)
        {
            // Arrange
            var previous = (int)Math.Pow(2, previousExp) + 1;
            var expected = (int)Math.Pow(2, previousExp + 1);

            // Act
            var actual = MathHelper.NextPowerOfTwo(previous);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-2, -1)]
        [InlineData(2, 1)]
        [InlineData(-1, -0.5d)]
        [InlineData(1, 0.5d)]
        [InlineData(-4, -2)]
        [InlineData(4, 2)]
        public void Normalize_ToWithinSpecifiedRange(double value, double expected)
        {
            // Act
            var actual = MathHelper.Normalize(value, -2, 2, -1, 1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-2, -0.5)]
        [InlineData(-1, 0)]
        [InlineData(0, 0.5)]
        [InlineData(1, 1)]
        [InlineData(2, 1.5)]
        public void Normalize_ToWithinZeroAndOne(double value, double expected)
        {
            // Act
            var actual = MathHelper.Normalize(value, -1, 1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsExactCopyOfEqualSizedSource()
        {
            // Arrange
            var height = 2;
            var width = 2;
            var expected = new int[,] { { 1, 2 }, { 3, 4 } };

            // Act
            var actual = MathHelper.Copy(expected, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsSquareArrayIncludingSmallerSource()
        {
            // Arrange
            var height = 3;
            var width = 3;
            var source = new int[,] { { 1, 2 }, { 3, 4 } };
            var expected = new int[,] { { 1, 2, default(int) }, 
                                      { 3, 4, default(int) }, 
                                      { default(int), default(int), default(int) } };

            // Act
            var actual = MathHelper.Copy(source, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsSquareArrayIncludingBiggerSource()
        {
            // Arrange
            var height = 2;
            var width = 2;
            var expected = new int[,] { { 1, 2 }, { 4, 5 } };
            var source = new int[,] { { 1, 2, 3 },
                                      { 4, 5, 6 },
                                      { 7, 8, 9 } };

            // Act
            var actual = MathHelper.Copy(source, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsArrayIncludingSourceWithLargerWidth()
        {
            // Arrange
            var height = 2;
            var width = 3;
            var source = new int[,] { { 1, 2 }, { 3, 4 } };
            var expected = new int[,] { { 1, 2, default(int) },
                                      { 3, 4, default(int) } };

            // Act
            var actual = MathHelper.Copy(source, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsArrayIncludingBiggerSourceWithLargerWidth()
        {
            // Arrange
            var height = 2;
            var width = 3;
            var expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            var source = new int[,] { { 1, 2, 3 },
                                      { 4, 5, 6 },
                                      { 7, 8, 9 } };

            // Act
            var actual = MathHelper.Copy(source, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsArrayIncludingSourceWithLargerHeight()
        {
            // Arrange
            var height = 3;
            var width = 2;
            var source = new int[,] { { 1, 2 }, { 3, 4 } };
            var expected = new int[,] { { 1, 2 }, { 3, 4 }, { default(int), default(int) } };

            // Act
            var actual = MathHelper.Copy(source, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Copy_ReturnsArrayIncludingBiggerSourceWithLargerHeight()
        {
            // Arrange
            var height = 3;
            var width = 2;
            var expected = new int[,] { { 1, 2 }, { 4, 5 }, { 7, 8 } };
            var source = new int[,] { { 1, 2, 3 },
                                      { 4, 5, 6 },
                                      { 7, 8, 9 } };

            // Act
            var actual = MathHelper.Copy(source, height, width);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
