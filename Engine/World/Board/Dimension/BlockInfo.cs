using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public struct BlockInfo
    {
        public Quarter.QuarterAlign Quarter { get; set; }

        public Point ChunkCoords { get; set; }

        public Point PositionInChunk { get; set; }

        public bool Lower { get; set; }


        public override string ToString()
        {
            return "{Quarter: " + Quarter + " ChunkCoords: " + ChunkCoords + " PositionInChunk: " + PositionInChunk + "}";
        }
    }
}
