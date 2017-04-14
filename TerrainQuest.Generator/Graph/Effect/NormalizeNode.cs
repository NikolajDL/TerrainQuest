using System.Collections.Generic;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Graph.Effect
{
    /// <summary>
    /// A node that takes a source node <see cref="HeightMap"/> 
    /// and returns it with it's values normalized.
    /// </summary>
    public class NormalizeNode : HeightMapNode
    {
        /// <summary>
        /// Get the source node being normalized
        /// </summary>
        public HeightMapNode Source { get; private set; }

        /// <summary>
        /// Get all nodes getting added in.
        /// </summary>
        public override IEnumerable<INode> Dependencies
        {
            get
            {
                yield return Source;
            }
        }

        /// <summary>
        /// Create a <see cref="HeightMap"/> normalizer node.
        /// </summary>
        /// <param name="source"><see cref="HeightMap"/> to normalize.</param>
        public NormalizeNode(HeightMapNode source)
        {
            Check.NotNull(source, nameof(source));

            Source = source;
        }
        
        protected override void Process()
        {
            Result = Source.Result.AsNormalized();
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public NormalizeNode(SerializationInfo info, StreamingContext context)
        {
            Source = info.GetTypedValue<HeightMapNode>(nameof(Source));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddTypedValue(nameof(Source), Source);
        }

        #endregion
    }
}
