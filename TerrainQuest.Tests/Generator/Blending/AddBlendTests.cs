using TerrainQuest.Generator;
using TerrainQuest.Generator.Blending;
using Xunit;

namespace TerrainQuest.Tests.Generator.Blending
{
    public class AddBlendTests
    {
        private readonly IBlendMode blendMode = new AddBlend();

        [Fact]
        public void Blend_ReturnsAddedHeightMaps()
        {
            // Arrange
            var data = new double[,] { { 0.1d, 0.2d }, 
                                       { 0.3d, 0.4d } };
            var expected = new double[,] { { 0.2d, 0.4d }, 
                                           { 0.6d, 0.8d } };
            var heightMapA = new HeightMap(data);
            var heightMapB = new HeightMap(data);

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
            var data = new double[,] { { 0.1d, 0.2d },
                                       { 0.3d, 0.4d } };
            var unchanged = new HeightMap(data);
            var heightMapA = new HeightMap(data);
            var heightMapB = new HeightMap(data);

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
            var data = new double[,] { { 0.1d, 0.2d },
                                       { 0.3d, 0.4d } };
            var unchanged = new HeightMap(data);
            var heightMapA = new HeightMap(data);
            var heightMapB = new HeightMap(data);

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
            var dataA = new double[,] { { 0.1d, 0.2d }, 
                                        { 0.3d, 0.4d } };
            var dataB = new double[,] { { 0.1d, 0.2d, 0.3d }, 
                                        { 0.4d, 0.5d, 0.6d }, 
                                        { 0.7d, 0.8d, 0.9d } };
            var expected = new double[,] { { 0.2d, 0.4d }, 
                                           { 0.7d, 0.9d } };
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
        public void Blend_RightSourceIsSmaller()
        {
            // Arrange
            var dataA = new double[,] { { 0.1d, 0.2d, 0.3d }, 
                                        { 0.4d, 0.5d, 0.6d }, 
                                        { 0.7d, 0.8d, 0.9d } };
            var dataB = new double[,] { { 0.1d, 0.2d }, 
                                        { 0.3d, 0.4d } };
            var expected = new double[,] { { 0.2d, 0.4d, 0.3d }, 
                                           { 0.7d, 0.9d, 0.6d }, 
                                           { 0.7d, 0.8d, 0.9d } };
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

    }
}
