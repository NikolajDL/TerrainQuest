using System.Collections.Generic;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Graph.Blending
{

    /// <summary>
    /// A node to mask a source <see cref="HeightMap"/> by a given mask <see cref="HeightMap"/>
    /// </summary>
    public class MaskNode : HeightMapNode
    {
        /// <summary>
        /// Get the source <see cref="HeightMap"/> which is being masked.
        /// </summary>
        public HeightMapNode Source { get; private set; }

        /// <summary>
        /// Get the masking <see cref="HeightMap"/>
        /// </summary>
        public HeightMapNode Mask { get; private set; }

        /// <summary>
        /// Get the masking dependencies
        /// </summary>
        public override IEnumerable<INode> Dependencies
        {
            get
            {
                yield return Source;
                yield return Mask;
            }
        }

        /// <summary>
        /// Create a MaskNode where the resulting <see cref="HeightMap"/> will be the same size as the given source.
        /// </summary>
        public MaskNode(HeightMapNode source, HeightMapNode mask)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(mask, nameof(mask));

            Source = source;
            Mask = mask;
        }

        protected override void Process()
        {
            var baseMap = Source.Result.Clone();
            var maskMap = Mask.Result;

            baseMap.ForEach((r, c) => {
                baseMap[r, c] = MathHelper.Clamp(maskMap.CheckPositionIsValid(r, c) ? baseMap[r, c] * maskMap[r, c] : 0d, 0d, 1d);
            });

            Result = baseMap;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public MaskNode(SerializationInfo info, StreamingContext context)
        {
            Source = (HeightMapNode)info.GetValue(nameof(Source), typeof(HeightMapNode));
            Mask = (HeightMapNode)info.GetValue(nameof(Mask), typeof(HeightMapNode));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Source), Source, Source.GetType());
            info.AddValue(nameof(Mask), Mask, Mask.GetType());
        }

        #endregion
    }
}
