using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator
{
    /// <summary>
    /// A map used to represent some 2-dimensional piece of data, such as heightmaps
    /// </summary>
    public class Map<TData>
        where TData : struct
    {
        protected TData[,] _data;

        /// <summary>
        /// Get the height of the map
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Get the width of the map
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Get the data that makes up this map.
        /// </summary>
        public TData[,] Data
        {
            get { return _data; }
        }

        #region Constructors

        /// <summary>
        /// Create a square map of the given size
        /// </summary>
        public Map(int size)
            : this(size, size)
        { }

        /// <summary>
        /// Create a map of the given dimensions
        /// </summary>
        public Map(int height, int width)
        {
            Height = height;
            Width = width;

            _data = new TData[Height, Width];
        }

        #endregion

        #region Indexer and setter/getter

        /// <summary>
        /// Get or set the value for a given coordinate pair
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if coordinate is out of bounds</exception>
        public TData this[int row, int col]
        {
            get { return GetValue(row, col); }
            set { SetValue(row, col, value); }
        }

        /// <summary>
        /// Get the value for a given coordinate pair
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if coordinate is out of bounds</exception>
        public TData GetValue(int row, int col)
        {
            if (!CheckPositionIsValid(row, col))
                throw new ArgumentOutOfRangeException();

            return _data[row, col];
        }

        /// <summary>
        /// Get the value for a given coordinate pair
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if coordinate is out of bounds</exception>
        public virtual void SetValue(int row, int col, TData value)
        {
            if (!CheckPositionIsValid(row, col))
                throw new ArgumentOutOfRangeException();

            _data[row, col] = value;
        }

        /// <summary>
        /// Check whether the given coordinates is valid for this map
        /// </summary>
        public bool CheckPositionIsValid(int row, int col)
        {
            if (row < 0)
                return false;
            if (col < 0)
                return false;
            if (row > Height - 1)
                return false;
            if (col > Width - 1)
                return false;

            return true;
        }

        #endregion
    }
}
