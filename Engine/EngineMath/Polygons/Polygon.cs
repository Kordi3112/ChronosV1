using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public class Polygon
    {
        public int Size => Vertices.Length;

        public Vector2[] Vertices { get; private set; }

       // public Line[] Lines { get; private set; }

        /// <summary>
        /// For LightManager
        /// </summary>
        internal List<int> LightsIdsInRange { get; set; }


        public Polygon(Vector3 scale, params Vector2[] vertices)
        {
            Vertices = new Vector2[vertices.Length];

            for (int i = 0; i < Size; i++)
                Vertices[i] = new Vector2(scale.X, scale.Y) * vertices[i];

            LightsIdsInRange = new List<int>();
        }

        public Polygon(params Vector2[] verticles)
        {
            Vertices = verticles;
            LightsIdsInRange = new List<int>();
        }

        public Polygon(int size)
        {
            Vertices = new Vector2[size];
            LightsIdsInRange = new List<int>();
        }

        public Polygon(Polygon basic, Matrix transform)
        {
            Vertices = new Vector2[basic.Size];
            //Lines = new Line[basic.Size];

            Transform(basic, transform);

            LightsIdsInRange = new List<int>();
        }

        /// <summary>
        /// Copy
        /// </summary>
        public Polygon(Polygon basic)
        {
            Vertices = basic.Vertices.Clone() as Vector2[];

            LightsIdsInRange = new List<int>();

        }
        /// <summary>
        /// Set polygon fields to transformed other polygon fields; Polygon1.Size should be equal to Polygon2.Size 
        /// </summary>
        public void Transform(Polygon basic, Matrix transform)
        {
            for (int i = 0; i < Size; i++)
            {
                Vertices[i] = Tools.Mul(basic.Vertices[i], transform);
            }
        }




        public bool IsPointInPolygon(Vector2 testPoint)
        {
            bool result = false;
            int j = Vertices.Count() - 1;
            for (int i = 0; i < Vertices.Count(); i++)
            {
                if (Vertices[i].Y < testPoint.Y && Vertices[j].Y >= testPoint.Y || Vertices[j].Y < testPoint.Y && Vertices[i].Y >= testPoint.Y)
                {
                    if (Vertices[i].X + (testPoint.Y - Vertices[i].Y) / (Vertices[j].Y - Vertices[i].Y) * (Vertices[j].X - Vertices[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }

            return result;
        }

        public Vector2 GetFarthestPointInDirection(Vector2 direction)
        {
            //check dot prod for the first vert

            Vector2 output = Vertices[0];
            float dotProd = Tools.Dot(Vertices[0], direction);

            for(int i = 1; i < Vertices.Length; i++)
            {
                float dot = Tools.Dot(Vertices[i], direction);

                if(dot > dotProd)
                {
                    output = Vertices[i];
                    dotProd = dot;
                }
            }

            return output;
        }

    }
}
