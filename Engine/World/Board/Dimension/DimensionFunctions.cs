using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public partial class Dimension
    {
        /// <summary>
        /// Returns chunk coordinate
        /// </summary>   
        Point MapCoordsToChunkCoords(Vector2 mapCoords)
        {
            return new Point((int)Math.Floor(Math.Abs(mapCoords.X) / ChunkParameters.ChunkSize.X), (int)Math.Floor(Math.Abs(mapCoords.Y) / ChunkParameters.ChunkSize.Y));
        }

        public BlockInfo GetBlockInfo(Vector2 mapCoords)
        {

            BlockInfo blockInfo = GetChunkInfo(mapCoords);

            Vector2 chunkPosition = ChunkPosition(blockInfo.ChunkCoords, blockInfo.Quarter);


            blockInfo.PositionInChunk = GetBlockInChunkCoords(chunkPosition, mapCoords);

            return blockInfo;

        }

        public BlockInfo GetChunkInfo(Vector2 mapCoords)
        {
            BlockInfo blockInfo = new BlockInfo();

            //check quarter
            if (mapCoords.X >= 0)
            {
                if (mapCoords.Y >= 0)
                    blockInfo.Quarter = Quarter.QuarterAlign.RightBot;
                else blockInfo.Quarter = Quarter.QuarterAlign.RightTop;
            }
            else
            {
                if (mapCoords.Y >= 0)
                    blockInfo.Quarter = Quarter.QuarterAlign.LeftBot;
                else blockInfo.Quarter = Quarter.QuarterAlign.LeftTop;
            }

            blockInfo.ChunkCoords = MapCoordsToChunkCoords(mapCoords);

            return blockInfo;
        }
        public BlockInfo GetBlockInfo(Vector2 rayStartingPoint, Vector2 rayFinishPoint)
        {
            BlockInfo blockInfo = GetBlockInfo(rayFinishPoint);
            blockInfo.Lower = true;

            Vector2 ray = rayFinishPoint - rayStartingPoint;

            BlockInfo startingPointChunk = GetChunkInfo(rayStartingPoint);

            Vector2 centerChunkPos = ChunkPosition(startingPointChunk.ChunkCoords, startingPointChunk.Quarter);

            LineGeneralForm rayLine = new LineGeneralForm(rayStartingPoint, rayFinishPoint);

            float closestRange2 = (GetQuarter(blockInfo.Quarter).GetChunk(blockInfo.ChunkCoords).GetBlocksTower(blockInfo.PositionInChunk).Lower.Bounds.Center - rayStartingPoint).LengthSquared();

            float rayRange = ray.Length();

            ActionForEachSurroundingChunk(startingPointChunk, rayRange, chunk => {

                bool isCenterChunk = true;
                //action
                Vector2 chunkToChunkVector = chunk.Position - centerChunkPos;
                if (chunkToChunkVector != Vector2.Zero)
                {
                    if (!chunk.Bounds.IsCrossedByLine(rayLine))
                        return;

                    if (!Tools.SameDirection(chunkToChunkVector, ray))
                        return;

                    isCenterChunk = false;
                }



                //check colliion for each Upper Block

                chunk.UpperBlocksPooler.ActionForeachActive(block => {

                    if (!block.Bounds.IsCrossedByLine(rayLine))
                        return;

                    Vector2 blockPos = block.Bounds.Center;

                    //if its center chunk u have to check if direction is the same
                    if (isCenterChunk)
                    {
                        Vector2 blockLine = blockPos - rayStartingPoint;
                        if (!Tools.SameDirection(blockLine, ray))
                            return;
                    }

                    //calculate distance
                    float r2 = (blockPos - rayStartingPoint).LengthSquared();

                    if(r2 <= closestRange2)
                    {
                        closestRange2 = r2;
                        blockInfo = new BlockInfo()
                        {
                            Quarter = chunk.Quarter,
                            ChunkCoords = chunk.DimensionCoords,
                            PositionInChunk = block.PositionInChunk,
                            Lower = false,
                        };


                    }
                });


            });

            return blockInfo;

        }


        internal void ActionForEachSurroundingChunk(BlockInfo centerChunkInfo, float range, Action<Chunk> action)
        {
            int maxOffset = (int)Math.Ceiling( range / Math.Max(ChunkParameters.ChunkSize.X, ChunkParameters.ChunkSize.Y));

            //always get center chunk
            Quarter initialQuarter = GetQuarter(centerChunkInfo.Quarter);

            Chunk _centerChunk = initialQuarter.GetChunk(centerChunkInfo.ChunkCoords.X, centerChunkInfo.ChunkCoords.Y);
            action(_centerChunk);

            for(int x = centerChunkInfo.ChunkCoords.X - maxOffset; x <= centerChunkInfo.ChunkCoords.X + maxOffset; x++)
            {
                for (int y = centerChunkInfo.ChunkCoords.Y - maxOffset; y <= centerChunkInfo.ChunkCoords.Y + maxOffset; y++)
                {
                    if (x == centerChunkInfo.ChunkCoords.X && y == centerChunkInfo.ChunkCoords.Y)
                        continue;

                    if (x < 0 || y < 0)
                    {
   
                        //out of quarter
                        //FixCoords
                        Chunk chunk1 = GetChunkWithFixedCoordinates(centerChunkInfo.Quarter, x, y, true);

                        action(chunk1);

                        continue; 
                    }

                    Chunk chunk = initialQuarter.GetChunk(x, y);

                    if (chunk == null)
                    {
                        initialQuarter.LoadChunk(new Point(x, y));
                        chunk = initialQuarter.GetChunk(x, y);
                    }
                        

                    action(chunk);

                }
            }

        }

        private Chunk GetChunkWithFixedCoordinates(Quarter.QuarterAlign quarterAlign, int x, int y, bool loadIfNotExist = false)
        {
            Quarter.QuarterAlign fixedAlign = quarterAlign;
            Point fixedChunkCoords = new Point(x, y);

            bool correctX = false;
            bool correctY = false;

            if(quarterAlign == Quarter.QuarterAlign.RightBot)
            {
                if(x < 0)
                {
                    if(y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.LeftTop;
                    }
                    else
                    {
                        fixedAlign = Quarter.QuarterAlign.LeftBot;
                        correctY = true;
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.RightTop;
                        correctX = true;
                    }
                    else
                    {
                        correctX = true;
                        correctY = true;
                    }
                }

            }

            if (quarterAlign == Quarter.QuarterAlign.RightTop)
            {
                if (x < 0)
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.LeftBot;
                    }
                    else
                    {
                        fixedAlign = Quarter.QuarterAlign.LeftTop;
                        correctY = true;
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.RightBot;
                        correctX = true;
                    }
                    else
                    {
                        correctX = true;
                        correctY = true;
                    }
                }
            }

            if (quarterAlign == Quarter.QuarterAlign.LeftTop)
            {
                if (x < 0)
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.RightBot;
                    }
                    else
                    {
                        fixedAlign = Quarter.QuarterAlign.RightTop;
                        correctY = true;
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.LeftBot;
                        correctX = true;
                    }
                    else
                    {
                        correctX = true;
                        correctY = true;
                    }
                }
            }

            if (quarterAlign == Quarter.QuarterAlign.LeftBot)
            {
                if (x < 0)
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.RightTop;
                    }
                    else
                    {
                        fixedAlign = Quarter.QuarterAlign.RightBot;
                        correctY = true;
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        fixedAlign = Quarter.QuarterAlign.LeftTop;
                        correctX = true;
                    }
                    else
                    {
                        correctX = true;
                        correctY = true;
                    }
                }
            }

            if(!correctX)
            {
                fixedChunkCoords.X = Math.Abs(fixedChunkCoords.X) - 1;
            }
            if(!correctY)
            {
                fixedChunkCoords.Y = Math.Abs(fixedChunkCoords.Y) - 1;
            }

            Quarter quarter = GetQuarter(fixedAlign);
            Chunk chunk = quarter.GetChunk(fixedChunkCoords);

            if (loadIfNotExist && chunk == null)
            {
                quarter.LoadChunk(fixedChunkCoords);
                return quarter.GetChunk(fixedChunkCoords);
            }


            return chunk;
        }

        public void SetChunkInChange(BlockInfo blockInfo)
        {
            Chunk chunk = GetQuarter(blockInfo.Quarter).GetChunk(blockInfo.ChunkCoords.X, blockInfo.ChunkCoords.Y);

            if (chunk == null)
                return;

            chunk.Changed();
        }

        public void SetBlock(BlockInfo blockInfo, Block.BlockId blockId, short variant = 0)
        {
            Chunk chunk = GetQuarter(blockInfo.Quarter).GetChunk(blockInfo.ChunkCoords.X, blockInfo.ChunkCoords.Y);

            if (chunk == null)
                return;

            chunk.SetBlock(blockId, blockInfo.PositionInChunk, variant, blockInfo.Lower);
        }

        public void SetBlocks(DimensionBrush brush, Vector2 centerBlockPos)
        {
            BlockInfo centerBlockInfo = GetBlockInfo(centerBlockPos);

            Quarter quarter = GetQuarter(centerBlockInfo.Quarter);

            brush.ActionForeachBlock((x, y, blockId, isLower) => {

                int X = centerBlockInfo.PositionInChunk.X + x;
                int Y = centerBlockInfo.PositionInChunk.Y + y;

                if (X < 0 || Y < 0 || X >= ChunkParameters.ChunkCellsSize.X || Y >= ChunkParameters.ChunkCellsSize.Y)
                    return; //TODO: Implement

                quarter.GetChunk(centerBlockInfo.ChunkCoords).SetBlock(blockId, new Point(X, Y) , 0, isLower);

            });

            quarter.GetChunk(centerBlockInfo.ChunkCoords).Changed();
        }

        internal Vector2 ChunkPosition(Point chunkPos, Quarter.QuarterAlign quarterAlign)
        {
            if (quarterAlign == Quarter.QuarterAlign.RightBot)
                return ChunkParameters.ChunkSize * chunkPos.ToVector2();

            if (quarterAlign == Quarter.QuarterAlign.RightTop)
                return new Vector2(ChunkParameters.ChunkSize.X * chunkPos.X, -ChunkParameters.ChunkSize.Y * (1 + chunkPos.Y));

            if (quarterAlign == Quarter.QuarterAlign.LeftTop)
                return new Vector2(-ChunkParameters.ChunkSize.X * (1 + chunkPos.X), -ChunkParameters.ChunkSize.Y * (1 + chunkPos.Y));

            if (quarterAlign == Quarter.QuarterAlign.LeftBot)
                return new Vector2(-ChunkParameters.ChunkSize.X * (1 + chunkPos.X), ChunkParameters.ChunkSize.Y * chunkPos.Y);

            return Vector2.Zero;

        }

        public Point GetBlockInChunkCoords(Vector2 chunkPosition, Vector2 mapCoords)
        {
            Vector2 pos = Tools.PointRelativeTo(mapCoords, chunkPosition);

            return new Point((int)Math.Floor(Math.Abs(pos.X) / ChunkParameters.BlockSize.X), (int)Math.Floor(Math.Abs(pos.Y) / ChunkParameters.BlockSize.Y));
        }

        internal Quarter GetQuarter(Quarter.QuarterAlign quarterAlign)
        {
            foreach (Quarter quarter in Quarters)
                if (quarter.Align == quarterAlign)
                    return quarter;

            return null;
        }

    }
}
