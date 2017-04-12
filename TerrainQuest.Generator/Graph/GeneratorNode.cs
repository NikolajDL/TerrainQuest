using TerrainQuest.Generator.Generators;

namespace TerrainQuest.Generator.Graph
{
    /// <summary>
    /// A node used for generating <see cref="HeightMap"/>s
    /// </summary>
    public class GeneratorNode : HeightMapNode
    {
        /// <summary>
        /// Get the generator used for processing this node
        /// </summary>
        public IGenerator Generator { get; }

        public GeneratorNode(IGenerator generator)
        {
            Check.NotNull(generator, nameof(generator));

            Generator = generator;
        }

        /// <summary>
        /// Process this node, generating a <see cref="HeightMap"/>
        /// </summary>
        protected override void Process()
        {
            Result = Generator.Generate();
        }
    }
}
