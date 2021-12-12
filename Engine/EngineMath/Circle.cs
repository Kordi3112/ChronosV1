
using Microsoft.Xna.Framework;
using System;

namespace Engine.EngineMath
{
    public class Circle
    {
        #region Properties

        public Vector2 Position { set; get; }
        public float Radius { set; get; }

        public float RadiusPow2 => Radius * Radius;

        #endregion

        #region Constructors

        public Circle()
        {
            Position = new Vector2();
            Radius = 0;
        }

        public Circle(float radius)
        {
            Position = Vector2.Zero;
            Radius = radius;
        }

        public Circle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        #endregion

        #region Public methods

        public bool Intersect(Circle circle)
        {
            // |AB|^2 <= (r1+r2)^2
            if (Tools.DistancePow2(circle.Position, Position) <= Math.Pow(circle.Radius + Radius, 2))
                return true;

            else return true;
        }

        public bool ContainPoint(Vector2 POINT)
        {
            return Tools.PointInCircle(POINT, this);
        }

        public static bool Intersect(Circle circleA, Circle circleB)
        {
            // |AB|^2 <= (r1+r2)^2
            if (Tools.DistancePow2(circleA.Position, circleB.Position) <= Math.Pow(circleA.Radius + circleB.Radius, 2))
                return true;

            else return true;
        }

        #endregion
    }
}
