using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Generators;

namespace TerrainQuest.Generator.Graph
{
    public class GeneratorNode : HeightMapNode
    {
        public IGenerator Generator { get; set; }

        public GeneratorNode(IGenerator generator)
        {
            Check.NotNull(generator, nameof(generator));

            Generator = generator;
        }

        protected override void Process()
        {
            Result = Generator.Generate();
        }
    }
}
