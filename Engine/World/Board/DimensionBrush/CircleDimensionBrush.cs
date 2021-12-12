using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public class CircleDimensionBrush : DimensionBrush
    {
        Random random;
        public CircleDimensionBrush()
        {
            long time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            random = new Random((int)(time % 100000));
        }
        /// <summary>
        /// Tuple<isFieldDefined, BlockId, isLower>
        /// </summary>
        protected override Tuple<bool, Block.BlockId, bool> GetBlock(int x, int y)
        {
            float r2 = x * x + y * y;



            bool defined = false;

            if (r2 < (MaxOffsetFromCenter.ToVector2() - Vector2.One).LengthSquared())
                defined = true;

            return new Tuple<bool, Block.BlockId, bool>(defined, (Block.BlockId)random.Next(0, 6), true);
        }
        

    }
}
