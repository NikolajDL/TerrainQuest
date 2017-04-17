using TerrainQuest.Generator;
using TerrainQuest.Generator.Helpers;
using Xunit;

namespace TerrainQuest.Tests.Generator.Helpers
{
    public class MapExtensionsTests
    {
        [Fact]
        public void ForEach_TraversesMapWithCorrectRowAndColumnOrder()
        {
            // Arrange
            var source = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            var expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var map = new Map<int>(source);
            var i = 0;

            // Act
            map.ForEach((r, c) => {
                Assert.Equal(expected[i], map[r, c]);

                i++;
            });
        }

        [Fact]
        public void ForEach_TraversesMapWithCorrectRowColumnAndIndexOrder()
        {
            // Arrange
            var source = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            var expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var map = new Map<int>(source);
            var expectedI = 0;

            // Act
            map.ForEach((r, c, i) => {
                Assert.Equal(expected[i], map[r, c]);
                Assert.Equal(expectedI, i);

                expectedI++;
            });
        }
    }
}
