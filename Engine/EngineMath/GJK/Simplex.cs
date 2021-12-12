using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    class Simplex
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

        public GJK.SupportPoint GetSupport(int i)
        {
            return new GJK.SupportPoint
            {
                point = points[i],
                fromA = fromA[i],
                fromB = fromB[i],
            };
        }

        public void Add(GJK.SupportPoint point)
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
    }
}
