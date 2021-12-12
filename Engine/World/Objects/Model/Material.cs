using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class Material
    {
        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Material()
        {
            Texture = null;
            Color = Color.White;
        }

        public Material(Color color, Texture2D texture)
        {
            Texture = texture;
            Color = color;
        }
    }
}
