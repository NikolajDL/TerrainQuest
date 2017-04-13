using System;
using System.Drawing;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Generators.Shape
{
    /// <summary>
    /// A generator that generates a square shape in the <see cref="HeightMap"/> 
    /// of a given height, with any size and offset by a configurable distance. 
    /// The square can also be created as a center square with some radius ratio.
    /// </summary>
    public class SquareGenerator : BaseGenerator
    {
        /// <summary>
        /// Get the size of the generated square
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Get the offset of the generated square. 
        /// The offset is calculated from the upper left corner.
        /// </summary>
        public Point Offset { get; private set; }

        /// <summary>
        /// Get the height of the generated shape on the <see cref="HeightMap"/>
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Generate a square shape generator
        /// </summary>
        public SquareGenerator(int height, int width, Size squareSize, Point offset, double mapHeight = 1d) 
            : base(height, width)
        {
            Size = squareSize;
            Offset = offset;
            Height = mapHeight;
        }

        /// <summary>
        /// Generate a square shape generator
        /// </summary>
        public SquareGenerator(int height, int width, Size squareSize, double mapHeight = 1d)
            : base(height, width)
        {
            Size = squareSize;
            Offset = new Point(0, 0);
            Height = mapHeight;
        }

        /// <summary>
        /// Generate a square shape generator from the center and out,
        /// with the size decided by the given squareRatio between 0 and 1.
        /// </summary>
        public SquareGenerator(int height, int width, float squareRatio, double mapHeight = 1d)
            : base(height, width)
        {
            squareRatio = MathHelper.Clamp(squareRatio, 0f, 1f);
            Size = new Size((int)(width * squareRatio), (int)(height * squareRatio));

            var restY = height - Size.Height;
            var restX = width - Size.Width;
            Offset = new Point(restY / 2, restX / 2);
            Height = mapHeight;
        }

        /// <summary>
        /// Generate a heightmap with a square.
        /// </summary>
        /// <returns></returns>
        public override HeightMap Generate()
        {
            var map = new HeightMap(Dimensions);

            var yMax = Math.Min(map.Height, Offset.Y + Size.Height);
            var xMax = Math.Min(map.Width, Offset.X + Size.Width);
            for(int y = Offset.Y; y < yMax; y++)
                for(int x = Offset.X; x < xMax; x++)
                {
                    if (map.CheckPositionIsValid(y, x))
                        map[y, x] = MathHelper.Clamp(Height, 0d, 1d);
                }

            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public SquareGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Size = (Size)info.GetValue(nameof(Size), typeof(Size));
            Offset = (Point)info.GetValue(nameof(Offset), typeof(Point));
            Height = info.GetDouble(nameof(Height));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Size), Size);
            info.AddValue(nameof(Offset), Offset);
            info.AddValue(nameof(Height), Height);
        }

        #endregion
    }
}
