using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    /// <summary>
    /// Ax + By + C = 0
    /// </summary>
    public struct LineGeneralForm
    {

        public float A { get; set; }

        public float B { get; set; }

        public float C { get; set; }

        public LineGeneralForm(float a, float b, float c)
        {
            A = a;
            B = b;
            C = c;
        }


        public LineGeneralForm(Vector2 pointA, Vector2 pointB)
        {
            Vector2 delta = pointB - pointA;

            if (delta.X == 0)
            {
                A = 1;
                B = 0;
                C = -pointA.X;
            }
            else
            {
                A = -delta.Y / delta.X;
                B = 1;
                C = -A * pointA.X - pointA.Y;
            }

        }

        /// <summary>
        /// Returns A * point.X + B * point.Y + C
        /// </summary>
        public float GetValue(Vector2 point)
        {
            return A * point.X + B * point.Y + C;
        }
    }
}
