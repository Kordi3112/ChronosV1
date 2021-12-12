using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    /// <summary>
    /// Triangle Light
    /// </summary>
    public class TriangleL
    {
        public Vector2[] Vertices { get; set; }
        
        public Vector2[] RealVertices { get; private set; }

        public TriangleL(Vector2 A, Vector2 B, Vector2 C)
        {
            Vertices = new Vector2[3];

            Vertices[0] = A;
            Vertices[1] = B;
            Vertices[2] = C;
        }

        /// <summary>
        /// Update Real Vertices
        /// </summary>
        /// <param name="matrix"></param>
        public void Update(Matrix matrix)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                RealVertices[i] = Tools.Mul(Vertices[i], matrix);
            }
        }
    }
}
