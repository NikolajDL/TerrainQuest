using TerrainQuest.Generator;
using TerrainQuest.Generator.Blending;
using Xunit;

namespace TerrainQuest.Tests.Generator.Blending
{
    public class MultiplyBlendTests
    {
        private readonly IBlendMode blendMode = new MultiplyBlend();
        private const int Precision = 15;

        [Fact]
        public void Blend_ReturnsAddedHeightMaps()
        {
            // Arrange
            var dataA = new double[,] { { 0.5d, 0.5d },
                                        { 0.5d, 0.5d } };
            var dataB = new double[,] { { 0.0d, 0.5d },
                                        { 1.0d, 2.0d } };
            var expected = new double[,] { { 0.0d, 0.25d },
                                           { 0.5d, 1.0d } };
            var heightMapA = new HeightMap(dataA);
            var heightMapB = new HeightMap(dataB);

            // Act
            var actual = blendMode.Blend(heightMapA, heightMapB);

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0]);
            Assert.Equal(expected[0, 1], actual[0, 1]);
            Assert.Equal(expected[1, 0], actual[1, 0]);
            Assert.Equal(expected[1, 1], actual[1, 1]);
        }

        [Fact]
        public void Blend_LeftSourceIsNotAltered()
        {
            // Arrange
            var dataA = new double[,] { { 0.5d, 0.5d },
                                        { 0.5d, 0.5d } };
            var dataB = new double[,] { { 0.0d, 0.5d },
                                        { 1.0d, 2.0d } };
            var unchanged = new HeightMap(dataA);
            var heightMapA = new HeightMap(dataA);
            var heightMapB = new HeightMap(dataB);

            // Act
            var actual = blendMode.Blend(heightMapA, heightMapB);

            // Assert
            Assert.Equal(unchanged[0, 0], heightMapA[0, 0]);
            Assert.Equal(unchanged[0, 1], heightMapA[0, 1]);
            Assert.Equal(unchanged[1, 0], heightMapA[1, 0]);
            Assert.Equal(unchanged[1, 1], heightMapA[1, 1]);
        }

        [Fact]
        public void Blend_RightSourceIsNotAltered()
        {
            // Arrange
            var dataA = new double[,] { { 0.5d, 0.5d },
                                        { 0.5d, 0.5d } };
            var dataB = new double[,] { { 0.0d, 0.5d },
                                        { 1.0d, 2.0d } };
            var unchanged = new HeightMap(dataB);
            var heightMapA = new HeightMap(dataA);
            var heightMapB = new HeightMap(dataB);

            // Act
            var actual = blendMode.Blend(heightMapA, heightMapB);

            // Assert
            Assert.Equal(unchanged[0, 0], heightMapB[0, 0]);
            Assert.Equal(unchanged[0, 1], heightMapB[0, 1]);
            Assert.Equal(unchanged[1, 0], heightMapB[1, 0]);
            Assert.Equal(unchanged[1, 1], heightMapB[1, 1]);
        }

        [Fact]
        public void Blend_RightSourceIsBigger()
        {
            // Arrange
            var dataA = new double[,] { { 0.5d, 0.5d },
                                        { 0.5d, 0.5d } };
            var dataB = new double[,] { { 0.0d, 0.5d, 0.5d },
                                        { 1.0d, 2.0d, 1.0d },
                                        { 0.5d, 1.0d, 2.0d } };
            var expected = new double[,] { { 0.0d, 0.25d },
                                           { 0.5d, 1.0d } };
            var heightMapA = new HeightMap(dataA);
            var heightMapB = new HeightMap(dataB);

            // Act
            var actual = blendMode.Blend(heightMapA, heightMapB);

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0], Precision);
            Assert.Equal(expected[0, 1], actual[0, 1], Precision);
            Assert.Equal(expected[1, 0], actual[1, 0], Precision);
            Assert.Equal(expected[1, 1], actual[1, 1], Precision);
        }

        [Fact]
        public void Blend_RightSourceIsSmaller()
        {
            // Arrange
            var dataA = new double[,] { { 0.5d, 0.5d, 0.5d },
                                        { 0.5d, 0.5d, 0.5d },
                                        { 0.5d, 0.5d, 0.5d } };
            var dataB = new double[,] { { 0.0d, 0.5d },
                                        { 1.0d, 2.0d } };
            var expected = new double[,] { { 0.0d, 0.25d, 0.5d },
                                           { 0.5d, 1.0d, 0.5d },
                                           { 0.5d, 0.5d, 0.5d } };
            var heightMapA = new HeightMap(dataA);
            var heightMapB = new HeightMap(dataB);

            // Act
            var actual = blendMode.Blend(heightMapA, heightMapB);

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0], Precision);
            Assert.Equal(expected[0, 1], actual[0, 1], Precision);
            Assert.Equal(expected[1, 0], actual[1, 0], Precision);
            Assert.Equal(expected[1, 1], actual[1, 1], Precision);
        }

    }
}
