using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Blending;
using TerrainQuest.Generator.Helpers;
using TerrainQuest.Generator.Serialization;

namespace TerrainQuest.Generator.Graph
{
    /// <summary>
    /// A node that blends the the <see cref="HeightMap"/>s of each dependency, 
    /// based on a given blending mode.
    /// The dependencies are blended in the order they are inserted as a dependency. 
    /// </summary>
    public class BlendingNode : HeightMapNode
    {

        private List<SerializedNode> _dependencies = new List<SerializedNode>();

        /// <summary>
        /// Get the blendmode being performed by this blendnode
        /// </summary>
        public IBlendMode BlendMode { get; private set; }

        /// <summary>
        /// Get the dimensions of the resulting heightmap
        /// </summary>
        public Size? Dimensions { get; private set; }

        /// <summary>
        /// Get all nodes getting blended.
        /// </summary>
        public override IEnumerable<INode> Dependencies
        {
            get
            {
                return _dependencies.Select(x => x.Node);
            }
        }

        /// <summary>
        /// Create a <see cref="BlendingNode"/> where the dimensions of the result, 
        /// equals the dimensions of the first dependency.
        /// </summary>
        public BlendingNode(IBlendMode blendMode)
        {
            Check.NotNull(blendMode, nameof(blendMode));

            BlendMode = blendMode;
        }

        /// <summary>
        /// Create a <see cref="BlendingNode"/> where the result will have the given dimensions
        /// </summary>
        public BlendingNode(int height, int width, IBlendMode blendMode)
        {
            Check.NotNull(blendMode, nameof(blendMode));

            Dimensions = new Size(width, height);
            BlendMode = blendMode;
        }

        /// <summary>
        /// Add an add dependency with a weight, 
        /// where the weight is a percentage of how much of the dependency is added.
        /// </summary>
        public void AddDependency(HeightMapNode dependency)
        {
            Check.NotNull(dependency, nameof(dependency));

            lock (_dependencies)
            {
                _dependencies.Add(new SerializedNode(dependency));
            }
        }

        protected override void Process()
        {
            lock (_dependencies)
            {
                if (!_dependencies.Any())
                    throw new InvalidOperationException("Cannot perform blending without any dependencies to blend.");

                var sources = Dependencies.Cast<HeightMapNode>();
                var result = sources.First().Result;

                if (Dimensions.HasValue)
                    result = new HeightMap(Dimensions.Value.Height, Dimensions.Value.Width, result.Data);

                foreach(var source in sources.Skip(1))
                {
                    result = BlendMode.Blend(result, source.Result);
                }

                Result = result;
            }
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public BlendingNode(SerializationInfo info, StreamingContext context)
        {
            Dimensions = (Size?)info.GetValue("Dimensions", typeof(Size?));
            _dependencies = (List<SerializedNode>)info.GetValue(nameof(Dependencies), typeof(List<SerializedNode>));
            BlendMode = info.GetTypedValue<IBlendMode>(nameof(BlendMode));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Dimensions", Dimensions, typeof(Size?));
            info.AddValue(nameof(Dependencies), _dependencies, _dependencies.GetType());
            info.AddTypedValue(nameof(BlendMode), BlendMode);
        }

        #endregion
    }
}
