using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public partial class GJK
    {
        public struct SupportPoint
        {
            public Vector2 point;
            public Vector2 fromA;
            public Vector2 fromB;
        }

        public static SupportPoint Support(Polygon A, Polygon B, Vector2 dir)
        {
            Vector2 a = A.GetFarthestPointInDirection(dir);
            Vector2 b = B.GetFarthestPointInDirection(-dir);
            return new SupportPoint
            {
                point = a - b,
                fromA = a,
                fromB = b,
            };
        }

        public static bool CheckColliding(Polygon A, Polygon B)
        {
            //initial d 
            Vector2 d = FindFirstDirection(A, B);

            Simplex simplex = new Simplex();

            //add first vertex
            simplex.Add(Support(A, B, d));

            //new direction
            d = Vector2.Zero - d;

            //lopp
            while (true)
            {
                SupportPoint pointA = Support(A, B, d);

                if (Tools.Dot(pointA.point, d) < 0)
                    return false;

                simplex.Add(pointA);

                if (HandleSimplex(simplex, ref d))
                    return true;

            }
        }




    }
}
