using System;
using System.Drawing;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Algorithms;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Generators.Noise
{
    /// <summary>
    /// A noise generator class, using a 2-dimensional Simplex noise algorithm.
    /// </summary>
    public class SimplexNoiseGenerator : BaseGenerator
    {

        /// <summary>
        /// Get the seed of the noise generator
        /// </summary>
        public long? Seed { get; private set; }

        /// <summary>
        /// Get the scale of the simplex noise
        /// </summary>
        public SizeF Scale { get; private set; }

        /// <summary>
        /// Create a simplex noise generator
        /// </summary>
        public SimplexNoiseGenerator(int height, int width, long? seed = null)
            : this(height, width, new SizeF(1,1), seed)
        { }

        /// <summary>
        /// Create a simplex noise generator
        /// </summary>
        public SimplexNoiseGenerator(int height, int width, SizeF scale, long? seed = null) 
            : base(height, width)
        {
            Seed = seed;
            Scale = scale;
        }

        /// <summary>
        /// Generate a simplex noise <see cref="HeightMap"/>
        /// </summary>
        /// <returns></returns>
        public override HeightMap Generate()
        {
            var map = new HeightMap(Dimensions);

            var noise = new OpenSimplexNoise(Seed ?? DateTime.Now.Ticks);

            map.ForEach((r, c) => {
                map[r, c] = (noise.Generate(c * Scale.Width, r * Scale.Height) + 1) / 2;
            });
            
            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public SimplexNoiseGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Seed = (long?)info.GetValue(nameof(Seed), typeof(long?));
            Scale = (SizeF)info.GetValue(nameof(Scale), typeof(SizeF));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Seed), Seed, typeof(long?));
            info.AddValue(nameof(Scale), Scale, typeof(SizeF));
        }

        #endregion
    }
}
