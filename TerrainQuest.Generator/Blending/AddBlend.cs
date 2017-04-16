using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// An add <see cref="IBlendMode"/> where the <see cref="HeightMap"/>s are added together. 
    /// </summary>
    public class AddBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                result[r, c] += right.IsInRange(r, c) ? right[r, c] : 0d;
            });

            return result;
        }
    }
}
