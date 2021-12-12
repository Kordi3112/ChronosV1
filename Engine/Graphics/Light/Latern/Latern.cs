using Engine.EngineMath;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics.Light
{
    public abstract class Latern
    {
        public enum Type
        {
            Circle,
        };

        public Vector2 Position2 => Transform.Position2;
        public Transform Transform { get; set; }
        public Color Color { get; set; }
        public float Range { get; set; }
        public bool IsShadowCaster { get; set; }
        public bool IsActive { get; set; }
        public RectangleF Bounds { get; set; }
        public new abstract Type GetType();



    }
}
