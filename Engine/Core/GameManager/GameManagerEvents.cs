using Engine.Control;
using Engine.Core.Instance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    //GameManager Instance Events
    public abstract partial class GameManager
    {

        internal void Init(GraphicsDevice graphicsDevice)
        {
           
        }

        internal void LoadContent()
        {
            CommandPanel = new CommandPanel(this);

            LoadContent(Mono.Content);
            LoadScenes();
        }

        internal void UnloadContent()
        {
            SceneManager.Dispose();
            VideoManager.Dispose();
        }

        internal void Update(GameTime gametime)
        {
            //update time
            Time.Update(gametime);
            //update input
            Input.Update();

            //update scenes
            SceneManager.UpdateActiveScenes();

        }

        internal void Draw(GameTime gametime)
        {
            DrawTime.Update(gametime);

            //Start drawing method
            VideoManager.StartDraw(Color.Black);

            SceneManager.DrawActiveScenes();

            CommandPanel.Draw(VideoManager);

            //Render Frame
            VideoManager.FinalDraw();
        }

    }
}
