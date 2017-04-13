using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace TerrainQuest.Generator.Generators
{
    /// <summary>
    /// A generator that generates a flat heightmap of a given height.
    /// </summary>
    public class FlatGenerator : IGenerator
    {
        private double _height;
        private int _dimensionHeight;
        private int _dimensionWidth;

        /// <summary>
        /// Get the height dimension of the generated <see cref="HeightMap"/>
        /// </summary>
        public int DimensionHeight { get { return _dimensionHeight; } }

        /// <summary>
        /// Get the width dimension of the generated <see cref="HeightMap"/>
        /// </summary>
        public int DimensionWidth { get { return _dimensionWidth; } }

        /// <summary>
        /// Get the height of the generated <see cref="HeightMap"/>
        /// </summary>
        public double Height { get { return _height; } }

        /// <summary>
        /// Create a generator, that generates a flat <see cref="HeightMap"/> of a given constant height
        /// </summary>
        public FlatGenerator(int height, int width, double heightValue = 0d)
        {
            _dimensionHeight = height;
            _dimensionWidth = width;
            _height = heightValue;
        }

        /// <summary>
        /// Generate a flat heightmap.
        /// </summary>
        public HeightMap Generate()
        {
            var map = new HeightMap(DimensionHeight, DimensionWidth);
            map.FlattenTo(Height);
            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public FlatGenerator(SerializationInfo info, StreamingContext context)
        {
            _dimensionHeight = info.GetInt32(nameof(DimensionHeight));
            _dimensionWidth = info.GetInt32(nameof(DimensionWidth));
            _height = info.GetDouble(nameof(Height));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(DimensionHeight), DimensionHeight);
            info.AddValue(nameof(DimensionWidth), DimensionWidth);
            info.AddValue(nameof(Height), Height);
        }

        #endregion
    }
}
