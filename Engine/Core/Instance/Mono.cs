using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Instance
{
    /// <summary>
    /// Container for some basic monogame fields
    /// </summary>
    public class Mono
    {
        public SpriteBatch SpriteBatch { get; set; }
        public ContentManager Content { get; set; }
        public Game MonogameInstance { get; set; }

        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
    }
}
