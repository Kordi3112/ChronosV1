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
        private static float epsilon = 0.00001f;

        private static Vector2 FindFirstDirection(Polygon A, Polygon B)
        {
            Vector2 dir = A.Vertices[0] - A.Vertices[0];
            if (dir.LengthSquared() < epsilon) // Avoid the distance of the first fetched point is 0
            {
                dir = A.Vertices[1] - B.Vertices[0];
            }
            return dir;
        }

        private static bool HandleSimplex(Simplex simplex, ref Vector2 d)
        {
            if (simplex.Count() == 2)
                return LineCase(simplex, ref d);

            return TriangleCase(simplex, ref d);
        }

        private static bool TriangleCase(Simplex simplex, ref Vector2 d)
        {
            Vector2 a = simplex.Get(0);
            Vector2 b = simplex.Get(1);
            Vector2 c = simplex.Get(2);

            Vector2 ab = b - a;
            Vector2 ac = c - a;
            Vector2 ao = -a;

            Vector2 abPerp = Tools.TripleProd(ac, ab, ab);
            Vector2 acPerp = Tools.TripleProd(ab, ac, ac);

            Vector3 abc = Tools.Mul(new Vector3(ab, 0), new Vector3(ac, 0));

            if (Tools.SameDirection(abPerp, ao))
            {
                //remove last point
                simplex.Remove(2);
                d = abPerp;
            }
            else
            {
                if (Tools.SameDirection(acPerp, ao))
                {
                    //remove b
                    simplex.Remove(1);
                    d = acPerp;

                }
                else return true;
            }

            return false;
        }

        private static bool LineCase(Simplex simplex, ref Vector2 d)
        {
            Vector2 a = simplex.Get(0);
            Vector2 b = simplex.Get(1);

            Vector2 ab = b - a;
            Vector2 ao = -a;

            if (Tools.SameDirection(ab, ao)) //same direction
                d = Tools.TripleProd(ab, ao, ab);
            else
            {
                SupportPoint sa = simplex.GetSupport(0);
                simplex.Clear();
                simplex.Add(sa);

                d = ao;
            }

            return false;

        }
    }
}
