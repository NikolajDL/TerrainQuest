using System.Runtime.Serialization;

namespace TerrainQuest.Generator.Generators
{
    /// <summary>
    /// A generator that generates a flat heightmap of a given height.
    /// </summary>
    public class FlatGenerator : BaseGenerator
    {
        /// <summary>
        /// Get the height of the generated <see cref="HeightMap"/>
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Create a generator, that generates a flat <see cref="HeightMap"/> of a given constant height
        /// </summary>
        public FlatGenerator(int height, int width, double heightValue = 0d)
            : base(height, width)
        {
            Height = heightValue;
        }

        /// <summary>
        /// Generate a flat heightmap.
        /// </summary>
        public override HeightMap Generate()
        {
            var map = new HeightMap(Dimensions);
            map.FlattenTo(Height);
            return map;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public FlatGenerator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Height = info.GetDouble(nameof(Height));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Height), Height);
        }

        #endregion
    }
}
