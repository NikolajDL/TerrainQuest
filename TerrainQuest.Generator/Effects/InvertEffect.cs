using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Effects
{
    /// <summary>
    /// An image effect to invert a <see cref="HeightMap"/>
    /// </summary>
    public class InvertEffect : IEffect
    {
        /// <summary>
        /// Create an invert effect
        /// </summary>
        public InvertEffect() { }

        /// <summary>
        /// Invert given heightmap
        /// </summary>
        public HeightMap Calculate(HeightMap heightMap)
        {
            var inverted = heightMap.Clone();

            inverted.ForEach((r, c) =>
            {
                inverted[r, c] = 1 - inverted[r, c];
            });

            return inverted;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public InvertEffect(SerializationInfo info, StreamingContext context)
        { }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        { }

        #endregion
    }
}
