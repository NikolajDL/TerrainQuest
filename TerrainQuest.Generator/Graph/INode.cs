using System.Collections.Generic;

namespace TerrainQuest.Generator.Graph
{

    /// <summary>
    /// Represents a node in the terrain generator graph structure
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Get a list of this nodes dependencies
        /// </summary>
        IEnumerable<INode> Dependencies { get; }

        /// <summary>
        /// Get whether this node is currently executing
        /// </summary>
        bool IsExecuting { get; }

        /// <summary>
        /// Get whether this node has finished executing
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Run any terrain processor associated with this node.
        /// Usually, any dependencies of this node is executed and awaited first.
        /// </summary>
        void Execute();
    }
}
