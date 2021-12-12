using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public abstract class TerraFormer
    {
        public TerraFormerParams Params { get; set; }

        public TerraFormer()
        {

            Params = new TerraFormerParams();

        }


        public abstract void GenerateChunk(Chunk chunk);

        public void ActionForEachBlock(Action<int, int> action)
        {
            for (int x = 0; x < ChunkParameters.ChunkCellsSize.X; x++)
            {
                for (int y = 0; y < ChunkParameters.ChunkCellsSize.X; y++)
                {
                    action(x, y);
                }

            }
        }
    }
}
