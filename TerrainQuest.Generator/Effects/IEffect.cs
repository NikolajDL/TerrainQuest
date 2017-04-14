using System.Runtime.Serialization;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An interface describing how to apply image effects to a <see cref="HeightMap"/>.
    /// </summary>
    public interface IEffect : ISerializable
    {
        /// <summary>
        /// Do the calculation for the given point and <see cref="HeightMap"/>
        /// </summary>
        HeightMap Calculate(HeightMap heightMap);
    }
}
