using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An effect to change the brightness of a <see cref="HeightMap"/>
    /// </summary>
    public class BrightnessEffect : IEffect
    {
        /// <summary>
        /// Get the brightness being applied by this effect
        /// </summary>
        public double Brightness { get; private set; }

        /// <summary>
        /// Create a brightness effect
        /// </summary>
        public BrightnessEffect(double brightness)
        {
            Brightness = brightness;
        }

        /// <summary>
        /// Change the brightness of the given heightmap
        /// </summary>
        public HeightMap Calculate(HeightMap heightMap)
        {
            var result = heightMap.Clone();

            result.ForEach((r, c) => {
                result[r, c] = result[r, c] + Brightness;
            });

            return result;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public BrightnessEffect(SerializationInfo info, StreamingContext context)
        {
            Brightness = info.GetDouble(nameof(Brightness));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Brightness), Brightness);
        }

        #endregion
    }
}
