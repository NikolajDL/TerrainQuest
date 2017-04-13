using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Graph;

namespace TerrainQuest.Generator.Serialization
{
    /// <summary>
    /// A serializable class representing the root of a terrain generation graph structure.
    /// </summary>
    public class GraphRoot : ISerializable
    {
        private INode _node;
        private Type _type;

        /// <summary>
        /// Get the root node of the terrain generation grapg
        /// </summary>
        public INode Node { get { return _node; } }

        public Type NodeType { get { return _type; } }

        /// <summary>
        /// Create a graph root node
        /// </summary>
        /// <param name="node"></param>
        public GraphRoot(INode node)
        {
            _node = node;
            _type = node.GetType();
        }
        
        /// <summary>
        /// Execute the <see cref="INode"/> associated with this <see cref="GraphRoot"/>
        /// </summary>
        public void Execute()
        {
            Node.Execute();
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public GraphRoot(SerializationInfo info, StreamingContext context)
        {
            _type = (Type)info.GetValue(nameof(NodeType), typeof(Type));
            _node = (INode)info.GetValue(nameof(Node), _type);
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(NodeType), _type, typeof(Type));
            info.AddValue(nameof(Node), _node, _type);
        }

        #endregion
    }
}
