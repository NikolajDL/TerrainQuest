using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator
{
    /// <summary>
    /// A heightmap used for final or intermediate processing results
    /// </summary>
    public sealed class HeightMap : Map<double>
    {

        /// <summary>
        /// Get the largest height value in the heightmap
        /// </summary>
        public double MaxHeight { get; private set; } = double.MaxValue;

        /// <summary>
        /// Get the lowest height value in the heightmap
        /// </summary>
        public double MinHeight { get; private set; } = double.MinValue;

        #region Constructors

        /// <summary>
        /// Create a square heightmap of the given size
        /// </summary>
        public HeightMap(int size)
            : base(size) { }

        /// <summary>
        /// Create a heightmap of the given dimensions
        /// </summary>
        public HeightMap(int height, int width)
            : base(height, width) { }

        /// <summary>
        /// Create a copy of the passed heightmap
        /// </summary>
        public HeightMap(double[,] map)
            : base(map.GetLength(0), map.GetLength(1))
        {
            _data = (double[,])map.Clone();

            RecalculateMinMaxValue();
        }

        /// <summary>
        /// Clone this heightmap
        /// </summary>
        public HeightMap Clone()
        {
            return new HeightMap(Data);
        }

        #endregion

        #region Indexer and setter/getter

        /// <summary>
        /// Get the value for a given coordinate pair and update the min/max value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if coordinate is out of bounds</exception>
        public override void SetValue(int row, int col, double value)
        {
            base.SetValue(row, col, value);
            UpdateMinMaxValue(value);
        }

        private void RecalculateMinMaxValue()
        {
            this.ForEach((r, c) => UpdateMinMaxValue(_data[r, c]));
        }

        private void UpdateMinMaxValue(double value)
        {
            if (value > MaxHeight)
                MaxHeight = value;
            else if (value < MinHeight)
                MinHeight = value;
        }

        #endregion

        #region Manipulation

        /// <summary>
        /// Flatten the height of the heightmap to zero.
        /// </summary>
        public void Flatten()
        {
            FlattenTo(0);
        }

        /// <summary>
        /// Flatten the height of the heightmap to a given value between 0 and 1
        /// </summary>
        /// <param name="height"></param>
        public void FlattenTo(double height)
        {
            height = MathHelper.Clamp(height, 0d, 1d);
            this.ForEach((r, c) => Data[r, c] = height);
        }

        /// <summary>
        /// Normalize the entire heightmap to stay within the interval [0,1]
        /// </summary>
        public void Normalize()
        {
            var normalizationRatio = (MaxHeight - MinHeight);
            this.ForEach((r, c) =>
            {
                Data[r, c] = (Data[r, c] - MinHeight) / normalizationRatio;
            });
        }

        /// <summary>
        /// Return a normalized copy of this heightmap
        /// </summary>
        public HeightMap AsNormalized()
        {
            var clone = Clone();
            clone.Normalize();
            return clone;
        }

        #endregion

        #region Image helpers

        public Bitmap AsBitmap()
        {
            var bitmap = new Bitmap(Width, Height);

            this. ForEach((r, c) => bitmap.SetPixel(c, r, GetColor(r, c)));

            return bitmap;
        }

        public Color GetColor(int row, int col)
        {
            var height = Data[row, col];
            var color = MathHelper.Clamp((int)(height * 255), 0, 255);
            return Color.FromArgb(color, color, color);
        }

        #endregion
    }
}
