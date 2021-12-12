using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Creation
{
    class Engine2
    {
        public enum EngineType
        {
            Backward,
            Forward,
        }

        //BASIC

        public Vector2 Position { get; set; }
        public float MaxThrust { get; set; }
        public float RotationSpeed { get; set; }
        
        public EngineType Type { get; set; }

        //

        public Vector2 RealPosition { get; set; }
        public float TargetRealRotation { get; set; }
        public float RealRotation { get; set; }
        public float Rotation { get; set; }

        public Vector2 F { get; set; }
    }
}
