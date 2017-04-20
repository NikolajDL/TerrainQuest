using System.Drawing;
using TerrainQuest.Generator;
using Xunit;

namespace TerrainQuest.Tests.Generator
{
    public class HeightMapTests
    {
        [Fact]
        public void MaxHeight_ReturnsZeroWhenInitializedWithoutSource()
        {
            // Arrange
            var size = 2;
            var map = new HeightMap(size);

            // Act
            var actual = map.MaxHeight;

            // Assert
            Assert.Equal(default(double), actual);
        }

        [Fact]
        public void MinHeight_ReturnsZeroWhenInitializedWithoutSource()
        {
            // Arrange
            var size = 2;
            var map = new HeightMap(size);

            // Act
            var actual = map.MinHeight;

            // Assert
            Assert.Equal(default(double), actual);
        }

        [Fact]
        public void Constructor_WithSourceMapOfSameSize()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d, 3d }, { 4d, 5d, 6d } };

            // Act
            var map = new HeightMap(source);

            // Assert
            Assert.Equal(source, map.Data);
        }

        [Fact]
        public void Constructor_WithSourceMapOfSmallerSize()
        {
            // Arrange
            var height = 2;
            var width = 3;
            var source = new double[,] { { 1d, 2d }, { 4d, 5d } };
            var expected = new double[,] { { 1d, 2d, default(double) }, { 4d, 5d, default(double) } };

            // Act
            var map = new HeightMap(height, width, source);

            // Assert
            Assert.Equal(expected, map.Data);
        }

        [Fact]
        public void Constructor_WithSourceRecalculatesMinMaxHeight()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };

            // Act
            var map = new HeightMap(source);

            // Assert
            Assert.Equal(1d, map.MinHeight);
            Assert.Equal(4d, map.MaxHeight);
        }

        [Fact]
        public void Constructor_WithSourceAndDimensionsRecalculatesMinMaxHeight()
        {
            // Arrange
            var size = 2;
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };

            // Act
            var map = new HeightMap(size, size, source);

            // Assert
            Assert.Equal(1d, map.MinHeight);
            Assert.Equal(4d, map.MaxHeight);
        }

        [Fact]
        public void Clone_ReturnsHeightMapWithExactlySameProperties()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var expected = new HeightMap(source);

            // Act
            var actual = expected.Clone();

            // Assert
            Assert.Equal(expected.Height, actual.Height);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.MinHeight, actual.MinHeight);
            Assert.Equal(expected.MaxHeight, actual.MaxHeight);
            Assert.Equal(expected.Data, actual.Data);
        }

        [Fact]
        public void Clone_ChangingClonePropertiesDoesNotChangeParent()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);
            var clone = map.Clone();

            // Act
            clone.SetValue(1, 1, 99d);

            // Assert
            Assert.NotEqual(map.Data[1, 1], clone.Data[1, 1]);
            Assert.Equal(99d, clone.Data[1, 1]);
        }

        [Fact]
        public void SetValue_MinHeightChangesGivenLowerInput()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);

            // Act
            var expected = map.MaxHeight;
            map.SetValue(1, 1, -1d);

            // Assert
            Assert.Equal(-1d, map.MinHeight);
            Assert.Equal(expected, map.MaxHeight);
        }

        [Fact]
        public void SetValue_MaxHeightChangesGivenHigherInput()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);

            // Act
            var expected = map.MinHeight;
            map.SetValue(1, 1, 10d);

            // Assert
            Assert.Equal(10d, map.MaxHeight);
            Assert.Equal(expected, map.MinHeight);
        }

        [Fact]
        public void SetValue_MinMaxHeightRemainsUnchangedIfValueIsWithin()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);

            // Act
            var expectedMin = map.MinHeight;
            var expectedMax = map.MaxHeight;
            map.SetValue(1, 1, 3d);

            // Assert
            Assert.Equal(expectedMin, map.MinHeight);
            Assert.Equal(expectedMax, map.MaxHeight);
        }

        [Fact]
        public void Data_SettingDataDoesNotUpdateMinHeight()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);

            // Act
            var expectedMin = map.MinHeight;
            var expectedMax = map.MaxHeight;
            map.Data[1, 1] = -1d;

            // Assert
            Assert.Equal(expectedMin, map.MinHeight);
            Assert.Equal(expectedMax, map.MaxHeight);
        }

        [Fact]
        public void Data_SettingDataDoesNotUpdateMaxHeight()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);

            // Act
            var expectedMin = map.MinHeight;
            var expectedMax = map.MaxHeight;
            map.Data[1, 1] = 10d;

            // Assert
            Assert.Equal(expectedMin, map.MinHeight);
            Assert.Equal(expectedMax, map.MaxHeight);
        }

        [Fact]
        public void RecalculateMinMaxValue_UpdatesMinMaxHeightAfterChangingData()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var map = new HeightMap(source);

            // Act
            map.Data[0, 1] = -1d;
            map.Data[1, 0] = 10d;
            map.RecalculateMinMaxValue();

            // Assert
            Assert.Equal(-1d, map.MinHeight);
            Assert.Equal(10d, map.MaxHeight);
        }

        [Fact]
        public void Flatten_SetsAllMapDataToZero()
        {
            // Arrange
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var expected = new double[,] { { 0d, 0d }, { 0d, 0d } };
            var map = new HeightMap(source);

            // Act
            map.Flatten();

            // Assert
            Assert.Equal(expected, map.Data);
        }

        [Fact]
        public void FlattenTo_SetsAllMapDataToZero()
        {
            // Arrange
            var value = 1.5d;
            var source = new double[,] { { 1d, 2d }, { 3d, 4d } };
            var expected = new double[,] { { value, value }, { value, value } };
            var map = new HeightMap(source);

            // Act
            map.FlattenTo(value);

            // Assert
            Assert.Equal(expected, map.Data);
        }

        [Fact]
        public void Normalize_ChangesDataToStayWithinZeroAndOne()
        {
            // Arrange
            var source = new double[,] { { -1, 0, 1 }, { -1, 0, 1 } };
            var expected = new double[,] { { 0, 0.5d, 1 }, { 0, 0.5d, 1 } };
            var map = new HeightMap(source);

            // Act
            map.Normalize();

            // Assert
            Assert.Equal(expected, map.Data);
            Assert.Equal(0, map.MinHeight);
            Assert.Equal(1, map.MaxHeight);
        }

        [Fact]
        public void Normalize_ChangesDataToStayWithinZeroAndOneBasedOnGivenMinMax()
        {
            // Arrange
            var source = new double[,] { { -1, 0, 1 }, { -1, 0, 1 } };
            var expected = new double[,] { { 0.25d, 0.5d, 0.75d }, { 0.25d, 0.5d, 0.75d } };
            var map = new HeightMap(source);

            // Act
            map.Normalize(-2, 2);

            // Assert
            Assert.Equal(expected, map.Data);
            Assert.Equal(0.25d, map.MinHeight);
            Assert.Equal(0.75d, map.MaxHeight);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(0.25, 64)]
        [InlineData(0.5, 128)]
        [InlineData(0.75, 191)]
        [InlineData(1, 255)]
        [InlineData(2, 255)]
        [InlineData(0.005, 1)]
        [InlineData(0.995, 254)]
        public void GetColor_MapsToGreyscaleColor(double height,int expectedValue)
        {
            // Arrange
            var source = new double[,] { { height } };
            var map = new HeightMap(source);
            var expected = Color.FromArgb(expectedValue, expectedValue, expectedValue);

            // Act
            var actual = map.GetColor(0, 0);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AsBitmap_ReturnsBitmapWithEveryPixelColorSet()
        {
            // Arrange
            var source = new double[,] { { 0.25, 0.5, 1 },
                                         { 0, 0.25, 0.5 },
                                         { 0, 0,   0.25 }};
            var expected = new int[,] { { 64, 128, 255 },
                                        {  0,  64, 128 },
                                        {  0,   0,  64 }};
            var map = new HeightMap(source);

            // Act
            var bitmap = map.AsBitmap();

            // Assert
            AssertBitmap(bitmap, expected);
        }

        private void AssertBitmap(Bitmap map, int[,] expected)
        {
            for(int r = 0; r < expected.GetLength(0); r++)
                for(int c = 0; c < expected.GetLength(1); c++)
                {
                    Assert.Equal(expected[r, c], map.GetPixel(c, r).R);
                    Assert.Equal(expected[r, c], map.GetPixel(c, r).G);
                    Assert.Equal(expected[r, c], map.GetPixel(c, r).B);
                    Assert.Equal(255, map.GetPixel(c, r).A);
                }
        }
    }
}
