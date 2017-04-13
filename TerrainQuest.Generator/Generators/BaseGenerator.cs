using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TerrainQuest.Generator.Generators
{
    public abstract class BaseGenerator : IGenerator
    {
        private Size _dimensions;

        /// <summary>
        /// Get the dimensions of the generated <see cref="HeightMap"/>
        /// </summary>
        public Size Dimensions { get { return _dimensions; } }

        public BaseGenerator(int height, int width)
            : this(new Size(width, height)) { }

        public BaseGenerator(Size dimensions)
        {
            _dimensions = dimensions;
        }

        public abstract HeightMap Generate();
        
        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        protected BaseGenerator(SerializationInfo info, StreamingContext context)
        {
            _dimensions = (Size)info.GetValue(nameof(Dimensions), typeof(Size));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Dimensions), _dimensions);
        }

        #endregion
    }
}
