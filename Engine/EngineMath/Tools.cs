using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.EngineMath
{
    public class Tools
    {
        // for x = <0,1> y = <1, 0> -> hiperbolic curve  a = <-0.25, inf>
        public static float HyperbolicValue(float x, float a)
        {

            float sqrtDelta = (float)Math.Sqrt(1 + 4 * a);

            float dx = (-1 + sqrtDelta) * 0.5f;

            float dy = -2 * a / (1 + sqrtDelta);

            return a / (x + dx) + dy;
        }

        public static Vector2 Rotate(Vector2 point, float angle)
        {

            float cosinus = (float)Math.Cos(angle);
            float sinus = (float)Math.Sin(angle);

            return new Vector2(point.X * cosinus - point.Y * sinus, point.X * sinus + point.Y * cosinus);
        }

        public static Vector2 Rotate(Vector2 point, float sin, float cos)
        {
            return new Vector2(point.X * cos - point.Y * sin, point.X * sin + point.Y * cos);
        }

        public static float Lerp(float a, float b, float u)
        {
            if (u < 0)
                u = 0;

            if (u > 1)
                u = 1;

            return a + (b - a) * u;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float u)
        {
            if (u < 0)
                u = 0;

            if (u > 1)
                u = 1;

            return a + (b - a) * u;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float u)
        {
            if (u < 0)
                u = 0;

            if (u > 1)
                u = 1;

            return a + (b - a) * u;
        }
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else return value;
        }

        public static float Fract(float value)
        {
            return value - (float)Math.Floor(value);
        }
        public static Vector2 Fract(Vector2 value)
        {
            return new Vector2(Fract(value.X), Fract(value.Y));
        }

        public static Vector2 TripleProd(Vector2 a, Vector2 b, Vector2 c)
        {
            //                      a x b x c
            return ToVector2(Mul(Mul(new Vector3(a, 0), new Vector3(b, 0)), new Vector3(c, 0)));
        }

        /// <summary>
        /// Get Perpendicular vector to AB which pointing at point O
        /// </summary>
        public static Vector2 GetPerpendicularTo(Vector2 o, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ao = o - a;

            return TripleProd(ab, ao, ab);
        }

        public static Vector2 GetPerpendicularNormalTo(Vector2 o, Vector2 a, Vector2 b)
        {
            Vector2 normal = GetPerpendicularTo(o, a, b);
            normal.Normalize();

            return normal;
        }
        /// <summary>
        /// returns true if vectors looks at the same direction (angleBetween < 90deg)
        /// </summary>
        public static bool SameDirection(Vector2 a, Vector2 b)
        {
            if (Dot(a, b) > 0)
                return true;
            return false;

        }
        public static int Floor(float value)
        {
            return (int)value;
        }

        public static Point Floor(Vector2 value)
        {
            return new Point(Floor(value.X), Floor(value.Y));
        }

        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            return (float)Math.Acos(CosAngleBetween(a, b));
        }     

        /// <summary>
        /// If angle > 90deg cos < 0 
        /// </summary>
        public static float CosAngleBetween(Vector2 a, Vector2 b)
        {

            double cos = Dot(a, b) / (a.Length() * b.Length());

            if (cos < -1.0d)
                cos = -1.0d;

            if (cos > 1.0d)
                cos = 1.0d;

            return (float)cos;
        }

        public static Vector2 Abs(Vector2 vector2)
        {
            return new Vector2(Math.Abs(vector2.X), Math.Abs(vector2.Y));
        }

        public static Vector3 Mul(Vector3 a, Vector3 b)
        {
            //return new Vector3(a.Y * b.Z - b.Y * a.Z, b.X * a.Z - a.X * b.Z, a.X * b.Y - b.X * a.Y);
            return Vector3.Multiply(a, b);
        }

        public static Vector2 Mul(Vector2 a, Vector2 b)
        {
            return Vector2.Multiply(a, b);
        }


        public static float MulZ(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - b.X * a.Y;
        }

        public static Vector2 ToVector2(Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static float Distance(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(DistancePow2(point1, point2));
        }

        public static float DistancePow2(Vector2 point1, Vector2 point2)
        {
            return (float)(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public static float Lenght(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static float LenghtPow2(Vector2 vector)
        {
            return (vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            return vector / Lenght(vector);
        }

        public static Vector2 AveragePoint(List<Vector2> points)
        {
            Vector2 result = new Vector2();

            for (int i = 0; i < points.Count; i++)
                result += points[i];

            return result / points.Count;
        }


        public static Vector2 AveragePoint(params Vector2[] points)
        {
            Vector2 result = new Vector2();

            for (int i = 0; i < points.Length; i++)
                result += points[i];

            return result / points.Length;
        }


        public static float Minimum(float[] VARS)
        {
            float min = VARS[0];

            for (int i = 1; i < VARS.Length; i++)
            {
                if (VARS[i] < min)
                    min = VARS[i];
            }

            return min;
        }

        public static float Maximum(float[] VARS)
        {
            float max = VARS[0];

            for (int i = 1; i < VARS.Length; i++)
            {
                if (VARS[i] > max)
                    max = VARS[i];
            }

            return max;
        }

        //creates left sided normal perpendicular to other vector
        public static Vector2 PerpendicularNormal(Vector2 vector)
        {
            Vector2 result = new Vector2(vector.Y, -vector.X);
            result.Normalize();

            return result;
        }

        //creates left sided normal perpendicular to line
        public static Vector2 PerpendicularNormal(Line line)
        {
            return PerpendicularNormal(line.Direction);
        }

        /*
        public static Vector2 PerpendicularNormal(Line line)
        {
            return PerpendicularNormal(new Vector2(line.B, line.A));
        }

        public static Vector2 PerpendicularNormal(Line LINE, Vector2 DIRECTION)
        {
            Vector2 normal = PerpendicularNormal(new Vector2(LINE.B, LINE.A));

            //Setting normal direction
            if ((AngleBetween(normal, DIRECTION) > (float)Math.PI * 0.5f))
                normal *= -1;

            return normal;
        }
        */

        public static Vector2 AngleToNormal(float rotation)
        {
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public static Vector3 Multiply(Vector2 a, Vector2 b)
        {
            return Multiply(new Vector3(a.X, a.Y, 0), new Vector3(b.X, b.Y, 0));
        }

        public static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, -a.X * b.Z + a.Z * b.X, a.X * b.Y - a.Y * b.X);
        }

        public static Vector3 Mul(Vector3 p, Matrix matrix)
        {
            Vector3 point;

            point.X = p.X * matrix.M11 + p.Y * matrix.M21 + p.Z * matrix.M31 + matrix.M41;
            point.Y = p.X * matrix.M12 + p.Y * matrix.M22 + p.Z * matrix.M32 + matrix.M42;
            point.Z = p.X * matrix.M13 + p.Y * matrix.M23 + p.Z * matrix.M33 + matrix.M43;

            return point;
        }


        public static Vector2 Mul(Vector2 p, Matrix matrix)
        {
            Vector2 point;

            point.X = p.X * matrix.M11 + p.Y * matrix.M21 + matrix.M41;
            point.Y = p.X * matrix.M12 + p.Y * matrix.M22 + matrix.M42;

            return point;
        }

        public static Vector3 ToVector3(Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2 PointRelativeTo(Vector2 POINT, Vector2 RELATIVE)
        {
            return POINT - RELATIVE;
        }

        /// OTHER
        ///

        public static Vector2 ToVector2(float[] array)
        {
            if (array.Length != 2)
                return new Vector2(0, 0);

            return new Vector2(array[0], array[1]);
        }

        public static Vector2 ToVector2(int[] array)
        {
            if (array.Length != 2)
                return new Vector2(0, 0);

            return new Vector2(array[0], array[1]);
        }

        ///Collision Stuff
        ///

        //main = sub1 * return.x + sub2 * return.y
        public static Vector2 VectorDistribution(Vector2 main, Vector2 sub1, Vector2 sub2)
        {
            float W = sub1.X * sub2.Y - sub1.Y * sub2.X;

            if (W == 0)
                return new Vector2();

            float Wa = main.X * sub2.Y - main.Y * sub2.X;

            float Wb = sub1.X * main.Y - sub1.Y * main.X;



            return new Vector2(Wa / W, Wb / W);
        }

        //checking if point is in angle ABC - B is origin
        public static bool PointInAngle(Vector2 point, Vector2 A, Vector2 B, Vector2 C)
        {
            Vector2 distribute = VectorDistribution(point - B, A - B, C - B);

            if (distribute.X >= -0.000001f && distribute.Y >= -0.000001f) //0.000001f to remove mistake with not annoucing collision, do not change
                return true;

            else return false;
        }

        //checking if point is in Parallelogram created on angle ABC - B is origin
        public static bool PointInParallelogram(Vector2 point, Vector2 A, Vector2 B, Vector2 C)
        {
            Vector2 distribute = VectorDistribution(point - B, A - B, C - B);

            if (distribute.X >= 0 && distribute.X <= 1 && distribute.Y >= 0 && distribute.Y <= 1)
                return true;

            else return false;
        }

        //checking if point is in triangle ABC 
        public static bool PointInTriangle(Vector2 point, Vector2 A, Vector2 B, Vector2 C)
        {
            // we just have to check if point is in two different angles;

            if (PointInAngle(point, A, B, C) && PointInAngle(point, C, A, B))
                return true;

            else return false;
        }

        //checking if point is in triangle ABC 
        public static bool PointInTriangle(Vector2 point, Vector2[] verticles)
        {
            // we just have to check if point is in two different angles;

            if (PointInAngle(point, verticles[0], verticles[1], verticles[2]) && PointInAngle(point, verticles[2], verticles[0], verticles[1]))
                return true;

            else return false;
        }

        //checking if point is in tetragon ABCD - verticles have to be clockwise
        public static bool PointInTetragon(Vector2 point, Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            if (PointInTriangle(point, A, B, C) || PointInTriangle(point, C, D, A))
                return true;

            else return false;
        }

        //checking if two circles are connected
        public static bool CirclesConnected(Circle circle1, Circle circle2)
        {
            if (DistancePow2(circle1.Position, circle2.Position) <= Math.Pow(circle1.Radius + circle2.Radius, 2))
                return true;

            else return false;
        }

        /// <summary>
        /// Check if point is in the circle
        /// </summary>
        public static bool PointInCircle(Vector2 POINT, Circle CIRCLE)
        {
            if (DistancePow2(POINT, CIRCLE.Position) <= CIRCLE.RadiusPow2)
                return true;

            else return false;
        }


        public static bool IsPointInPolygon(IList<Vector2> polygon, Vector2 testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }

            return result;
        }

        /// <summary>
        /// Returns true if line cross polygon
        /// </summary>
        public static bool DoLineCrossPolygon(Vector2[] polygon, LineGeneralForm line)
        {
            bool valueIsPositive = false;

            float value = line.GetValue(polygon[0]);

            if (value == 0)
                return true;

            if (value > 0)
                valueIsPositive = true;

            for (int i = 1; i < polygon.Length; i++)
            {
                value = line.GetValue(polygon[i]);

                if (value == 0)
                    return true; // Rectangle vertex lay on the line

                if ((value > 0) != valueIsPositive)
                    return true;
            }

            return false;
        }


        public static float CalculateGeometricInertia(Vector2[] vertices, int loops)
        {
            //loops > 2

            float I = 0;

            //first loop




            for (int i = loops; i > 0 ; i--)
            {
                float scale = (float)i / loops;

                for (int v = 0; v < vertices.Length; v++)
                {
                    I += (scale * vertices[v]).LengthSquared();
                }
            }


            return I / (vertices.Length * loops);

        }
    }
}
