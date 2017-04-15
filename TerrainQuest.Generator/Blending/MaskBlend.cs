using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A masking <see cref="IBlendMode"/> where the <see cref="HeightMap"/>s are multiplied together,
    /// similarly to <see cref="MultiplyBlend"/> except if the masking (right) map is smaller than
    /// the source (left) map, the overflowing pixels will be black (i.e. they're multiplied by zero). 
    /// </summary>
    public class MaskBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                result[r, c] *= right.CheckPositionIsValid(r, c) ? right[r, c] : 0d;
            });

            return result;
        }
    }
}
