using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An effect use to blur a <see cref="HeightMap"/> using gaussian blur.
    /// </summary>
    public class GaussianBlurEffect : IEffect
    {

        /// <summary>
        /// Get the deviation of the gaussian function used when <see cref="HeightMap"/> is blurred.
        /// </summary>
        public double Deviation { get; private set; }

        /// <summary>
        /// Get the radius of the kernel used for the gaussian blur effect. 
        /// </summary>
        public int? Radius { get; private set; }

        /// <summary>
        /// Create a gaussian blur image effect
        /// </summary>
        /// <param name="deviation">Deviation of the blur effect</param>
        /// <param name="radius">Radius of the convolution kernel used to blur the image. 
        /// If the radius is null, it'll be calculated from the given deviation</param>
        public GaussianBlurEffect(double deviation, int? radius = null)
        {
            Deviation = deviation;
            Radius = radius;
        }

        /// <summary>
        /// Blur the given heightmap
        /// </summary>
        public HeightMap Calculate(HeightMap heightMap)
        {
            return CalculateTwoPassGaussianConvolution(heightMap);
        }

        private HeightMap CalculateTwoPassGaussianConvolution(HeightMap heightMap)
        {
            var kernel = CalculateNormalizedKernel();

            var resultX = CalculateGaussianConvolution(heightMap, kernel, Direction.Horizontal);
            var resultY = CalculateGaussianConvolution(resultX, kernel, Direction.Vertical);

            return resultY;
        }

        private HeightMap CalculateGaussianConvolution(HeightMap source, double[,] kernel,
            Direction direction)
        {
            var result = new HeightMap(source.Height, source.Width);
            source.ForEach((r, c) => {
                result[r, c] = CalculatePoint(source, r, c, kernel, direction);
            });
            return result;
        }

        private double[,] CalculateNormalizedKernel()
        {
            int radius = Radius.HasValue ? Radius.Value
                            : (int)Math.Ceiling( Deviation * 3 ) * 2 + 1;
            var kernel = new double[radius, 1];
            int half = radius / 2;
            double sum = 0d;
            for (var i = 0; i < radius; i++)
            {
                kernel[i, 0] = 1 / (Math.Sqrt(2 * Math.PI) * Deviation) 
                    * Math.Exp(-(i - half) * (i - half) / (2 * Deviation * Deviation));
                sum += kernel[i, 0];
            }
            for (var i = 0; i < radius; i++)
            {
                kernel[i, 0] /= sum;
            }

            return kernel;
        }

        private double CalculatePoint(HeightMap source, int row, int column, 
            double[,] kernel, Direction direction)
        {
            double result = 0d;
            int half = kernel.GetLength(0) / 2;

            for(var i = 0; i < kernel.GetLength(0); i++)
            {
                int coX = direction == Direction.Horizontal ? column + i - half : column;
                int coY = direction == Direction.Vertical ? row + i - half : row;
                coX = MathHelper.Clamp(coX, 0, source.Width-1);
                coY = MathHelper.Clamp(coY, 0, source.Height-1);

                result += source[coY, coX] * kernel[i, 0];
            }

            return result;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public GaussianBlurEffect(SerializationInfo info, StreamingContext context)
        {
            Deviation = info.GetDouble(nameof(Deviation));
            Radius = (int?)info.GetValue(nameof(Radius), typeof(int?));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Deviation), Deviation);
            info.AddValue(nameof(Radius), Radius, typeof(int?));
        }

        #endregion

        private enum Direction
        {
            Vertical,
            Horizontal
        }
    }
}
