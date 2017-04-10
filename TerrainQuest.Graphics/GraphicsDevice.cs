using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Drawing;
using System.Windows.Forms;
using Device = SharpDX.Direct3D11.Device;

namespace TerrainQuest.Graphics
{
    /// <summary>
    /// A DirectX11 (SharpDX) graphics device
    /// </summary>
    public class GraphicsDevice : IDisposable
    {
        private const int BufferCount = 1;

        private Control _control;
        private GraphicsOptions _options;
        private Viewport _viewport;

        private Device _device;
        private SwapChain _swapChain;
        private DeviceContext _context;

        private Texture2D _backBuffer;
        private RenderTargetView _renderTargetView;

        private Texture2D _depthStencilBuffer;
        private DepthStencilView _depthStencilView;

        /// <summary>
        /// Get the options used to create the graphics device.
        /// </summary>
        public GraphicsOptions Options
        {
            get { return _options; }
        }

        /// <summary>
        /// Get the underlying DirectX device
        /// </summary>
        public Device Device
        {
            get { return _device; }
        }

        /// <summary>
        /// Get the underlying DirectX graphics device context 
        /// </summary>
        public DeviceContext Context
        {
            get { return _context; }
        }

        public GraphicsDevice(Control control, GraphicsOptions options)
        {
            Check.NotNull(control, nameof(control));
            Check.NotNull(options, nameof(options));

            _control = control;
            _options = options;

            Initialize();
        }

        public void Dispose()
        {
            ReleaseAll();
        }

        private void ReleaseAll()
        {
            Utilities.Dispose(ref _depthStencilView);
            Utilities.Dispose(ref _depthStencilBuffer);

            Utilities.Dispose(ref _renderTargetView);
            Utilities.Dispose(ref _backBuffer);

            if (_context != null)
            {
                _context.ClearState();
                _context.Flush();
            }
            Utilities.Dispose(ref _context);
            Utilities.Dispose(ref _swapChain);
            Utilities.Dispose(ref _device);
        }

        private void Initialize()
        {
            try
            {
                _device = CreateDevice();

                // Create swapchain
                var swapChainDescription = CreateSwapChainDescription();
                _swapChain = CreateSwapChain(swapChainDescription);

                // Set device context
                _context = _device.ImmediateContext;

                // Create backbuffer and rendertarget
                _backBuffer = CreateBackBuffer();
                _renderTargetView = CreateRenderTargetView();

                // Create depth/stencil buffer and view
                _depthStencilBuffer = CreateDepthStencilBuffer();
                _depthStencilView = CreateDepthStencilView();

                // Bind views
                BindViewsToOutputMerger();

                // Set viewport
                FitViewportToClient();
            }
            catch
            {
                ReleaseAll();

                throw;
            }
        }

        private Device CreateDevice()
        {
#if DEBUG
            var deviceCreationFlag = DeviceCreationFlags.Debug;
#else
            var deviceCreationFlag = DeviceCreationFlags.None;
#endif
            return new Device(SharpDX.Direct3D.DriverType.Hardware, deviceCreationFlag);
        }

        #region Create Swap Chain

