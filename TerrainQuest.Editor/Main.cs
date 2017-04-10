using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using System.Windows.Forms;
using TerrainQuest.Graphics;

namespace TerrainQuest.Editor
{
    public partial class Main : Form
    {
        private GraphicsOptions _options;
        private GraphicsDevice _device;

        /// <summary>
        /// Get the graphics device.
        /// </summary>
        public GraphicsDevice Device { get { return _device; } }

        public Main()
        {
            InitializeComponent();

            _options = new GraphicsOptions();

            _device = new GraphicsDevice(renderingControl, _options);

            renderingControl.Run(ProcessFrame);
        }

        private void ProcessFrame()
        {
            Update();

            Render();
        }

        private void Update()
        {

        }

        private void Render()
        {
            // Do the drawing
            Device.ClearRenderAndDepthView(Color.Red);

            Device.Present();
        }
    }
}
