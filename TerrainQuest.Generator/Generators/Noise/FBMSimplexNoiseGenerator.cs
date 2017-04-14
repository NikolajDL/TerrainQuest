using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Algorithms;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Generators.Noise
{
    /// <summary>
    /// A noise generator class, that uses Fractional Brownian Motion 
    /// along with OpenSimplex noise to generate a noise <see cref="HeigthMap"/>.
    /// </summary>
    public class FBMSimplexNoiseGenerator : BaseGenerator
    {
        private OpenSimplexNoise generator;

        /// <summary>
        /// Get the number of iterations used for the Brownian Motion
        /// </summary>
        public int Iterations { get; private set; }

        /// <summary>
        /// Get the persistance factor by which the noise frequency 
        /// is multiplied at each iteration.
        /// </summary>
        public float Persistence { get; private set; }

        /// <summary>
        /// Get the scale factor of the OpenSimplex noise
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        /// Get the seed of the noise generator
        /// </summary>
        public long? Seed { get; private set; }

        public FBMSimplexNoiseGenerator(int height, int width, 
            int iterations, float persistence, float scale, long? seed = null) 
            : base(height, width)
        {
            Iterations = iterations;
            Persistence = persistence;
            Scale = scale;
            Seed = seed;
        }

        public override HeightMap Generate()
        {
            var map = new HeightMap(Dimensions);

            generator = new OpenSimplexNoise(Seed ?? DateTime.Now.Ticks);
            
            map.ForEach((r, c) => {
                map[r, c] = SumOctave(c, r);
            });

            return map;
        }

        private double SumOctave(int x, int y)
        {
            double noise = 0;
            double freq = Scale;
            double maxAmp = 0;
            double amp = 1;

            for(int i = 0; i < Iterations; i++)
            {
                noise += generator.Generate(x * freq, y * freq) * amp;
                maxAmp += amp;
                amp *= Persistence;
                freq *= 2;
            }

            noise /= maxAmp;

            noise = (noise * 0.5) + 0.5;

            return noise;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public FBMSimplexNoiseGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Iterations = info.GetInt32(nameof(Iterations));
            Persistence = info.GetSingle(nameof(Persistence));
            Scale = info.GetSingle(nameof(Scale));
            Seed = (long?)info.GetValue(nameof(Seed), typeof(long?));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Iterations), Iterations);
            info.AddValue(nameof(Persistence), Persistence);
            info.AddValue(nameof(Scale), Scale);
            info.AddValue(nameof(Seed), Seed, typeof(long?));
        }

        #endregion
    }
}
