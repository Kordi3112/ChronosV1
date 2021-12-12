using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public class TerraFormer1 : TerraFormer
    {
        public TerraFormer1(int seed)
        {
            Params.Seed = seed;
        }

        public override void GenerateChunk(Chunk chunk)
        {

        }
    }
}
