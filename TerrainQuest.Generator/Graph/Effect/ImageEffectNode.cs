using System.Collections.Generic;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Effects;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Graph.Effect
{
    /// <summary>
    /// A node that takes the source <see cref="HeightMap"/>, 
    /// and applies an image effect, such as brightness or contrast.
    /// </summary>
    public class ImageEffectNode : HeightMapNode
    {
        private List<IEffect> _effects = new List<IEffect>();

        /// <summary>
        /// Get the source node being inverted
        /// </summary>
        public HeightMapNode Source { get; private set; }

        /// <summary>
        /// Get the image effects to be applied to the Source <see cref="HeightMap"/>
        /// </summary>
        public IEnumerable<IEffect> Effects {
            get {
                return _effects;
            }
        }

        /// <summary>
        /// Get all nodes getting added in.
        /// </summary>
        public override IEnumerable<INode> Dependencies
        {
            get
            {
                yield return Source;
            }
        }

        /// <summary>
        /// Create an image effect node to apply a collection of image effect to the given Source map.
        /// Each effect is applied in the order it is added to the node.
        /// </summary>
        public ImageEffectNode(HeightMapNode source, params IEffect[] effects)
        {
            Check.NotNull(source, nameof(source));

            Source = source;
            _effects.AddRange(effects);
        }

        /// <summary>
        /// Add an effect to the list of effects associated with this node.
        /// </summary>
        /// <param name="effect"></param>
        public void AddEffect(IEffect effect)
        {
            Check.NotNull(effect, nameof(effect));

            _effects.Add(effect);
        }

        protected override void Process()
        {
            var result = Source.Result;

            foreach (var effect in Effects)
            {
                result = effect.Calculate(result);
            }
            Result = result;
        }

        #region Serialization

        /// <summary>
        /// Object deserialization constructor
        /// </summary>
        public ImageEffectNode(SerializationInfo info, StreamingContext context)
        {
            Source = info.GetTypedValue<HeightMapNode>(nameof(Source));
            _effects = info.GetTypedValue<List<IEffect>>(nameof(Effects));
        }

        /// <summary>
        /// Object serialization method
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddTypedValue(nameof(Source), Source);
            info.AddTypedValue(nameof(Effects), _effects);
        }

        #endregion
    }
}
