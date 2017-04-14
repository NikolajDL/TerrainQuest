using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// An overlay <see cref="IBlendMode"/> which is a combination of the
    /// <see cref="MultiplyBlend"/> mode and the <see cref="ScreenBlend"/> mode.
    /// </summary>
    public class OverlayBlend : IBlendMode
    {
        /// <summary>
        /// Perform the blending.
        /// </summary>
        public HeightMap Blend(HeightMap left, HeightMap right)
        {
            var result = left.Clone();

            result.ForEach((r, c) => {
                var a = result[r, c];
                var b = right.CheckPositionIsValid(r, c) ? right[r, c] : 0d;

                if (a < 0.5)
                    result[r, c] = 2 * a * b;
                else
                    result[r, c] = 1 - (2 * (1 - a) * (1 - b));
            });

            return result;
        }
    }
}
