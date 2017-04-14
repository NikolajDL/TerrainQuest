using System.Drawing;
using System.Runtime.Serialization;

namespace TerrainQuest.Generator.Generators
{
    /// <summary>
    /// An abstract BaseGenerator adding a <see cref="Size"/> property
    /// to every generator inheriting from it, so it doesn't have to
    /// deal with the dimensions and serializing them each time.
    /// </summary>
    public abstract class BaseGenerator : IGenerator
    {

        /// <summary>
        /// Get the dimensions of the generated <see cref="HeightMap"/>
        /// </summary>
        public Size Dimensions { get; private set; }

        public BaseGenerator(int height, int width)
            : this(new Size(width, height)) { }

        public BaseGenerator(Size dimensions)
        {
            Dimensions = dimensions;
        }

        public abstract HeightMap Generate();
        
        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        protected BaseGenerator(SerializationInfo info, StreamingContext context)
        {
            Dimensions = (Size)info.GetValue(nameof(Dimensions), typeof(Size));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Dimensions), Dimensions, typeof(Size));
        }

        #endregion
    }
}
