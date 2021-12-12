using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath.GJK2
{
    public static class GJKTool
    {
        public static float SqrDistance(Vector2 a, Vector2 b)
        {
            float dx = a.X - b.X;
            float dz = a.Y - b.Y;

            return dx * dx + dz * dz;
        }

        /// Interpretation point c on which side of ab
        public static int WhitchSide(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ac = c - a;
            Vector2 ab = b - a;
            float cross = ab.X * ac.Y - ab.Y * ac.X;

            return cross > 0 ? 1 : (cross < 0 ? -1 : 0);
        }

        /// Get the nearest point from the origin to the line segment ab. The closest point can be a vertical point or the end of a line segment
        public static Vector2 GetClosestPointToOrigin(Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ao = Vector2.Zero - a;

            float sqrLength = ab.LengthSquared();

            // point ab coincides
            if (sqrLength < float.Epsilon)
                return a;

            float projection = Vector2.Dot(ab, ao) / sqrLength;

            if (projection < 0)
                return a;

            if (projection > 1.0f)
                return b;

            return a + ab * projection;
        }

        /// Obtain the vertical point c from the origin to the straight line ab. (c - o) Is the perpendicular line from the origin to ab
        public static Vector2 GetPerpendicularToOrigin(Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ao = Vector2.Zero - a;

            float sqrLength = ab.LengthSquared();

            if (sqrLength < float.Epsilon)
                return Vector2.Zero;

            float projection = Vector2.Dot(ab, ao) / sqrLength;

            return a + ab * projection;
        }

        public static bool Contains(List<Vector2> points, Vector2 point)
        {
            int n = points.Count;

            if (n < 3)
                return false;

            // First calculate the internal direction
            int innerSide = WhitchSide(points[0], points[1], points[2]);

            // Determine whether the simplex contains a point by judging whether the points are all inside the three sides
            for (int i = 0; i < n; ++i)
            {
                int iNext = (i + 1) % n;
                int side = WhitchSide(points[i], points[iNext], point);

                if (side == 0) // On the border
                    return true;

                if (side != innerSide) // Outside
                    return false;
            }

            return true;
        }
    }
}
