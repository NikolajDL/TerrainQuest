using System;
using System.Collections.Generic;
using System.Linq;

namespace TerrainQuest.Generator.Algorithms
{
    /// <summary>
    /// An algorithm the produces Voronoi noise
    /// </summary>
    public class VoronoiNoise
    {
        private Random rand;

        /// <summary>
        /// Construct a seeded Voronoi noise algorithm.
        /// </summary>
        /// <param name="seed"></param>
        public VoronoiNoise(int? seed = null)
        {
            rand = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        /// <summary>
        /// Generate a map with voronoi noise of the given width and height, and with the given number of points/cells. 
        /// The exponent is used with the distance function, to control how strong the noise is. 
        /// </summary>
        public double[,] Generate(int width, int height, int numberOfPoints, float exponent)
        {
            var points = GeneratePoints(width, height, numberOfPoints);

            double[,] map = new double[height, width];

            for(int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    var point = points.FirstOrDefault(p => p.Y == y && p.X == x);

                    double distance = Math.Pow(Math.Sqrt(MinDistance(new VoronoiPoint(x,y), points)), exponent);

                    map[y, x] = distance;
                }

            return map;
        }

        private int MinDistance(VoronoiPoint basePoint, VoronoiPoint[] points)
        {
            int min = int.MaxValue;
            int current = 0;

            foreach(var point in points)
            {
                current = DistanceSquared(basePoint, point);
                if (current < min)
                    min = current;
            }

            return min;
        }

        private int DistanceSquared(VoronoiPoint a, VoronoiPoint b)
        {
            var x = b.X - a.X;
            var y = b.Y - a.Y;
            return x * x + y * y;
        }

        private VoronoiPoint[] GeneratePoints(int width, int height, int numberOfPoints)
        {
            var points = new List<VoronoiPoint>(numberOfPoints);

            for(int i = 0; i < numberOfPoints; i++)
            {
                int x = RandomValue(width);
                int y = RandomValue(height);
                points.Add(new VoronoiPoint(x, y));
            }

            points.Sort();

            return points.Distinct().ToArray();
        }

        private int RandomValue(int max)
        {
            return rand.Next(max);
        }

        private class VoronoiPoint : IComparable<VoronoiPoint>
        {
            public int X { get; }

            public int Y { get; }

            public VoronoiPoint(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int CompareTo(VoronoiPoint other)
            {
                if(X < other.X)
                {
                    return 1;
                }else if(other.X < X)
                {
                    return -1;
                } else
                {
                    return Y < other.Y 
                        ? 1 
                        : Y > other.Y ? -1 : 0;
                }
            }
        }
    }
}
