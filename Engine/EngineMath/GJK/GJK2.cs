using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath.GJK2
{
    public class GJK_2
    {
        private Simplex Simplex = new Simplex();
        private Polygon shapeA;
        private Polygon shapeB;
        /// The maximum number of iterations
        private int maxIterCount = 10;
        /// Floating point error
        private float epsilon = 0.00001f;

        /// Current support direction used
        private Vector2 Direction;
        public bool IsCollision;

        // Nearest point
        public Vector2 ClosestOnA;
        public Vector2 ClosestOnB;

        private SimplexEdge simplexEdge = new SimplexEdge();
        public Vector2 PenetrationVector;
        private Edge currentEpaEdge;


        /// Collision Detection
        public void CheckCollision(Polygon shapeA, Polygon shapeB, bool runEpa = true)
        {
            this.shapeA = shapeA;
            this.shapeB = shapeB;

            Simplex.Clear();
            IsCollision = false;
            Direction = Vector2.Zero;

            ClosestOnA = Vector2.Zero;
            ClosestOnB = Vector2.Zero;

            simplexEdge.Clear();
            currentEpaEdge = null;
            PenetrationVector = Vector2.Zero;

            Direction = FindFirstDirection();
            Simplex.Add(Support(Direction));
            Simplex.Add(Support(-Direction));

            Direction = -GJKTool.GetClosestPointToOrigin(Simplex.Get(0), Simplex.Get(1));

            for (int i = 0; i < 10000; ++i)
            {
                // The direction is close to 0, indicating that the origin is on the side
                if (Direction.LengthSquared() < epsilon)
                {
                    IsCollision = true;
                    break;
                }

                SupportPoint p = Support(Direction);
                // The new point coincides with the previous point. That is, along the direction of dir, no closer point can be found.
                if (GJKTool.SqrDistance(p.point, Simplex.Get(0)) < epsilon ||
                    GJKTool.SqrDistance(p.point, Simplex.Get(1)) < epsilon)
                {
                    IsCollision = false;
                    break;
                }

                Simplex.Add(p);

                // The simplex contains the origin
                if (Simplex.Contains(Vector2.Zero))
                {
                    IsCollision = true;
                    break;
                }

                Direction = FindNextDirection();
            }

            //if there is no collision calculate closest points
            if (!IsCollision)
            {
                ComputeClosetPoint();

                //if (ClosestOnA == Vector2.Zero && ClosestOnB == Vector2.Zero)
                //    IsCollision = true;

                return;
            }

            if (!runEpa)
                return;


            CalculatePenetrationVector();
        }

        public void CalculatePenetrationVector()
        {
            if (!IsCollision)
                return;
            // Only the edge closest to the origin is kept, to avoid floating point errors that cause the origin to fall on the edge, which makes it impossible to calculate the normal direction of the edge
            if (Simplex.Count() > 2)
            {
                FindNextDirection();
            }

            // EPA Algorithm to calculate penetration vector
            simplexEdge.InitEdges(Simplex);

            for (int i = 0; i < maxIterCount; ++i)
            {
                Edge e = simplexEdge.FindClosestEdge();
                currentEpaEdge = e;

                PenetrationVector = e.normal * e.distance;

                Vector2 point = Support(e.normal).point;
                float distance = Vector2.Dot(point, e.normal);

                if (distance - e.distance < epsilon)
                    break;

                simplexEdge.InsertEdgePoint(e, point);
            }
        }
        public SupportPoint Support(Vector2 dir)
        {
            Vector2 a = shapeA.GetFarthestPointInDirection(dir);
            Vector2 b = shapeB.GetFarthestPointInDirection(-dir);
            return new SupportPoint
            {
                point = a - b,
                fromA = a,
                fromB = b,
            };
        }

        public Vector2 FindFirstDirection()
        {
            Vector2 dir = shapeA.Vertices[0] - shapeB.Vertices[0];
            if (dir.LengthSquared() < epsilon) // Avoid the distance of the first fetched point is 0
            {
                dir = shapeA.Vertices[1] - shapeB.Vertices[0];
            }
            return dir;
        }

        public Vector2 FindNextDirection()
        {
            if (Simplex.Count() == 2)
            {
                Vector2 crossPoint = GJKTool.GetClosestPointToOrigin(Simplex.Get(0), Simplex.Get(1));
                // Take the vector close to the origin
                return Vector2.Zero - crossPoint;
            }
            else if (Simplex.Count() == 3)
            {
                Vector2 crossOnCA = GJKTool.GetClosestPointToOrigin(Simplex.Get(2), Simplex.Get(0));
                Vector2 crossOnCB = GJKTool.GetClosestPointToOrigin(Simplex.Get(2), Simplex.Get(1));

                // Keep the one that is close to the origin and remove the one that is farther away
                if (crossOnCA.LengthSquared() < crossOnCB.LengthSquared())
                {
                    Simplex.Remove(1);
                    return Vector2.Zero - crossOnCA;
                }
                else
                {
                    Simplex.Remove(0);
                    return Vector2.Zero - crossOnCB;
                }
            }

            //   Should not be executed here
            return new Vector2(0, 0);
        }

        void ComputeClosetPoint()
        {
            /*
             *  L = AB，It is an edge on the Minkowski difference set, and the vertices constituting the two points A and B also come from the edges of their respective shapes.
             *  E1 = Aa - Ba，E2 = Ab - Bb
             *  Finding the closest distance between two convex hulls has evolved into finding the closest distance between the two edges of E1 and E2.
             *  
             *  Let Q point be the vertical point from the origin to L, then::
             *      L = B - A
             *      Q · L = 0
             *  Because Q is a point on L, you can use r1, r2 to represent Q (r1 + r2 = 1)，Then there are:: Q = A * r1 + B * r2
             *      (A * r1 + B * r2) · L = 0
             *  Use r2 Instead of r1: r1 = 1 - r2
             *      (A - A * r2 + B * r2) · L = 0
             *      (A + (B - A) * r2) · L = 0
             *      L · A + L · L * r2 = 0
             *      r2 = -(L · A) / (L · L)
             */

            SupportPoint A = Simplex.GetSupport(0);
            SupportPoint B = Simplex.GetSupport(1);

            Vector2 L = B.point - A.point;
            float sqrDistanceL = L.LengthSquared();
            // support Points coincide
            if (sqrDistanceL < 0.0001f)
            {
                ClosestOnA = ClosestOnB = A.point;
            }
            else
            {
                float r2 = -Vector2.Dot(L, A.point) / sqrDistanceL;
                r2 = Tools.Clamp(r2, 0, 1);
                float r1 = 1.0f - r2;

                ClosestOnA = A.fromA * r1 + B.fromA * r2;
                ClosestOnB = A.fromB * r1 + B.fromB * r2;
            }
        }
    }

    public struct SupportPoint
    {
        public Vector2 point;
        public Vector2 fromA;
        public Vector2 fromB;
    }

    public class Edge
    {
        public Vector2 a;
        public Vector2 b;
        public Vector2 normal;
        public float distance;
        public int index;
    }

    public class Simplex
    {
        public List<Vector2> points = new List<Vector2>();
        public List<Vector2> fromA = new List<Vector2>();
        public List<Vector2> fromB = new List<Vector2>();

        public void Clear()
        {
            points.Clear();
            fromA.Clear();
            fromB.Clear();
        }

        public int Count()
        {
            return points.Count;
        }

        public Vector2 Get(int i)
        {
            return points[i];
        }

        public SupportPoint GetSupport(int i)
        {
            return new SupportPoint
            {
                point = points[i],
                fromA = fromA[i],
                fromB = fromB[i],
            };
        }

        public void Add(SupportPoint point)
        {
            points.Add(point.point);
            fromA.Add(point.fromA);
            fromB.Add(point.fromB);
        }

        public void Remove(int index)
        {
            points.RemoveAt(index);
            fromA.RemoveAt(index);
            fromB.RemoveAt(index);
        }

        public Vector2 GetLast()
        {
            return points[points.Count - 1];
        }

        public bool Contains(Vector2 point)
        {
            return GJKTool.Contains(points, point);
        }
    }

    public class SimplexEdge
    {
        public List<Edge> edges = new List<Edge>();

        public void Clear()
        {
            edges.Clear();
        }

        public void InitEdges(Simplex simplex)
        {
            edges.Clear();

            if (simplex.Count() != 2)
            {
                throw new System.Exception("simplex point count must be 2!");
            }

            edges.Add(CreateInitEdge(simplex.Get(0), simplex.Get(1)));
            edges.Add(CreateInitEdge(simplex.Get(1), simplex.Get(0)));

            UpdateEdgeIndex();
        }

        public Edge FindClosestEdge()
        {
            float minDistance = float.MaxValue;
            Edge ret = null;
            foreach (var e in edges)
            {
                if (e.distance < minDistance)
                {
                    ret = e;
                    minDistance = e.distance;
                }
            }
            return ret;
        }

        public void InsertEdgePoint(Edge e, Vector2 point)
        {
            Edge e1 = CreateEdge(e.a, point);
            edges[e.index] = e1;

            Edge e2 = CreateEdge(point, e.b);
            edges.Insert(e.index + 1, e2);

            UpdateEdgeIndex();
        }

        public void UpdateEdgeIndex()
        {
            for (int i = 0; i < edges.Count; ++i)
            {
                edges[i].index = i;
            }
        }

        public Edge CreateEdge(Vector2 a, Vector2 b)
        {
            Edge e = new Edge();
            e.a = a;
            e.b = b;

            e.normal = GJKTool.GetPerpendicularToOrigin(a, b);
            float lengthSq = e.normal.LengthSquared();
            if (lengthSq > float.Epsilon)
            {
                e.distance = (float)Math.Sqrt(lengthSq);
                // Unitized edge
                e.normal *= 1.0f / e.distance;
            }
            else
            {
                // Use mathematical methods to get the perpendicular of the straight line, but the direction may be wrong
                Vector2 v = a - b;
                v.Normalize();
                e.normal = new Vector2(-v.Y, v.X);
            }
            return e;
        }

        Edge CreateInitEdge(Vector2 a, Vector2 b)
        {
            Edge e = new Edge
            {
                a = a,
                b = b,
            };

            Vector2 perp = GJKTool.GetPerpendicularToOrigin(a, b);
            e.distance = perp.Length();

            // Use mathematical methods to get the perpendicular of a straight line
            // The direction can be taken at will, just the other side is the opposite
            Vector2 v = a - b;
            v.Normalize();
            e.normal = new Vector2(-v.Y, v.X);

            return e;
        }
    }
}
