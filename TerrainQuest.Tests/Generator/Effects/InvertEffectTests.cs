using Moq;
using System.Runtime.Serialization;
using TerrainQuest.Generator;
using TerrainQuest.Generator.Effects;
using TerrainQuest.Generator.Helpers;
using Xunit;

namespace TerrainQuest.Tests.Generator.Effects
{
    public class InvertEffectTests
    {

        [Fact]
        public void Calculate_AppliesEffectOnHeightMap()
        {
            // Arrange
            var data = new double[,] { { 0d, 0.25d, 0.5d, 0.75d, 1d } };
            var heightMap = new HeightMap(data);
            var expected = MockImageEffect(heightMap);
            var effect = new InvertEffect();

            // Act
            var actual = effect.Calculate(heightMap).Data;

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0]);
            Assert.Equal(expected[0, 1], actual[0, 1]);
            Assert.Equal(expected[0, 2], actual[0, 2]);
            Assert.Equal(expected[0, 3], actual[0, 3]);
            Assert.Equal(expected[0, 4], actual[0, 4]);
        }

        private double[,] MockImageEffect(HeightMap h)
        {
            var result = h.Clone();
            h.ForEach((r, c) => { result[r, c] = 1 - result[r, c]; });
            return result.Data;
        }

        private static SerializationInfo MockSerializationInfo<T>()
        {
            var formatterConverter = new Mock<IFormatterConverter>().Object;
            return new SerializationInfo(typeof(T), formatterConverter);
        }
    }
}
