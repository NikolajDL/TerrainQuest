using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainQuest.Generator.Graph
{
    /// <summary>
    /// A node that that has a <see cref="HeightMap"/> as it's processing result.
    /// </summary>
    public abstract class HeightMapNode : ParallelNode
    {
        public HeightMap Result { get; set; }
    }
}
