namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// An interface that describes how to apply a blend mode
    /// </summary>
    public interface IBlendMode
    {
        /// <summary>
        /// Blend the two given heightmaps
        /// </summary>
        HeightMap Blend(HeightMap left, HeightMap right);
    }
}
