using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Generators;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Graph
{
    /// <summary>
    /// A node used for generating <see cref="HeightMap"/>s
    /// </summary>
    public class GeneratorNode : HeightMapNode
    {
        private IGenerator _generator;

        /// <summary>
        /// Get the generator used for processing this node
        /// </summary>
        public IGenerator Generator { get { return _generator; } }

        public GeneratorNode(IGenerator generator)
        {
            Check.NotNull(generator, nameof(generator));

            _generator = generator;
        }

        /// <summary>
        /// Process this node, generating a <see cref="HeightMap"/>
        /// </summary>
        protected override void Process()
        {
            Result = Generator.Generate();
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public GeneratorNode(SerializationInfo info, StreamingContext context)
        {
            _generator = info.GetTypedValue<IGenerator>(nameof(Generator));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddTypedValue(nameof(Generator), _generator);
        }

        #endregion
    }
}
