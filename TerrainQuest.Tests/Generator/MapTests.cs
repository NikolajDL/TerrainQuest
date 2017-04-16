using System;
using TerrainQuest.Generator;
using Xunit;

namespace TerrainQuest.Tests.Generator
{
    public class MapTests
    {
        [Fact]
        public void Constructor_SingleSizeYieldsQuadraticMap()
        {
            // Arrange
            var size = 3;

            // Act
            var map = new Map<int>(size);

            // Assert
            Assert.Equal(size, map.Width);
            Assert.Equal(size, map.Height);
        }

        [Fact]
        public void Constructor_WithSpecificDimensions()
        {
            // Arrange
            var width = 5;
            var height = 3;

            // Act
            var map = new Map<int>(height, width);

            // Assert
            Assert.Equal(width, map.Width);
            Assert.Equal(height, map.Height);
        }

        [Fact]
        public void Constructor_WithSourceMapOfSameSize()
        {
            // Arrange
            var width = 3;
            var height = 2;
            var source = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            
            // Act
            var map = new Map<int>(height, width, source);

            // Assert
            Assert.Equal(width, map.Width);
            Assert.Equal(height, map.Height);
            Assert.Equal(source, map.Data);
        }

        [Fact]
        public void Constructor_WithSourceMapOfSmallerSize()
        {
            // Arrange
            var size = 3;
            var expectedSource = new int[,] { { 1, 2, default(int) }, { 4, 5, default(int)}, { default(int), default(int), default(int) } };
            var source = new int[,] { { 1, 2 }, { 4, 5 } };

            // Act
            var map = new Map<int>(size, size, source);

            // Assert
            Assert.Equal(expectedSource, map.Data);
        }

        [Fact]
        public void Constructor_WithSourceMapOfBiggerSize()
        {
            // Arrange
            var size = 2;
            var expectedSource = new int[,] { { 1, 2 }, { 4, 5 } };
            var source = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            
            // Act
            var map = new Map<int>(size, size, source);

            // Assert
            Assert.Equal(expectedSource, map.Data);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValueWithinRange()
        {
            // Arrange
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);

            // Act
            var actual = map.GetValue(1, 1);

            // Assert
            Assert.Equal(source[1, 1], actual);
        }

        [Fact]
        public void GetValue_RowOutOfRangeThrowsArgumentOutOfRangeExceptions()
        {
            // Arrange
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                // Act
                var actual = map.GetValue(2, 1);
            });
        }

        [Fact]
        public void GetValue_ColumnOutOfRangeThrowsArgumentOutOfRangeExceptions()
        {
            // Arrange
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                // Act
                var actual = map.GetValue(1, 2);
            });
        }

        [Fact]
        public void SetValue_SetsExpectedValue()
        {
            // Arrange
            var expectedValue = 99;
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);
            
            // Act
            map.SetValue(1, 1, expectedValue);

            // Assert
            Assert.Equal(expectedValue, map.Data[1, 1]);
        }

        [Fact]
        public void SetValue_RowOutOfRangeThrowsArgumentOutOfRangeExceptions()
        {
            // Arrange
            var expectedValue = 99;
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                // Act
                map.SetValue(2, 1, expectedValue);
            });
        }

        [Fact]
        public void SetValue_ColumnOutOfRangeThrowsArgumentOutOfRangeExceptions()
        {
            // Arrange
            var expectedValue = 99;
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                // Act
                map.SetValue(1, 2, expectedValue);
            });
        }

        [Fact]
        public void IsInRange_DimensionWithinRangeReturnTrue()
        {
            // Arrange
            var size = 2;
            var map = new Map<int>(size);

            // Act
            var actual = map.IsInRange(1, 1);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(2, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void IsInRange_DimensionsOutsideRangeReturnFalse(int r, int c)
        {
            // Arrange
            var size = 2;
            var map = new Map<int>(size);

            // Act
            var actual = map.IsInRange(r, c);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Data_ReturnsRawMapData()
        {
            // Arrange
            var expectedValue = 99;
            var source = new int[,] { { 1, 2 }, { 4, 5 } };
            var map = new Map<int>(source);

            // Act
            var actual = map.Data;

            // Assert
            Assert.Equal(source, actual);
        }
    }
}
