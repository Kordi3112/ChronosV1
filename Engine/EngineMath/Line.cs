using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public struct Line : IEquatable<Line>
    {
        public Vector2 Point { get; set; }

        public Vector2 Direction { get; set; }

        /// <summary>
        /// Creates Line from A to B
        /// </summary>
        public Line(Vector2 A, Vector2 B)
        {
            Point = A;
            Direction = B - A;
        }

        public Line(Vector2 A, Vector2 direction, bool useDirection = false)
        {
            Point = A;

            Direction = direction;
        }

        /// <summary>
        /// returns true if point align on line
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public bool ContainsPoint(Vector2 point)
        {
            if ((point.X - Point.X) * Direction.Y == (point.Y - Point.Y) * Direction.X)
                return true;
            else return false;
        }

        public Vector2 GetPoint(float t)
        {
            return Point + Direction * t;
        }

        /// <summary>
        /// calculate Ta & Tb 
        /// </summary>
        public LineCollisionInfo CheckCollision(Line lineB)
        {

            float invW = 1 / (Direction.Y * lineB.Direction.X - Direction.X * lineB.Direction.Y);

            if (invW == 0)
                return LineCollisionInfo.Parallel;
            else
            {
                float dX = lineB.Point.X - Point.X;
                float dY = lineB.Point.Y - Point.Y;

                float Wa = dY * lineB.Direction.X - dX * lineB.Direction.Y;
                float Wb = dY * Direction.X - dX * Direction.Y;

                float ta = Wa * invW;
                float tb = Wb * invW;

                return new LineCollisionInfo(ta, tb, false);
            }

        }

        /// <summary>
        /// calculate Ta & Tb 
        /// </summary>
        public bool CheckCollision(ref Line line, ref float ta, ref float tb)
        {
            float W = (Direction.Y * line.Direction.X - Direction.X * line.Direction.Y);

            if (W == 0)
                return false;

            float invW = 1.0f / W;



            float dX = line.Point.X - Point.X;
            float dY = line.Point.Y - Point.Y;

            float Wa = dY * line.Direction.X - dX * line.Direction.Y;
            float Wb = dY * Direction.X - dX * Direction.Y;

            ta = Wa * invW;
            tb = Wb * invW;

            return true;

        }

        internal bool CheckCollisionFast2(ref Line line, ref float ta, float minT)
        {
            float W = (Direction.Y * line.Direction.X - Direction.X * line.Direction.Y);


            float dX = line.Point.X - Point.X;
            float dY = line.Point.Y - Point.Y;

            float Wa = dY * line.Direction.X - dX * line.Direction.Y;
                     

            if (W == 0)
                return false;

            ///

            
             
            if(W < 0)
            {
                if (Wa > 0)
                    return false;

                if (Wa < minT * W)
                    return false;

                float Wb = dY * Direction.X - dX * Direction.Y;

                if (Wb > 0)
                    return false;

                if (Wb < W)
                    return false;



                float invW = 1.0f / W;

                ta = Wa * invW;

                return true;
            }
            else
            {
                if (Wa < 0)
                    return false;

                if (Wa > minT * W)
                    return false;

                float Wb = dY * Direction.X - dX * Direction.Y;

                if (Wb < 0)
                    return false;

                if (Wb > W)
                    return false;

                float invW = 1.0f / W;

                ta = Wa * invW;

                return true;

            }
             

        }
        /// <summary>
        /// Returns true if lines have infinity collisions
        /// </summary>
        /// <param name="line"></param>
        /// <param name="minT"></param>
        /// <returns></returns>
        public bool CheckCollisionFast(ref Line line, ref float minT)
        {
            float W = (Direction.Y * line.Direction.X - Direction.X * line.Direction.Y);


            float dX = line.Point.X - Point.X;
            float dY = line.Point.Y - Point.Y;

            float Wa = dY * line.Direction.X - dX * line.Direction.Y;


            if (W == 0)
            {
                // no collision or infinity colision
                if (Wa != 0)
                    return false; // no collsion

                float Wb = dY * Direction.X - dX * Direction.Y;

                if (Wb != 0)
                  return false;// no collsion
              
                return true; //infinity colisions
            }
                

            ///



            if (W < 0)
            {
                if (Wa > 0)
                    return false; // t1 < 0    

                if (Wa < minT * W) // t1 < minT - its not closest
                    return false;

                float Wb = dY * Direction.X - dX * Direction.Y;

                if (Wb > 0)
                    return false; // t2 < 0

                if (Wb <= W)
                    return false; // t2 >= 1


            }
            else
            {
                if (Wa < 0)
                    return false; // t1 < 0

                if (Wa > minT * W)
                    return false; //t1 < minT - its not closest

                float Wb = dY * Direction.X - dX * Direction.Y;

                if (Wb < 0)
                    return false; // t2 < 0

                if (Wb >= W)
                    return false; // t2 >= 1
            }

            float invW = 1.0f / W;

            minT = Wa * invW;

            return false;
        }
        /// <summary>
        /// calculate Ta & Tb & CollisionPoint
        /// </summary>
        public LineCollisionInfo CheckCollisionPoint(Line lineB)
        {

            float W = Direction.Y * lineB.Direction.X - Direction.X * lineB.Direction.Y;

            if (W == 0)
                return LineCollisionInfo.Parallel;
            else
            {
                float dX = lineB.Point.X - Point.X;
                float dY = lineB.Point.Y - Point.Y;

                float Wa = dY * lineB.Direction.X - dX * lineB.Direction.Y;
                float Wb = dY * Direction.X - dX * Direction.Y;

                float ta = Wa / W;
                float tb = Wb / W;

                return new LineCollisionInfo(ta, tb, GetPoint(ta), false);
            }

        }

        /// <summary>
        /// Transform to Ax + By + C = 0
        /// </summary>
        public LineGeneralForm ToGeneralForm()
        {
            //
            float A;
            float B = 1;
            float C;

            if (Direction.X == 0)
            {
                A = 1;
                B = 0;
                C = -Point.X;
            }
            else
            {
                A = -Direction.Y / Direction.X;

                float t = -Point.X / Direction.X;

                C = -(Point.Y * Direction.Y * t);
            }

            return new LineGeneralForm(A, B, C);
        }

        public bool Equals(Line other)
        {
            if (Point == other.Point && Direction == other.Direction)
                return true;
            else return false;
        }

        public static bool operator== (Line A, Line B)
        {
            if (A.Equals(B))
                return true;
            else return false;

        }

        public static bool operator!=(Line A, Line B)
        {
            if (A == B)
                return false;
            else return true;

        }
    }
}
