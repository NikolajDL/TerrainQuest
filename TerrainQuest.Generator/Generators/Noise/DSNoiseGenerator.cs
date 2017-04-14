using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Algorithms;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Generators.Noise
{
    /// <summary>
    /// A generator used to create a <see cref="HeightMap"/> using diamond square noise.
    /// </summary>
    public class DSNoiseGenerator : BaseGenerator
    {
        /// <summary>
        /// Get the randomness factor used for the diamond square noise.
        /// </summary>
        public double RandomnessFactor { get; private set; }

        /// <summary>
        /// Get the seed of the noise generator
        /// </summary>
        public int? Seed { get; private set; }

        /// <summary>
        /// Create a white noise generator
        /// </summary>
        public DSNoiseGenerator(int height, int width, double? randomnessFactor = null, int? seed = null) 
            : base(height, width)
        {
            RandomnessFactor = randomnessFactor ?? 2d;
            Seed = seed;
        }

        /// <summary>
        /// Generate a white noise <see cref="HeightMap"/>
        /// </summary>
        public override HeightMap Generate()
        {
            var maxLength = Math.Max(Dimensions.Height, Dimensions.Width);

            var noise = new DiamondSquareNoise(Seed);

            var result = noise.Generate(maxLength, RandomnessFactor);

            var resizedResult = MathHelper.Copy(result, Dimensions.Height, Dimensions.Width);

            var map = new HeightMap(resizedResult);
            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public DSNoiseGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RandomnessFactor = info.GetDouble(nameof(RandomnessFactor));
            Seed = (int?)info.GetValue(nameof(Seed), typeof(int?));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(RandomnessFactor), RandomnessFactor);
            info.AddValue(nameof(Seed), Seed, typeof(int?));
        }

        #endregion
    }
}
