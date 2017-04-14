using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An effect to change the contrast of a <see cref="HeightMap"/>
    /// </summary>
    public class ContrastEffect : IEffect
    {
        private const double High = 259d;
        private const double Low = 255d;

        /// <summary>
        /// Get the contrast being applied by this effect
        /// </summary>
        public double Contrast { get; private set; }

        /// <summary>
        /// Create a contrast effect
        /// </summary>
        public ContrastEffect(double contrast)
        {
            Contrast = contrast;
        }

        /// <summary>
        /// Change the contrast of the given heightmap
        /// </summary>
        public HeightMap Calculate(HeightMap heightMap)
        {
            var result = heightMap.Clone();

            double F = (High * (Contrast + Low)) / (Low * (High - Contrast));

            result.ForEach((r, c) => {
                result[r, c] = F * (result[r, c] - 0.5d) + 0.5d;
            });

            return result;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public ContrastEffect(SerializationInfo info, StreamingContext context)
        {
            Contrast = info.GetDouble(nameof(Contrast));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Contrast), Contrast);
        }

        #endregion
    }
}
