using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A screen brightening <see cref="IBlendMode"/> 
    /// which is an invertion of the <see cref="MultiplyBlend"/> mode.
    /// </summary>
    public class ScreenBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                var a = result[r, c];
                var b = right.IsInRange(r, c) ? right[r, c] : 1d;
                result[r, c] = 1 - (1 - a) * (1 - b);
            });

            return result;
        }
    }
}
