using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public class PolygonL
    {
        public Vector2[] Vertices { get; set; }

        public Vector2[] RealVertices { get; private set; }

        public PolygonL(params Vector2[] vertices)
        {
            Vertices = vertices;
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
