using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Instance
{
    class MonoGameInstance : Game
    {
        public GameManager GameManager { get; set; }

        GraphicsDeviceManager _graphicsDeviceManager;

        public MonoGameInstance(GameManager gameManager)
        {

            GameManager = gameManager;
            GameManager.Mono.MonogameInstance = this;

            Vector2 resolution = new Vector2(1200, 600);

            resolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            _graphicsDeviceManager = new GraphicsDeviceManager(this)
            {

                // PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                // PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,

                PreferredBackBufferWidth = (int)resolution.X,
                PreferredBackBufferHeight = (int)resolution.Y,

                GraphicsProfile = GraphicsProfile.HiDef,

                

            };


            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            _graphicsDeviceManager.HardwareModeSwitch = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 200f);
            
            IsFixedTimeStep = true;

            _graphicsDeviceManager.ApplyChanges();


            GameManager.Mono.GraphicsDevice = GraphicsDevice;


            //Set VideoManager
            GameManager.VideoManager = new VideoManager(GraphicsDevice, _graphicsDeviceManager)
            {
                Resolution = resolution,
                IsFullscreen = false,
                Mode = VideoManager.ViewMode.SixteenNine,

            };

            GameManager.VideoManager.ApplyChanges();

            //Set Content root
            Content.RootDirectory = "Content";



            // GameManager.VideoManager = new VideoManager(_graphicsDeviceManager, GraphicsDevice);
            // GameManager.VideoManager.ScreenResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);




            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            GameManager.Mono.Content = Content;

            base.Initialize();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            GameManager.Activated();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            GameManager.Deactivated();
        }

        protected override void LoadContent()
        {
            GameManager.LoadContent();
        }

        protected override void UnloadContent()
        {
            GameManager.UnloadContent();

        }

        protected override void Update(GameTime gameTime)
        {
            GameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        
    }
}
