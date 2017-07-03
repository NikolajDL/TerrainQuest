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
    public class GaussianBlurEffectTests
    {
        private const int Precision = 13;

        [Fact]
        public void Constructor_InitializesCorrectDeviation()
        {
            // Act
            var effect = new GaussianBlurEffect(0.5d);

            // Assert
            Assert.Equal(0.5d, effect.Deviation);
        }

        [Fact]
        public void Constructor_InitializesCorrectRadius()
        {
            // Act
            var effect = new GaussianBlurEffect(0.5d, 100);

            // Assert
            Assert.Equal(100, effect.Radius);
        }

        [Fact]
        public void Constructor_InitializesRadiusToNullByDefault()
        {
            // Act
            var effect = new GaussianBlurEffect(0.5d);

            // Assert
            Assert.Null(effect.Radius);
        }

        [Fact]
        public void ISerializable_SerializeAndDeserializeCorrectly()
        {
            // Arrange
            var expected = new GaussianBlurEffect(0.5d, 100);
            var info = MockSerializationInfo<GaussianBlurEffect>();

            // Act
            expected.GetObjectData(info, new StreamingContext());
            var actual = new GaussianBlurEffect(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.Deviation, actual.Deviation);
            Assert.Equal(expected.Radius, actual.Radius);
        }

        [Fact]
        public void ISerializable_SerializeAndDeserializeWithNullRadiusCorrectly()
        {
            // Arrange
            var expected = new GaussianBlurEffect(0.5d);
            var info = MockSerializationInfo<GaussianBlurEffect>();

            // Act
            expected.GetObjectData(info, new StreamingContext());
            var actual = new GaussianBlurEffect(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.Deviation, actual.Deviation);
            Assert.Null(actual.Radius);
        }

        [Fact]
        public void Calculate_AppliesEffectOnHeightMap()
        {
            // Arrange
            var deviation = 1.5d;
            var radius = 5;
            var kernel = new double[5, 1] { { 0.120078384243213d }, { 0.23388075658535d }, { 0.292081718342872d }, { 0.23388075658535d }, { 0.120078384243213d } };
            var data = new double[,] { { 0d, 0.25d, 0.5d, 0.75d, 1d } };
            var heightMap = new HeightMap(data);
            var expected = MockImageEffect(heightMap, kernel);
            var effect = new GaussianBlurEffect(deviation, radius);

            // Act
            var actual = effect.Calculate(heightMap).Data;

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0], Precision);
            Assert.Equal(expected[0, 1], actual[0, 1], Precision);
            Assert.Equal(expected[0, 2], actual[0, 2], Precision);
            Assert.Equal(expected[0, 3], actual[0, 3], Precision);
            Assert.Equal(expected[0, 4], actual[0, 4], Precision);
        }

        // TODO: Test that the mean error of the blur result, 
        // compared to "real" (convolution of gaussian function)
        // gaussian blur, is within a reasonable threshold.

        private HeightMap MockImageEffect(HeightMap heightMap, double[,] kernel)
        {
            var resultA = new HeightMap(heightMap.Height, heightMap.Width);
            var resultB = new HeightMap(heightMap.Height, heightMap.Width);
            int half = 5 / 2;
            heightMap.ForEach((r, c) => {
                var sum = 0d;
                for(var i = 0; i < 5; i++)
                {
                    var x = MathHelper.Clamp(c + i - half, 0, 4);
                    sum += heightMap[0, x] * kernel[i, 0];
                }
                resultA[r, c] = sum;
            });
            resultA.ForEach((r, c) => {
                var sum = 0d;
                for (var i = 0; i < 5; i++)
                {
                    sum += resultA[0, c] * kernel[i, 0];
                }
                resultB[r, c] = sum;
            });
            return resultB;
        }

        private static SerializationInfo MockSerializationInfo<T>()
        {
            var formatterConverter = new Mock<IFormatterConverter>().Object;
            return new SerializationInfo(typeof(T), formatterConverter);
        }
    }
}