        private SwapChainDescription CreateSwapChainDescription()
        {
            var sampleDescription = CreateSampleDescription();
            var swapChainDescription = new SwapChainDescription
            {
                BufferCount = BufferCount,
                ModeDescription = new ModeDescription(_control.ClientSize.Width,
                    _control.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = _control.Handle,
                SampleDescription = sampleDescription,
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
            return swapChainDescription;
        }

        private SwapChain CreateSwapChain(SwapChainDescription swapChainDesc)
        {
            var factory = new Factory1();
            {
                factory.MakeWindowAssociation(_control.Handle, WindowAssociationFlags.IgnoreAll);
                return new SwapChain(factory, _device, swapChainDesc);
            }
        }

        private SampleDescription CreateSampleDescription()
        {
            SampleDescription sampleDesc;
            if (Options.UseMultisampling && Options.Multisamples > 1)
            {
                int sampleCount = Math.Min(Options.Multisamples, Device.MultisampleCountMaximum);
                int multisampleQuality = _device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, sampleCount);
                Check.Ensures(multisampleQuality > 0);
                sampleDesc = new SampleDescription(sampleCount, multisampleQuality - 1);
            }
            else
            {
                sampleDesc = new SampleDescription(1, 0);
            }
            return sampleDesc;
        }


        #endregion

        #region Create BackBuffer and RenderTarget

        private Texture2D CreateBackBuffer()
        {
            return SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(_swapChain, 0);
        }

        private RenderTargetView CreateRenderTargetView()
        {
            return new RenderTargetView(_device, _backBuffer);
        }

        #endregion

        #region Create Depth/Stencil buffer

        private Texture2D CreateDepthStencilBuffer()
        {
            var depthBufferDescription = CreateDepthBufferDescription();
            return new Texture2D(_device, depthBufferDescription);
        }

        private Texture2DDescription CreateDepthBufferDescription()
        {
            return new Texture2DDescription
            {
                Format = Format.D32_Float_S8X24_UInt,
                Width = _backBuffer.Description.Width,
                Height = _backBuffer.Description.Height,
                SampleDescription = _swapChain.Description.SampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                ArraySize = 1,
                MipLevels = 1
            };
        }

        private DepthStencilView CreateDepthStencilView()
        {
            return new DepthStencilView(_device, _depthStencilBuffer);
        }

        #endregion

        private void BindViewsToOutputMerger()
        {
            _context.OutputMerger.SetRenderTargets(_depthStencilView, _renderTargetView);
        }


        #region Common graphics wrapper functions

        public void ClearRenderAndDepthView(SharpDX.Color color)
        {
            _context.ClearRenderTargetView(_renderTargetView, color);
            _context.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
        }

        public void Present(int syncInterval = 0, PresentFlags presentFlags = PresentFlags.None)
        {
            _swapChain.Present(syncInterval, presentFlags);
        }

        #endregion



        #region Viewport functions

        private void FitViewportToClient()
        {
            var viewportSize = CalculateViewportSizeWithFixedAspect();
            var offset = CalculateViewportOffset(viewportSize.Width, viewportSize.Height);

            _viewport = new Viewport(offset.X, offset.Y, viewportSize.Width, viewportSize.Height, 0, 1);

            Context.Rasterizer.SetViewport(_viewport);
        }

        private Size CalculateViewportSizeWithFixedAspect()
        {
            int viewportWidth;
            int viewportHeight;
            int clientWidth = _control.ClientSize.Width;
            int clientHeight = _control.ClientSize.Height;
            var clientAspect = (float)(clientWidth / clientHeight);
            var viewportAspect = (float)(Options.Resolution.Width / Options.Resolution.Height);

            if (clientAspect < viewportAspect)
            {
                viewportHeight = clientHeight;
                viewportWidth = (int)(viewportHeight * viewportAspect);

                if (clientWidth > viewportWidth)
                {
                    viewportWidth = clientWidth;
                    viewportHeight = (int)(viewportWidth / viewportAspect);
                }
            }
            else
            {
                viewportWidth = clientWidth;
                viewportHeight = (int)(viewportWidth / viewportAspect);

                if (clientHeight > viewportHeight)
                {
                    viewportHeight = clientHeight;
                    viewportWidth = (int)(viewportHeight * viewportAspect);
                }

            }

            return new Size(viewportWidth, viewportHeight);
        }

        private System.Drawing.Point CalculateViewportOffset(int viewportWidth, int viewportHeight)
        {
            var xOffset = (viewportWidth - _control.ClientSize.Width) / 2;
            var yOffset = (viewportHeight - _control.ClientSize.Height) / 2;
            return new System.Drawing.Point(-xOffset, -yOffset);
        }

        #endregion
    }
}
