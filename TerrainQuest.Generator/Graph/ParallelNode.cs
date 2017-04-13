using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TerrainQuest.Generator.Graph
{

    /// <summary>
    /// An abstract node that executes it's dependencies in parallel 
    /// before running it's own processor.
    /// </summary>
    public abstract class ParallelNode : INode
    {
        /// <summary>
        /// Get whether this node is currently executing
        /// </summary>
        public bool IsExecuting { get; private set; }

        /// <summary>
        /// Get whether this nodes dependencies are done executing
        /// </summary>
        public bool IsDependenciesDone { get; private set; }

        /// <summary>
        /// Get whether this node is done executing
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable<INode> Dependencies
        {
            get { yield break; }
        }

        /// <summary>
        /// The terrain generation processor to execute once this nodes dependencies are done
        /// </summary>
        protected abstract void Process();

        /// <summary>
        /// Execute this node by first calling execute on all dependencies and once they finish, 
        /// execute the Process method of this node
        /// </summary>
        public void Execute()
        {
            if (IsDone)
                return;

            IsExecuting = true;

            Parallel.ForEach(Dependencies, node => node.Execute());
            IsDependenciesDone = true;

            Process();

            IsExecuting = false;
            IsDone = true;
        }

        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}
