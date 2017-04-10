using System.Drawing;

namespace TerrainQuest.Graphics
{
    /// <summary>
    /// Options for the <see cref="GraphicsDevice"/>
    /// </summary>
    public class GraphicsOptions
    {
        public Size Resolution { get; } = new Size(800, 600);

        public int Multisamples { get; }

        public bool UseMultisampling { get; }

        public GraphicsOptions(Size? resolution = null, bool useMultisampling = true, int multisamples = 4)
        {
            if (resolution != null)
                Resolution = resolution.Value;
            UseMultisampling = useMultisampling;
            Multisamples = Multisamples;
        }
    }
}
