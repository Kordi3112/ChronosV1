using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public struct Force
    {
        public enum ForceType
        {
            Impulse,
            Continous
        }

        public Vector2 Value { get; set; }

        public Vector2 Point { get; set; }

        public ForceType Type { get; set; }

        public Force(Vector2 point, Vector2 value)
        {
            Point = point;
            Value = value;

            Type = ForceType.Continous;
        }

        public Force(Vector2 point, Vector2 value, ForceType type)
        {
            Point = point;
            Value = value;

            Type = type;
        }

        public Force(Vector2 value)
        {
            Point = Vector2.Zero;
            Value = value;

            Type = ForceType.Continous;
        }

        public Force(Vector2 value, ForceType type)
        {
            Point = Vector2.Zero;
            Value = value;

            Type = type;
        }
    }
}
