using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An effect to change the intensity of a <see cref="HeightMap"/>.
    /// Intensity is calculated by multiplying a constant factor by each pixel.
    /// </summary>
    public class IntensityEffect : IEffect
    {
        /// <summary>
        /// Get the intensity being applied by this effect
        /// </summary>
        public double Intensity { get; private set; }

        /// <summary>
        /// Create a intensity effect
        /// </summary>
        public IntensityEffect(double intensity)
        {
            Intensity = intensity;
        }

        /// <summary>
        /// Change the intensity of the given heightmap
        /// </summary>
        public HeightMap Calculate(HeightMap heightMap)
        {
            var result = heightMap.Clone();

            result.ForEach((r, c) => {
                result[r, c] = result[r, c] * Intensity;
            });

            return result;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public IntensityEffect(SerializationInfo info, StreamingContext context)
        {
            Intensity = info.GetDouble(nameof(Intensity));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Intensity), Intensity);
        }

        #endregion
    }
}
