using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TerrainQuest.Editor.Rendering
{
    /// <summary>
    /// Render-loop callback function
    /// </summary>
    public delegate void ProcessFrameCallback();

    /// <summary>
    /// A windows forms control used to drive terrain rendering
    /// </summary>
    public class RenderingControl : UserControl
    {
        private Font _fontForDesignMode;
        private Thread _renderThread;
        private ProcessFrameCallback _processFrameCallback;

        public RenderingControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        protected override void Dispose(bool disposing)
        {
            // Close thread.
            _renderThread?.Abort();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Start running the rendering loop, with the given callback
        /// </summary>
        public void Run(ProcessFrameCallback processFrame)
        {
            // If renderthread is already running, no need to start a new
            if (_renderThread != null)
                return;

            _processFrameCallback = processFrame;

            _renderThread = new Thread(RunLoop);
            _renderThread.Start();
        }

        private void RunLoop()
        {
            // Loop while we have a frame
            while (NextFrame())
            {
                Invoke(_processFrameCallback);
                Invalidate();
            }
        }

        private bool NextFrame()
        {
            return !(this.Disposing || IsDisposed);
        }

        #region Handle Design mode

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (DesignMode)
                base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DesignMode)
            {
                if (_fontForDesignMode == null)
                    _fontForDesignMode = new Font("Calibri", 24, FontStyle.Regular);

                e.Graphics.Clear(System.Drawing.Color.WhiteSmoke);
                string text = "Terrain Generator Editor Renderer";
                var sizeText = e.Graphics.MeasureString(text, _fontForDesignMode);

                e.Graphics.DrawString(text, _fontForDesignMode, new SolidBrush(System.Drawing.Color.Black), (Width - sizeText.Width) / 2, (Height - sizeText.Height) / 2);
            }
        }

        #endregion

    }
}
