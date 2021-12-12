using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public struct LineCollisionInfo
    {
        public float T1 { get; set; }
        public float T2 { get; set; }
        public bool IsParallel { get; set; }
        public Vector2 CollisionPoint { get; set; }

        /// <summary>
        /// new Vector2(T1, T2);
        /// </summary>
        public Vector2 T => new Vector2(T1, T2);

        public static LineCollisionInfo Parallel => new LineCollisionInfo() { IsParallel = true };

        public LineCollisionInfo(float t1, float t2, bool isPallarel)
        {
            T1 = t1;
            T2 = t2;
            CollisionPoint = Vector2.Zero;
            IsParallel = isPallarel;
        }

        public LineCollisionInfo(float t1, float t2, Vector2 collisionPoint, bool isPallarel)
        {
            T1 = t1;
            T2 = t2;
            CollisionPoint = collisionPoint;
            IsParallel = isPallarel;
        }

        /// <summary>
        /// T1 >= 0 && T1 < 1 && T2 >= 0 && T2 < 1
        /// </summary>
        /// <returns></returns>
        public bool IsColliding()
        {
            if (T1 >= 0 && T1 < 1 && T2 >= 0 && T2 < 1)
                return true;
            else return false;
        }
    }
}
