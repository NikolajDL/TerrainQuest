using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A subtract <see cref="IBlendMode"/> where the second <see cref="HeightMap"/> 
    /// is subtracted from the first.
    /// </summary>
    public class SubtractBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                result[r, c] -= right.IsInRange(r, c) ? right[r, c] : 0d;
            });

            return result;
        }
    }
}
