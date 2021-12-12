using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public abstract class DimensionBrush
    {
        public Point MaxOffsetFromCenter { get; set; } 

        /// <summary>
        /// Tuple<isFieldDefined, BlockId, isLower>
        /// </summary>
        protected abstract Tuple<bool, Block.BlockId, bool> GetBlock(int x, int y);

        public void ActionForeachBlock(Action<int, int, Block.BlockId, bool> action)
        {

            for (int x = -MaxOffsetFromCenter.X; x <= MaxOffsetFromCenter.X; x++)
            {
                for (int y = -MaxOffsetFromCenter.Y; y <= MaxOffsetFromCenter.Y; y++)
                {
                    var tuple = GetBlock(x, y);

                    //tuple.Item1 = IsFieldDefined
                    if (tuple.Item1)
                        action(x, y, tuple.Item2, tuple.Item3);
                }
            }



        }

    }
}
