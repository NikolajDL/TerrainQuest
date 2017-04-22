using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A difference <see cref="IBlendMode"/> where the second <see cref="HeightMap"/> 
    /// is subtracted from the first.
    /// </summary>
    public class DifferenceBlend : IBlendMode
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
                var ab = a - b;
                var ba = b - a;
                result[r, c] = ba > 0 ? ba : ab;
            });

            return result;
        }
    }
}
