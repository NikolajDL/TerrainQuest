using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An effect to correct the gamma of a <see cref="HeightMap"/>
    /// </summary>
    public class GammaEffect : IEffect
    {
        private const double High = 259d;
        private const double Low = 255d;

        /// <summary>
        /// Get the gamma being applied by this effect
        /// </summary>
        public double Gamma { get; private set; }

        /// <summary>
        /// Create a gamma effect
        /// </summary>
        public GammaEffect(double gamma)
        {
            if (gamma <= 0d)
                throw new ArgumentOutOfRangeException("Gamma must be a value above zero");

            Gamma = gamma;
        }

        /// <summary>
        /// Change the gamma of the given heightmap
        /// </summary>
        public HeightMap Calculate(HeightMap heightMap)
        {
            var result = heightMap.Clone();

            double gammaCorrection = 1 / Gamma;

            result.ForEach((r, c) => {
                result[r, c] = Math.Pow(result[r, c], gammaCorrection);
            });

            return result;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public GammaEffect(SerializationInfo info, StreamingContext context)
        {
            Gamma = info.GetDouble(nameof(Gamma));
            if (Gamma <= 0d)
                throw new ArgumentOutOfRangeException("Gamma must be a value above zero");
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Gamma), Gamma);
        }

        #endregion
    }
}
