using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public partial class Dimension
    {


        public struct ChunkArea
        {
            public int xMin;
            public int xMax;
            public int yMin;
            public int yMax;

            public ChunkArea(int xMin, int xMax, int yMin, int yMax)
            {
                this.xMin = xMin;
                this.xMax = xMax;
                this.yMin = yMin;
                this.yMax = yMax;
            }

            public ChunkArea(Point x, Point y)
            {
                xMin = x.X;
                yMin = x.Y;
                xMax = y.X;
                yMax = y.Y;
            }

            public static ChunkArea Null => new ChunkArea(0, -1, 0, -1);
            
        }

        internal ChunkArea[] VisibleChunksArea { get; private set; }
    }
}
