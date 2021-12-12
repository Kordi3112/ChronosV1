using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public class TerraFormer2 : TerraFormer
    {
        Noise noise;

        public TerraFormer2(int seed)
        {
            Params.Seed = seed;

            noise = new Noise(new Point(1000, 1000), seed);
        }
        public override void GenerateChunk(Chunk chunk)
        {
            ActionForEachBlock((x, y) => {

                BlocksTower blocksTower = chunk.GetBlocksTower(x, y);

                float scale = 0.002f;


                //float value = (float)Perlin.OctavePerlin(block.Position.X * scale, block.Position.Y * scale, 0,5, 0.5);
                //float value = Perlin.fbm(chunk.Position * scale);
                //float value = (float)Perlin.perlin(block.Position.X * scale, block.Position.Y * scale);

                float value = noise.FbmPerlin(blocksTower.Lower.Position * scale - new Vector2(-50, -50), 5);

                int blockId = 0;

                if (value < 0.5f)
                    blockId = -1;
                else if (value < 0.6f)
                    blockId = 0;
                else if (value < 0.62f)
                    blockId = 1;
                else if (value < 0.70f)
                    blockId = 2;
                else if (value < 0.75f)
                    blockId = 3;
                else if (value < 0.80f)
                    blockId = 4;
                else if (value <= 1.0f)
                    blockId = 5;


                if(value > 0.65f)
                {
                    chunk.SetBlock((Block.BlockId)blockId, new Point(x, y), (short)((value * 300) % 4), true);
                    chunk.SetBlock((Block.BlockId)blockId, new Point(x, y), (short)((value * 300) % 4), false);
                }
                else chunk.SetBlock((Block.BlockId)blockId, new Point(x, y), (short)((value * 300) % 4), true);



            });
        }
    }
}
