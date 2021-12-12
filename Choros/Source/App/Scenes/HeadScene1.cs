using Choros.Source.Resource;
using Engine.Core.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.App.Scenes
{
    public class HeadScene1 : HeadScene
    {
        ResourcePackManager ResourcePackManager;

        Texture2D Cursor1;

        SpriteFont spritefont;

        float time = 0;

        int frames = 0;

        int fps = 0;

        public HeadScene1(ResourcePackManager resourcePackManager)
        {
            ResourcePackManager = resourcePackManager;
        }

        public override void Close()
        {

        }

        public override void Dispose()
        {

        }

        public override void Draw()
        {
            frames++;

            if (time > 0.5f)
            {
                time = 0;
                fps = frames * 2;
                frames = 0;
            }

            time += GameManager.DrawTime.RealDeltaTime;

            GameManager.VideoManager.SetFinalRenderTarget();

            GameManager.VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate);

            GameManager.VideoManager.SpriteBatch.DrawString(spritefont, "FPS: " + fps, new Vector2(0, 0), Color.Yellow, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            GameManager.VideoManager.SpriteBatch.End();

            DrawCursor();
        }

        private void DrawCursor()
        {
            Vector2 position = Mouse.GetState().Position.ToVector2();
            //Vector2 position = GameManager.VideoManager.GraphicsDevice.Cu

            position += GameManager.VideoManager.ViewShift;

            GameManager.VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate);

            GameManager.VideoManager.SpriteBatch.Draw(Cursor1, position, color: Color.Pink, origin: Cursor1.Bounds.Size.ToVector2() * 0.5f);
            GameManager.VideoManager.SpriteBatch.End();
        }

        public override void Init()
        {

            spritefont = ResourcePackManager.FontsPack.Get("Default");

            Cursor1 = GameManager.Mono.Content.Load<Texture2D>("Cursor1");
        }

        public override void Load()
        {

        }

        public override void Update()
        {
            if (GameManager.Input.Keys.IsKeyDown(Keys.Escape))
                GameManager.Exit();

            if (GameManager.Input.Keys.IsKeyDown(Keys.LeftShift))
                GameManager.Time.TimeSpeed = 0.05f;
            else GameManager.Time.TimeSpeed = 1.0f;

            if (GameManager.Input.Keys.ButtonOnClick(Keys.OemTilde))
                GameManager.CommandPanel.IsActive = !GameManager.CommandPanel.IsActive;


            GameManager.CommandPanel.Update();
        }
    }
}
