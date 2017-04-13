using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Helpers;

namespace TerrainQuest.Generator.Graph.Blending
{
    /// <summary>
    /// A node that mixes the <see cref="HeightMap"/>s of each dependency, 
    /// by a given weight, into the resulting <see cref="HeightMap"/>. 
    /// The dependencies are mixed in the order they are added. 
    /// </summary>
    public class MixNode : HeightMapNode
    {
        private readonly Size? _size;

        private readonly List<WeightedNode> _dependencies = new List<WeightedNode>();

        /// <summary>
        /// Get all nodes getting mixed in.
        /// </summary>
        public override IEnumerable<INode> Dependencies
        {
            get
            {
                return _dependencies.Select(x => x.Node);
            }
        }

        /// <summary>
        /// Create a MixNode where the dimensions of the result, 
        /// equals the dimensions of the first mix dependency.
        /// </summary>
        public MixNode()
        { }

        /// <summary>
        /// Create a MixNode where the result will have the given dimensions
        /// </summary>
        public MixNode(int height, int width)
        {
            _size = new Size(width, height);
        }

        /// <summary>
        /// Add a mix dependency with a weight, 
        /// where the weight is a percentage of how much of the dependency is added to the mix.
        /// </summary>
        public void AddDependency(HeightMapNode dependency, float mixWeight)
        {
            lock (_dependencies)
            {
                _dependencies.Add(new WeightedNode { Node = dependency, Weight = mixWeight });
            }
        }

        protected override void Process()
        {
            lock (_dependencies)
            {
                if (!_dependencies.Any())
                    throw new InvalidOperationException("Cannot execute MixNode without any dependencies to mix.");

                var map = CreateBaseHeightMap();
                foreach(var dependency in _dependencies)
                {
                    map = Mix(map, dependency.Node.Result, dependency.Weight);
                }

                Result = map;
            }
        }

        private HeightMap CreateBaseHeightMap()
        {
            if (_size.HasValue)
            {
                return new HeightMap(_size.Value.Height, _size.Value.Width);
            } else {
                var baseMap = _dependencies.First().Node.Result;
                return new HeightMap(baseMap.Height, baseMap.Width);
            }
        }

        private HeightMap Mix(HeightMap baseMap, HeightMap mixMap, float weight)
        {
            baseMap.ForEach((r, c) =>
            {
                if (mixMap.CheckPositionIsValid(r, c))
                    baseMap[r, c] = MathHelper.Clamp(baseMap[r, c] + mixMap[r, c] * weight, 0, 1);
            });

            return baseMap;
        }

        private class WeightedNode
        {
            public HeightMapNode Node { get; set; }
            public float Weight { get; set; }
        }
    }
}
