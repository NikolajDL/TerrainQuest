using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Graph;

namespace TerrainQuest.Generator.Serialization
{
    /// <summary>
    /// A serializable class representing a serialized node of a terrain generation graph structure.
    /// This is to serialize the runtime type of the node, which is then used when deserializing the node.
    /// </summary>
    public class SerializedNode : SerializedNode<HeightMapNode>
    {
        public SerializedNode(HeightMapNode node) 
            : base(node) { }

        public SerializedNode(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }
    }

    /// <summary>
    /// A serializable class representing a serialized node of a terrain generation graph structure.
    /// This is to serialize the runtime type of the node, which is then used when deserializing the node.
    /// </summary>
    public class SerializedNode<T> : ISerializable
        where T : INode
    {
        private T _node;
        private Type _type;

        /// <summary>
        /// Get the node associated with this serialized node
        /// </summary>
        public T Node { get { return _node; } }

        public Type NodeType { get { return _type; } }

        /// <summary>
        /// Create a serialized node
        /// </summary>
        /// <param name="node"></param>
        public SerializedNode(T node)
        {
            _node = node;
            _type = node.GetType();
        }
        
        /// <summary>
        /// Execute the <see cref="T"/> associated with this <see cref="SerializedNode"/>
        /// </summary>
        public void Execute()
        {
            Node.Execute();
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public SerializedNode(SerializationInfo info, StreamingContext context)
        {
            _type = (Type)info.GetValue(nameof(NodeType), typeof(Type));
            _node = (T)info.GetValue(nameof(Node), _type);
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
