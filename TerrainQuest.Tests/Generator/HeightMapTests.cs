using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator;
using Xunit;

namespace TerrainQuest.Tests.Generator
{
    public class HeightMapTests
    {
        [Fact]
        public void MaxHeight_ReturnsMinValueWhenInitializedWithoutSource()
        {
            // Arrange
            var size = 2;
            var map = new HeightMap(size);

            // Act
            var actual = map.MaxHeight;

            // Assert
            Assert.Equal(double.MinValue, actual);
        }

        [Fact]
        public void MinHeight_ReturnsMaxValueWhenInitializedWithoutSource()
        {
            // Arrange
            var size = 2;
            var map = new HeightMap(size);

            // Act
            var actual = map.MinHeight;

            // Assert
            Assert.Equal(double.MaxValue, actual);
        }
    }
}
