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
    public class GammaEffectTests
    {
        [Fact]
        public void Constructor_InitializesExpectedProperties()
        {
            // Act
            var effect = new GammaEffect(0.5d);

            // Assert
            Assert.Equal(0.5d, effect.Gamma);
        }

        [Fact]
        public void Constructor_ZeroGammaThrowsArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                // Act
                var effect = new GammaEffect(0d);
            });
        }

        [Fact]
        public void Constructor_NegativeGammaThrowsArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                // Act
                var effect = new GammaEffect(-1d);
            });
        }

        [Fact]
        public void ISerializable_SerializeAndDeserializeCorrectly()
        {
            // Arrange
            var expected = new GammaEffect(0.5d);
            var info = MockSerializationInfo<GammaEffect>();

            // Act
            expected.GetObjectData(info, new StreamingContext());
            var actual = new GammaEffect(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.Gamma, actual.Gamma);
        }

        [Theory]
        [InlineData(0.1d)]
        [InlineData(0.5d)]
        [InlineData(0.9d)]
        [InlineData(1d)]
        [InlineData(2d)]
        [InlineData(5d)]
        [InlineData(10d)]
        public void Calculate_AppliesEffectOnHeightMap(double gamma)
        {
            // Arrange
            var data = new double[,] { { 0d, 0.25d, 0.5d, 0.75d, 1d } };
            var heightMap = new HeightMap(data);
            var expected = MockImageEffect(heightMap, gamma);
            var effect = new GammaEffect(gamma);

            // Act
            var actual = effect.Calculate(heightMap).Data;

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0]);
            Assert.Equal(expected[0, 1], actual[0, 1]);
            Assert.Equal(expected[0, 2], actual[0, 2]);
            Assert.Equal(expected[0, 3], actual[0, 3]);
            Assert.Equal(expected[0, 4], actual[0, 4]);
        }

        private double[,] MockImageEffect(HeightMap h, double gamma)
        {
            var result = h.Clone();

            double gammaCorrection = 1 / gamma;

            h.ForEach((r, c) => { result[r, c] = Math.Pow(result[r, c], gammaCorrection); });
            return result.Data;
        }

        private static SerializationInfo MockSerializationInfo<T>()
        {
            var formatterConverter = new Mock<IFormatterConverter>().Object;
            return new SerializationInfo(typeof(T), formatterConverter);
        }
    }
}
