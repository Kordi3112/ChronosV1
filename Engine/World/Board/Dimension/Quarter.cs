using Engine.Core;
using Engine.EngineMath;
using Engine.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public class Quarter : IDisposable
    {
        [Flags]
        public enum QuarterAlign
        {
            RightBot =  0b0000001,
            RightTop =  0b0000010,
            LeftTop =   0b0000100,
            LeftBot =   0b0001000,

            Right = RightBot | RightTop,
            Left = LeftBot | LeftTop,
            Top = LeftTop | RightTop,
            Bot = LeftBot | RightBot,
            All = RightBot | RightTop | LeftBot | LeftTop,
        }

        public QuarterAlign Align { get; private set; }

        public List<List<Chunk>> ChunksList { get; set; }
        internal Dimension.ChunkArea VisibleChunksArea { get; set; }

        Queue<Chunk> ChunksToUpdateTextures { get; set; }

        Dimension Dimension { get; set; }

        Point MaxChunksCapacity { get; set; }
        internal Point ChunksCapacity;

        internal Quarter(QuarterAlign align, Dimension dimension)
        {
            Align = align;

            ChunksList = new List<List<Chunk>>();

            ChunksToUpdateTextures = new Queue<Chunk>();

            Dimension = dimension;
        }

        internal void Init(Point maxChunksCapacity)
        {
            if (maxChunksCapacity == Point.Zero)
                MaxChunksCapacity = new Point(int.MaxValue - 10, int.MaxValue - 10);
            else MaxChunksCapacity = maxChunksCapacity;

            //Initial change of chunk size

            Point starterChunksSize = new Point(10, 10);

            if (starterChunksSize.X > MaxChunksCapacity.X)
                starterChunksSize.X = MaxChunksCapacity.X;

            if (starterChunksSize.Y > MaxChunksCapacity.Y)
                starterChunksSize.Y = MaxChunksCapacity.Y;

            ChunksList = new List<List<Chunk>>();
            ChangeChunkSize(starterChunksSize);
        }

        /// <summary>
        /// Update chunks - generating
        /// </summary>
        internal void CheckChunks()
        { 
            ActionForEachVisibleChunk((x, y) => {

                Chunk chunk = ChunksList[x][y];


                if (chunk == null)
                {
                    ChunksList[x][y] = new Chunk();

                    LoadChunk(ChunksList[x][y], x, y);

                    chunk = ChunksList[x][y];
                }

                if (chunk.IsChanged && !chunk.InQueueToUpdate)
                {
                    chunk.InQueueToUpdate = true;
                    ChunksToUpdateTextures.Enqueue(chunk);
                }
                    

            });
        }

        /// <summary>
        /// Update chunks - visual
        /// </summary>
        internal void UpdateVisibleChunks(BlocksLibrary blocksLibrary)
        {
            if (ChunksToUpdateTextures.Count == 0)
                return;

            
            while (ChunksToUpdateTextures.Count > 0)
            {
                //if (updatedCount + 1 > maxUpdatesCount)
                    //break;

                Chunk chunk = ChunksToUpdateTextures.Dequeue();

                chunk.Update(Dimension.ChunkDrawTarget, Dimension.GraphicsDevice, blocksLibrary);

                //updatedCount++;
            }
            
        }

        internal void Draw(RenderTarget2D renderTarget)
        {
            Dimension.GraphicsDevice.SetRenderTarget(renderTarget);


            ActionForEachVisibleChunk((x, y) =>
            {

                Chunk chunk = ChunksList[x][y];

                EffectsPack.TextureEffect.Parameters["Texture"].SetValue(chunk.Texture);
                EffectsPack.TextureEffect.CurrentTechnique.Passes[0].Apply();

                VideoManager.DrawTextureRectanglePrimitive(Dimension.GraphicsDevice, chunk.Bounds, Color.White);
            });


        }

        internal void Draw3(RenderTarget2D renderTarget, BlocksLibrary blocksLibrary)
        {
            ActionForEachVisibleChunk((x, y) =>
            {

                Chunk chunk = ChunksList[x][y];

                chunk.Draw2(Dimension.VideoManager, blocksLibrary, Dimension.WorldManager.Camera);
            });
        }

        internal void Draw2(RenderTarget2D renderTarget, BlocksLibrary blocksLibrary)
        {
            ActionForEachVisibleChunk((x, y) => {

                Chunk chunk = ChunksList[x][y];

                chunk.Draw(Dimension.VideoManager, blocksLibrary, Dimension.WorldManager.Camera);
            });
        }

        private void LoadChunk(Chunk chunk, int x, int y)
        {
            if (Align == QuarterAlign.RightBot)
            {
                chunk.Position = new Vector2(ChunkParameters.ChunkSize.X * x, ChunkParameters.ChunkSize.Y * y);
            }
            else if (Align == QuarterAlign.RightTop) 
            {
                chunk.Position = new Vector2(ChunkParameters.ChunkSize.X * x, -ChunkParameters.ChunkSize.Y * (y + 1));
            }
            else if (Align == QuarterAlign.LeftTop) //LeftTop
            {
                chunk.Position = new Vector2(-ChunkParameters.ChunkSize.X * (x + 1), -ChunkParameters.ChunkSize.Y * (y + 1));
            }

            else if (Align == QuarterAlign.LeftBot) //LeftBot
            {
                chunk.Position = new Vector2(-ChunkParameters.ChunkSize.X * (x + 1), ChunkParameters.ChunkSize.Y * y);
            }

            chunk.InitBlocks();

            chunk.Quarter = Align;
            chunk.DimensionCoords = new Point(x, y);

            Dimension.TerraFormer.GenerateChunk(chunk);
        }

        private void ChangeChunkSize(Point size)
        {
            if (size.X <= ChunksCapacity.X && size.Y <= ChunksCapacity.Y)
                return;

            Point diff = size - ChunksCapacity;
            //x diff
            for (int x = 0; x < diff.X; x++)
            {
                int xCoord = ChunksCapacity.X + x;

                ChunksList.Add(new List<Chunk>());
                ChunksList[xCoord] = new List<Chunk>();


                for (int y = 0; y < ChunksCapacity.Y; y++)
                {
                    //add empty chunk
                    ChunksList[xCoord].Add(null);
                }
            }


            if (size.X > ChunksCapacity.X)
                ChunksCapacity.X = size.X;

            //y diff
            for (int x = 0; x < ChunksCapacity.X; x++)
            {
                for (int y = 0; y < diff.Y; y++)
                {
                    //add empty chunk
                    ChunksList[x].Add(null);
                }
            }

            if (size.Y > ChunksCapacity.Y)
                ChunksCapacity.Y = size.Y;
        }

        /// <summary>
        /// When u need to read chunk which have never been seen
        /// </summary>
        internal void LoadChunk(Point position)
        {
            //set chunk size
            ChangeChunkSize(position);

            Chunk chunk = ChunksList[position.X][position.Y];

            if (chunk != null)
                return;

            ChunksList[position.X][position.Y] = new Chunk();

            LoadChunk(ChunksList[position.X][position.Y], position.X, position.Y);
        }

        internal void UpdateVisibleArea(RectangleF viewBounds)
        {
            RectangleF visibleQuarterRectangle = RectangleF.CommonPart(GetQuarterMaxRectangle(), viewBounds);

            VisibleChunksArea = GetChunkAreaFromVisibleRectangle(visibleQuarterRectangle);

            if (VisibleChunksArea.xMax >= ChunksCapacity.X || VisibleChunksArea.yMax >= ChunksCapacity.Y)
            {
                ChangeChunkSize(new Point(VisibleChunksArea.xMax + 1, VisibleChunksArea.yMax + 1));
            }

           

            
        }

        private Dimension.ChunkArea GetChunkAreaFromVisibleRectangle(RectangleF visibleQuarterRectangle)
        {
            if (Align == QuarterAlign.RightBot)
                return new Dimension.ChunkArea(MapCoordsToChunkCoords(visibleQuarterRectangle.LeftTop), MapCoordsToChunkCoords(visibleQuarterRectangle.RightBot));
            else if (Align == QuarterAlign.RightTop)
                return new Dimension.ChunkArea(MapCoordsToChunkCoords(visibleQuarterRectangle.LeftBot), MapCoordsToChunkCoords(visibleQuarterRectangle.RightTop));
            else if (Align == QuarterAlign.LeftTop)
                return new Dimension.ChunkArea(MapCoordsToChunkCoords(visibleQuarterRectangle.RightBot), MapCoordsToChunkCoords(visibleQuarterRectangle.LeftTop));
            else if (Align == QuarterAlign.LeftBot)
                return new Dimension.ChunkArea(MapCoordsToChunkCoords(visibleQuarterRectangle.RightTop), MapCoordsToChunkCoords(visibleQuarterRectangle.LeftBot));

            return new Dimension.ChunkArea();
        }

        private Point MapCoordsToChunkCoords(Vector2 mapCoords)
        {
            return new Point((int)Math.Round(Math.Abs(mapCoords.X) / ChunkParameters.ChunkSize.X), (int)Math.Round(Math.Abs(mapCoords.Y) / ChunkParameters.ChunkSize.Y));
        }

        private RectangleF GetQuarterMaxRectangle()
        {
            if(Align == QuarterAlign.RightBot)
                return new RectangleF(new Vector2(), MaxChunksCapacity.ToVector2());
            else if(Align == QuarterAlign.RightTop)
                return new RectangleF(new Vector2(0, -MaxChunksCapacity.Y), MaxChunksCapacity.ToVector2());
            else if(Align == QuarterAlign.LeftTop)
                return new RectangleF(new Vector2(-MaxChunksCapacity.X, -MaxChunksCapacity.Y), MaxChunksCapacity.ToVector2());
            else if (Align == QuarterAlign.LeftBot)
                return new RectangleF(new Vector2(-MaxChunksCapacity.X, 0), MaxChunksCapacity.ToVector2()); 

            return RectangleF.Zero;
        }


        internal void ActionForEachVisibleChunk(Action<int, int> action)
        {
            for (int x = VisibleChunksArea.xMin; x < VisibleChunksArea.xMax; x++)
            {
                for (int y = VisibleChunksArea.yMin; y < VisibleChunksArea.yMax; y++)
                {
                    action(x, y);
                }

            }
        }

        internal Chunk GetChunk(int x, int y)
        {
            if (x > ChunksCapacity.X || y > ChunksCapacity.Y)
                return null;
        return ChunksList[x][y];
        }

        internal Chunk GetChunk(Point pos) => GetChunk(pos.X, pos.Y);
        public void Dispose()
        {
            //dispose all chunks

            for (int x = 0; x < ChunksCapacity.X; x++)
            {
                for (int y = 0; y < ChunksCapacity.Y; y++)
                {
                    if (ChunksList[x][y] != null)
                        ChunksList[x][y].Dispose();
                }
            }
        }
    }
}
