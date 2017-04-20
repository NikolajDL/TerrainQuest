namespace TerrainQuest.Generator.Graph
{
    /// <summary>
    /// A node that that has a <see cref="HeightMap"/> as it's processing result
    /// </summary>
    public abstract class HeightMapNode : ParallelNode
    {
        /// <summary>
        /// Get or set the <see cref="HeightMap"/> result of the node processing
        /// </summary>
        public virtual HeightMap Result { get; protected set; }
    }
}
