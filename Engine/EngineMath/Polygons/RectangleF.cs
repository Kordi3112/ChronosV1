using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public struct RectangleF
    {
        public Vector2 Point { get; set; }
        public Vector2 Size { get; set; }

        public float X => Point.X;
        public float Y => Point.Y;

        public float Width => Size.X;
        public float Height => Size.Y;

        public float X2 => X + Width;
        public float Y2 => Y + Height;

        public Vector2 TopMiddle => new Vector2(X + 0.5f * Width, 0);
        public Vector2 BotMiddle => new Vector2(X + 0.5f * Width, Y2);
        public Vector2 LeftMiddle => new Vector2(0, Y + 0.5f * Height);
        public Vector2 RightMiddle => new Vector2(X2, Y + 0.5f * Height);
        public Vector2 LeftTop => new Vector2(X, Y);
        public Vector2 LeftBot => new Vector2(X, Y2);
        public Vector2 RightTop => new Vector2(X2, Y);
        public Vector2 RightBot => new Vector2(X2, Y2);

        public Vector2 Center => Point + Size * 0.5f;

        public static RectangleF Zero => new RectangleF(Vector2.Zero, Vector2.Zero);

        public RectangleF(Vector2 point, Vector2 size)
        {
            Point = point;
            Size = size;
        }

        public static RectangleF CreateFromCenter(Vector2 center, Vector2 size)
        {
            return new RectangleF(center - 0.5f * size, size);
        }

        public static RectangleF CreateFromPoints(Vector2 point1, Vector2 point2)
        {
            return new RectangleF(point1, point2 - point1);
        }

        public static RectangleF CreateFromPointsWithCheck(Vector2 point1, Vector2 point2)
        {
            return CreateFromPoints(new Vector2(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y)), new Vector2(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y)));
        }



        public static bool DoIntersect(RectangleF A, RectangleF B)
        {
            if ((A.X < B.X2) && (A.X2 > B.X) &&
                (A.Y < B.Y2) &&
                (A.Y2 > B.Y))
            {
                //"Intersecting"

                return true;
            }
            else return false;
        }

        public static bool Intersect(RectangleF a, RectangleF b)
        {
            return (Math.Abs((a.X + a.Width * 0.5f) - (b.X + b.Width * 0.5f)) * 2 < (a.Width + b.Width)) &&
                   (Math.Abs((a.Y + a.Height * 0.5f) - (b.Y + b.Height * 0.5f)) * 2 < (a.Height + b.Height));
        }

        public static RectangleF CommonPart(RectangleF A, RectangleF B)
        {
            Interval Ax = new Interval(A.X, A.X2);
            Interval Bx = new Interval(B.X, B.X2);

            Interval Ay = new Interval(A.Y, A.Y2);
            Interval By = new Interval(B.Y, B.Y2);

            Interval ABx = Interval.CommonPart(Ax, Bx);
            Interval ABy = Interval.CommonPart(Ay, By);

            if (ABx != Interval.Empty && ABy != Interval.Empty)
            {
                return new RectangleF(new Vector2(ABx.Left, ABy.Left), new Vector2(ABx.Lenght(), ABy.Lenght()));
            }
            else return RectangleF.Zero;
        }

        public static RectangleF Union(RectangleF A, RectangleF B)
        {
            return RectangleF.CreateFromPoints(new Vector2(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y)), new Vector2(Math.Max(A.X2, B.X2), Math.Max(A.Y2, B.Y2)));
        }


        /// <summary>
        /// ?
        /// </summary>
        internal RectangleF GetModyfied(Vector2 modyfier)
        {
            Vector2 secondPoint = Vector2.Zero;

            if (modyfier.X != 0 & modyfier.Y != 0)
            {
                secondPoint = modyfier * 0.5f * Size + Center;

                return CreateFromPointsWithCheck(secondPoint, Center);
            }
            else
            {

                if (modyfier.X == 0 && modyfier.Y != 0)
                {
                    float dY = modyfier.Y * 0.5f * Height;
                    float y = Center.Y + dY;

                    return new RectangleF(new Vector2(X, Math.Min(y, Center.Y)), new Vector2(Width, Math.Abs(dY)));
                }
                else if (modyfier.X != 0 && modyfier.Y == 0)
                {
                    float dX = modyfier.X * 0.5f * Width;
                    float x = Center.Y + dX;

                    return new RectangleF(new Vector2(Math.Min(x, Center.X), Y), new Vector2(Math.Abs(dX), Height));
                }
                else
                {
                    return new RectangleF(Point, Size);
                }


            }
        }

        public bool Contains(Vector2 point)
        {
            if (point.X >= X && point.X <= X2 && point.Y >= Y && point.Y <= Y2)
            {
                return true;
            }

            else return false;
        }

        /// <summary>
        /// Return true if WHOLE rectangle is in left rectangle
        /// </summary>
        public bool Contains(RectangleF rectangle)
        {
            if (rectangle.Width <= Width && rectangle.Height <= Height)
            {
                if (Contains(rectangle.Point))
                    return true;

                else return false;
            }
            else return false;
        }

        public bool Intersect(RectangleF B)
        {
            return Intersect(this, B);
        }

        /// <summary>
        /// Returns true if line cross Rectangle Frame
        /// </summary>
        public bool IsCrossedByLine(Line line)
        {
            return IsCrossedByLine(line.ToGeneralForm());
        }
        /// <summary>
        /// Returns true if line cross Rectangle Frame
        /// </summary>
        public bool IsCrossedByLine(LineGeneralForm line)
        {
            return Tools.DoLineCrossPolygon(new Vector2[] {LeftTop, RightTop, RightBot, LeftBot }, line);
        }

        public override string ToString()
        {
            return "{Point: " + Point + " Size: " + Size + "}";
        }
    }
}
