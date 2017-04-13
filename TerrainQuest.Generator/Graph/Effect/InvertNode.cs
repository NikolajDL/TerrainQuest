using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Graph.Effect
{
    /// <summary>
    /// A node that takes the source <see cref="HeightMap"/>, 
    /// inverts it and passes the inverted map as a result.
    /// </summary>
    public class InvertNode : HeightMapNode
    {
        /// <summary>
        /// Get the source node being inverted
        /// </summary>
        public HeightMapNode Source { get; private set; }

        /// <summary>
        /// Create a <see cref="HeightMap"/> inverter node.
        /// </summary>
        /// <param name="source"><see cref="HeightMap"/> to invert.</param>
        public InvertNode(HeightMapNode source)
        {
            Source = source;
        }

        protected override void Process()
        {
            var inverted = Source.Result.Clone();

            inverted.ForEach((r, c) =>
            {
                inverted[r, c] = 1 - inverted[r, c];
            });

            Result = inverted;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public InvertNode(SerializationInfo info, StreamingContext context)
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
