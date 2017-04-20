using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator;
using TerrainQuest.Generator.Effects;
using TerrainQuest.Generator.Helpers;
using Xunit;

namespace TerrainQuest.Tests.Generator.Effects
{
    public class ContrastEffectTests
    {
        [Fact]
        public void Constructor_InitializesExpectedProperties()
        {
            // Act
            var effect = new ContrastEffect(0.5d);

            // Assert
            Assert.Equal(0.5d, effect.Contrast);
        }

        [Fact]
        public void ISerializable_SerializeAndDeserializeCorrectly()
        {
            // Arrange
            var expected = new ContrastEffect(0.5d);
            var info = MockSerializationInfo<ContrastEffect>();

            // Act
            expected.GetObjectData(info, new StreamingContext());
            var actual = new ContrastEffect(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.Contrast, actual.Contrast);
        }

        [Theory]
        [InlineData(-20d)]
        [InlineData(-10d)]
        [InlineData(-1d)]
        [InlineData(0d)]
        [InlineData(0.1d)]
        [InlineData(0.5d)]
        [InlineData(0.9d)]
        [InlineData(1d)]
        [InlineData(2d)]
        [InlineData(10d)]
        [InlineData(20d)]
        [InlineData(40d)]
        public void Calculate_AppliesEffectOnHeightMap(double contrast)
        {
            // Arrange
            var data = new double[,] { { 0d, 0.25d, 0.5d, 0.75d, 1d } };
            var heightMap = new HeightMap(data);
            var expected = MockImageEffect(heightMap, contrast);
            var effect = new ContrastEffect(contrast);

            // Act
            var actual = effect.Calculate(heightMap).Data;

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0]);
            Assert.Equal(expected[0, 1], actual[0, 1]);
            Assert.Equal(expected[0, 2], actual[0, 2]);
            Assert.Equal(expected[0, 3], actual[0, 3]);
            Assert.Equal(expected[0, 4], actual[0, 4]);
        }

        private double[,] MockImageEffect(HeightMap h, double brightness)
        {
            const double High = 259d;
            const double Low = 255d;

            double F = (High * (brightness + Low)) / (Low * (High - brightness));

            var result = h.Clone();
            h.ForEach((r, c) => { result[r, c] = F * (result[r, c] - 0.5d) + 0.5d; });
            return result.Data;
        }

        private static SerializationInfo MockSerializationInfo<T>()
        {
            var formatterConverter = new Mock<IFormatterConverter>().Object;
            return new SerializationInfo(typeof(T), formatterConverter);
        }
    }
}
