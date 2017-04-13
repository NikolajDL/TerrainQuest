using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Generators.Shape
{
    /// <summary>
    /// Generates a pyramid shape in the middle of a <see cref="HeightMap"/>
    /// </summary>
    public class PyramidGenerator : BaseGenerator
    {
        /// <summary>
        /// Get the size ratio of the pyramid shape
        /// </summary>
        public float Ratio { get; private set; }

        /// <summary>
        /// Get the height of the lowest point of the generated pyramid share
        /// </summary>
        public double MinHeight { get; private set; }

        /// <summary>
        /// Get the height of the highest point of the generated pyramid share
        /// </summary>
        public double MaxHeight { get; private set; }

        public PyramidGenerator(int height, int width, float pyramidRatio, double minHeight = 0d, double maxHeight = 1d) 
            : base(height, width)
        {
            Ratio = MathHelper.Clamp(pyramidRatio, 0f, 1f);
            MinHeight = MathHelper.Clamp(minHeight, 0d, 1d);
            MaxHeight = MathHelper.Clamp(maxHeight, 0d, 1d);
        }

        /// <summary>
        /// Generate a <see cref="HeightMap"/> with a pyramid shape in the center.
        /// </summary>
        public override HeightMap Generate()
        {
            var map = new HeightMap(Dimensions);

            var sizeY = (int)(map.Height * Ratio);
            var sizeX = (int)(map.Width * Ratio);
            var halfY = sizeY / 2;
            var halfX = sizeX / 2;
            
            var range = MaxHeight - MinHeight;

            var offsetY = (map.Height - sizeY) / 2;
            var offsetX = (map.Width - sizeX) / 2;

            for (int y = 0; y < sizeY; y++)
            {
                var r = offsetY + y;
                var heightY = MinHeight + ((halfY - Math.Abs(y - halfY)) / (double)halfY) * range;
                for (int x = 0; x < sizeX; x++)
                {
                    var c = offsetX + x;
                    var heightX = MinHeight + ((halfX - Math.Abs(x - halfX)) / (double)halfX) * range;
                    map[r, c] = Math.Min(heightY, heightX);
                }
            }
            
            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public PyramidGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Ratio = info.GetSingle(nameof(Ratio));
            MinHeight = info.GetDouble(nameof(MinHeight));
            MaxHeight = info.GetDouble(nameof(MaxHeight));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Ratio), Ratio);
            info.AddValue(nameof(MinHeight), MinHeight);
            info.AddValue(nameof(MaxHeight), MaxHeight);
        }

        #endregion
    }
}
