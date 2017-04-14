using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Algorithms;

namespace TerrainQuest.Generator.Generators.Noise
{
    /// <summary>
    /// A noise generator that generates a <see cref="HeightMap"/>
    /// with a Voronoi noise pattern.
    /// </summary>
    public class VoronoiNoiseGenerator : BaseGenerator
    {
        /// <summary>
        /// Get the number of points used to generate the voronoi noise
        /// </summary>
        public int PointCount { get; set; }

        /// <summary>
        /// Get the exponent of the voronoi noise
        /// </summary>
        public float Exponent { get; private set; }

        /// <summary>
        /// Get the seed of the noise generator
        /// </summary>
        public int? Seed { get; private set; }


        /// <summary>
        /// Create a Voronoi noise generator
        /// </summary>
        public VoronoiNoiseGenerator(int height, int width, int? pointCount = null, float exponent = 1f, int? seed = null) 
            : base(height, width)
        {
            PointCount = pointCount ?? Math.Max(height, width);
            Exponent = exponent;
            Seed = seed;
        }

        /// <summary>
        /// Generate the Voronoi noise pattern map
        /// </summary>
        /// <returns></returns>
        public override HeightMap Generate()
        {
            var generator = new VoronoiNoise(Seed);

            var noise = generator.Generate(Dimensions.Width, Dimensions.Height,
                PointCount, Exponent);

            var map = new HeightMap(noise);
            map.Normalize();
            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public VoronoiNoiseGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            PointCount = info.GetInt32(nameof(PointCount));
            Exponent = info.GetSingle(nameof(Exponent));
            Seed = (int?)info.GetValue(nameof(Seed), typeof(int?));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(PointCount), PointCount);
            info.AddValue(nameof(Exponent), Exponent);
            info.AddValue(nameof(Seed), Seed, typeof(int?));
        }

        #endregion
    }
}
