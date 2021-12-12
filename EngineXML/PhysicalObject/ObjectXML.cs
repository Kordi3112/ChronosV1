using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineXML
{
    public class ObjectXML
    {
        public class Hitbox
        {
            public Vector2[] Vertices;
        }

        public class Component
        {
            public string Tag;
            public string Name;
            public float Rotation;
            public Vector2 Position;
        }

        public Component[] Components;
        public float HitboxScale;
        public Hitbox[] Hitboxes;
        public float HitboxRadius;
    }
}
