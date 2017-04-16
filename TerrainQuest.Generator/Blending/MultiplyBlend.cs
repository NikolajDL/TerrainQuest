using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A multiply darkening <see cref="IBlendMode"/> where the <see cref="HeightMap"/>s are multiplied together,
    /// which usually leaves the result darkened. Any pixels in the source (left) map that isn't in the multiply (right) map
    /// is included (i.e. multiplied by 1). 
    /// </summary>
    public class MultiplyBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                result[r, c] *= right.IsInRange(r, c) ? right[r, c] : 1d;
            });

            return result;
        }
    }
}
