using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public static class ChunkParameters
    {
        public static Point ChunkCellsSize = new Point(40, 40);

        public static Vector2 BlockSize = new Vector2(16, 16);

        public static Vector2 ChunkSize => ChunkCellsSize.ToVector2() * BlockSize;
    }
}
