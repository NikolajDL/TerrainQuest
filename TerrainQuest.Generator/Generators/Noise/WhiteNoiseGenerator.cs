using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Generators.Noise
{
    /// <summary>
    /// A generator used to create a <see cref="HeightMap"/> containing only white noise.
    /// </summary>
    public class WhiteNoiseGenerator : BaseGenerator
    {

        /// <summary>
        /// Get the seed of the noise generator
        /// </summary>
        public int? Seed { get; private set; }

        /// <summary>
        /// Create a white noise generator
        /// </summary>
        public WhiteNoiseGenerator(int height, int width, int? seed = null) 
            : base(height, width)
        {
            Seed = seed;
        }

        /// <summary>
        /// Generate a white noise <see cref="HeightMap"/>
        /// </summary>
        public override HeightMap Generate()
        {
            var map = new HeightMap(Dimensions);

            var rand = Seed.HasValue ? new Random(Seed.Value) : new Random();

            map.ForEach((r, c) => {
                map[r, c] = rand.NextDouble();
            });

            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public WhiteNoiseGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Seed = (int?)info.GetValue(nameof(Seed), typeof(int?));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Seed), Seed, typeof(int?));
        }

        #endregion
    }
}
