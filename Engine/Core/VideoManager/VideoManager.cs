using Engine.EngineMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public partial class VideoManager : IDisposable
    {
        public enum ViewMode
        {
            Windowed,
            Resized,
            SixteenNine,
            FourThree,
            SixteenTen,

            //...
        }

        public ViewMode Mode { get; set; }
        public bool IsFullscreen { get; set; }
        public Vector2 Resolution { get; set; }
        public Vector2 Center => Resolution * 0.5f;
        public Vector2 ViewSize { get; private set; }
        public Vector2 ViewCenter => ViewSize * 0.5f;
        public Vector2 ViewShift { get; private set; }
        public Circle ViewCircle { get; private set; }

        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public RenderTarget2D RenderTarget2D { get; private set; }

        public VideoManager(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
        {
            GraphicsDeviceManager = graphicsDeviceManager;
            GraphicsDevice = graphicsDevice;

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            

            SpriteBatch = new SpriteBatch(graphicsDevice);

            //DEFAULT
            IsFullscreen = false;
            Mode = ViewMode.Windowed;

           // GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        }
        

        void SetViewMode(ViewMode viewMode)
        {
            Mode = viewMode;

            if (viewMode == ViewMode.Resized)
            {
                ViewSize = Resolution;
                ViewShift = Vector2.Zero;
            }
            else if (viewMode == ViewMode.Windowed)
            {
                ViewSize = Resolution;
                ViewShift = Vector2.Zero;
            }
            else
            {
                float a = 1;

                if (viewMode == ViewMode.SixteenNine)
                    a = 16.0f / 9.0f;
                else if (viewMode == ViewMode.FourThree)
                    a = 4.0f / 3.0f;
                else if (viewMode == ViewMode.SixteenTen)
                    a = 16.0f / 10.0f;
                //....


                //       X
                if (a * Resolution.Y <= Resolution.X)
                {
                    ViewSize = new Vector2(a * Resolution.Y, Resolution.Y);

                }
                else
                {
                    ViewSize = new Vector2(Resolution.X, Resolution.X / a);
                }

                ViewShift = (Resolution - ViewSize) * 0.5f;

            }
        }


        public void ApplyChanges()
        {
            SetViewMode(Mode);

            GraphicsDeviceManager.PreferredBackBufferWidth = (int)Resolution.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = (int)Resolution.Y;

            GraphicsDeviceManager.IsFullScreen = IsFullscreen;


            //Set ViewCircle
            float r = 0.5f * (float)Math.Sqrt(ViewSize.X * ViewSize.X + ViewSize.Y * ViewSize.Y);
            ViewCircle = new Circle(ViewCenter, r);

            GraphicsDeviceManager.ApplyChanges();

            RenderTarget2D = new RenderTarget2D(GraphicsDevice, (int)ViewSize.X, (int)ViewSize.Y, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        }

        public void SetFinalRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(RenderTarget2D);
        }

        public void SetRenderTarget(RenderTarget2D target)
        {
            GraphicsDevice.SetRenderTarget(target);
        }

        public void ClearTarget(Color color)
        {
            GraphicsDevice.Clear(color);
        }

        public void StartDraw(Color color)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget2D);

            GraphicsDevice.Clear(color);
        }

        public void FinalDraw()
        {
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate);

            SpriteBatch.Draw(RenderTarget2D, ViewShift, Color.White);

            SpriteBatch.End();
        }

        public void Dispose()
        {
            RenderTarget2D.Dispose();
        }
    }
}
