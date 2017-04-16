using System;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A brightening <see cref="IBlendMode"/> where the highest value is copies to the result
    /// </summary>
    public class LightenBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                var a = result[r, c];
                var b = right.IsInRange(r, c) ? right[r, c] : 0d;
                result[r, c] = Math.Max(a, b);
            });

            return result;
        }
    }
}
