using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    /// <summary>
    /// Stores ids of vertices from polygon - they are use to creating triangles
    /// </summary>
    public struct TriangleIds
    {
        public int[] Ids { get; set; }

        public int A => Ids[0];
        public int B => Ids[1];
        public int C => Ids[2];

        public TriangleIds(int A, int B, int C)
        {
            Ids = new int[3];

            Ids[0] = A;
            Ids[1] = B;
            Ids[2] = C;
        }

    }
}
