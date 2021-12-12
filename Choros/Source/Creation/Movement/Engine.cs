using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Creation
{
    class Engine
    {
        public Vector2 Position { get; set; }

        public Vector2 RealPosition { get; set; }

        public float Rotation { get; set; }
        public float RealRotation { get; set; }

        public Engine(Vector2 position)
        {
            Position = position;
        }
    }
}
