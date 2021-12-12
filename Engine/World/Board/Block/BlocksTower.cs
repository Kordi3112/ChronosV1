using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public class BlocksTower
    {
        public Block Upper { get; set; }

        public Block Lower { get; set; }

        public BlocksTower(Block lowerBlock, Block upperBlock)
        {
            Lower = lowerBlock;
            Upper = upperBlock;
        }

        public BlocksTower(Vector2 position, Point positionInChunk)
        {
            Init(position, positionInChunk);
        }
        private void Init(Vector2 position, Point positionInChunk)
        {
            Lower = new Block(Block.BlockId.Void, position, positionInChunk);
            Upper = new Block(Block.BlockId.Void, position, positionInChunk);
        }
    }
}
