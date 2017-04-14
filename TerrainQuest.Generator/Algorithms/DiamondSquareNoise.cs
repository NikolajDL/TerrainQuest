using System;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Algorithms
{
    /// <summary>
    /// A class for generating diamond square noise.
    /// </summary>
    public class DiamondSquareNoise
    {
        private int? _seed;

        public DiamondSquareNoise(int? seed = null)
        {
            _seed = seed;
        }

        /// <summary>
        /// Generate a 2-dimensional diamond square noise texture
        /// </summary>
        public double[,] Generate(int length, double h)
        {
            var r = _seed.HasValue ? new Random(_seed.Value) : new Random();
            var cornerSeed = new[] { r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble() };

            return Generate(length, h, cornerSeed);
        }

        /// <summary>
        /// Generate a 2-dimensional diamond square noise texture,
        /// seeded by an array consisitng of at least 4 corner values.
        /// </summary>
        public double[,] Generate(int length, double h, params double[] cornerSeed)
        {
            var mapLength = length;
            // Diamond Square algorithm only works for lengths that are powers of 2 plus 1. 
            // So if we get the length '101' we want to get '129' (128+1)
            if (!MathHelper.IsPowerOfTwo(length - 1))
                mapLength = MathHelper.NextPowerOfTwo(length - 1) + 1;

            var map = GenerateCornerSeededArrayForMap(mapLength, cornerSeed);

            var generatedMap = GenerateFromSeed(map, h);


            return MathHelper.Copy(generatedMap, length, length);
        }

        /// <summary>
        /// Generate a 2-dimensional diamond square noise texture
        /// seeded by a given 2-dimensional seed. 
        /// The seed will also determine the size of the generated map.
        /// </summary>
        public double[,] Generate(double[,] seed, int seedLevel, double h)
        {
            var mapLength = seed.GetLength(0);
            if (seed.GetLength(0) != seed.GetLength(1))
                throw new ArgumentException("Diamond Square Noise only works for square seeds.");

            if (!MathHelper.IsPowerOfTwo(mapLength - 1))
                mapLength = MathHelper.NextPowerOfTwo(mapLength - 1) + 1;

            var generatedMap = MathHelper.Copy(seed, mapLength, mapLength);
            return GenerateFromSeed(generatedMap, h, seedLevel);
        }

        private double[,] GenerateFromSeed(double[,] map, double h, int skip = 0)
        {
            var width = map.GetLength(0) - 1;
            var r = _seed.HasValue ? new Random(_seed.Value) : new Random();

            int i = 0;

            for (int sideLength = width; sideLength > 1; sideLength /= 2, h /= 2)
            {
                // The more we skip, we more of the seed are we keeping
                if (skip > i++)
                    continue;

                int halfSide = sideLength / 2;

                // Diamond step
                double avg;
                for (int x = 0; x < width; x += sideLength)
                {
                    for (int y = 0; y < width; y += sideLength)
                    {
                        avg = map[x, y] + map[x + sideLength, y] + map[x, y + sideLength] +
                                     map[x + sideLength, y + sideLength];
                        avg /= 4;

                        map[x + halfSide, y + halfSide] = MathHelper.Clamp(avg + RandRange(r, h), -1, 1);
                    }
                }

                // Square step
                for (int x = 0; x < width; x += halfSide)
                {
                    for (int y = (x + halfSide) % sideLength; y < width; y += sideLength)
                    {
                        avg =
                            map[(x - halfSide + width) % (width), y] +
                            map[(x + halfSide) % (width), y] +
                            map[x, (y + halfSide) % (width)] +
                            map[x, (y - halfSide + width) % (width)];
                        avg /= 4;
                        avg = MathHelper.Clamp(avg + RandRange(r, h), -1, 1);

                        map[x, y] = avg;

                        if (x == 0) map[width, y] = MathHelper.Clamp(avg, -1, 1);
                        if (y == 0) map[x, width] = MathHelper.Clamp(avg, -1, 1);
                    }
                }

            }

            return map;
        }

        private static double[,] GenerateCornerSeededArrayForMap(int length, params double[] cornerSeed)
        {
            var map = new double[length, length];

            var seedLength = cornerSeed.Length;
            map[0, 0] = cornerSeed[0 % seedLength];
            map[0, length - 1] = cornerSeed[1 % seedLength];
            map[length - 1, 0] = cornerSeed[2 % seedLength];
            map[length - 1, length - 1] = cornerSeed[3 % seedLength];

            return map;
        }

        private static double RandRange(Random r, double h)
        {
            return (r.NextDouble() * 2 * h) - h;
        }
    }
}
