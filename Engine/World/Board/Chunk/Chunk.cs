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
    public class Chunk : IDisposable
    {
        /// <summary>
        /// Texture of the whole chunk
        /// </summary>
        public Texture2D Texture { get; private set; }

        public bool IsChanged { get; private set; }
        internal bool InQueueToUpdate { get; set; }

        public bool firstUpdate;

        public bool IsEmpty => UpperBlocksPooler.Count == 0  && LowerBlocksPooler.Count == 0 ? true : false;

        public bool HasAnyUpperBlock => UpperBlocksPooler.Count == 0 ? true : false;

        /// <summary>
        /// Position LeftTop corner
        /// </summary>
        public Vector2 Position { get; internal set; }

        public Point DimensionCoords { get; internal set; }

        public Quarter.QuarterAlign Quarter { get; internal set; }

        public RectangleF Bounds => new RectangleF(Position, ChunkParameters.ChunkSize);

        /// <summary>
        /// Chunk data v1
        /// </summary>
        BlocksTower[,] BlocksTowers { get; set; }

        /// <summary>
        /// Chunk data v2
        /// </summary>
        /// 
        internal BlocksPooler UpperBlocksPooler { get; set; }
        internal BlocksPooler LowerBlocksPooler { get; set; }

        public Chunk()
        {
            IsChanged = true;
            firstUpdate = true;

            //Blocks = new Block[ChunkParameters.ChunkCellsSize.X, ChunkParameters.ChunkCellsSize.Y];

            BlocksTowers = new BlocksTower[ChunkParameters.ChunkCellsSize.X, ChunkParameters.ChunkCellsSize.Y];

            UpperBlocksPooler = new BlocksPooler();
            LowerBlocksPooler = new BlocksPooler();
        }

        public void InitBlocks()
        {

            for (int x = 0; x < ChunkParameters.ChunkCellsSize.X; x++)
            {
                for (int y = 0; y < ChunkParameters.ChunkCellsSize.Y; y++)
                {
                    BlocksTowers[x, y] = new BlocksTower(Position + new Vector2(x * ChunkParameters.BlockSize.X, y * ChunkParameters.BlockSize.Y), new Point(x, y));
                }
            }

        }

        /// <summary>
        /// Update chunk texture
        /// </summary>
        public void Update(RenderTarget2D target, GraphicsDevice graphicsDevice, BlocksLibrary blockskLibrary)
        {
            if (!IsChanged)
                return;

            ForceUpdate(target, graphicsDevice, blockskLibrary);
        }

        void ForceUpdate(RenderTarget2D target, GraphicsDevice graphicsDevice, BlocksLibrary blocksLibrary)
        {

            graphicsDevice.SetRenderTarget(target);

            graphicsDevice.Clear(Color.Transparent);

            Effect effect = EffectsPack.TextureEffect;

            //Translate cell map coords to texture coords
            Matrix matrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) * Matrix.CreateScale(1 / (ChunkParameters.ChunkCellsSize.X * ChunkParameters.BlockSize.X), 1 / (ChunkParameters.ChunkCellsSize.Y * ChunkParameters.BlockSize.Y), 1);

            Matrix matrix2 = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateScale(2f, -2f, 1);

            //apply current pass

            effect.Parameters["WorldViewProjection"].SetValue(matrix * matrix2);
            EffectsPack.ColorEffect.Parameters["WorldViewProjection"].SetValue(matrix * matrix2);

            effect.CurrentTechnique.Passes[0].Apply();




            /// Draw Lower blocks
            LowerBlocksPooler.ActionForeachActive(block => {
                    
                //check if there is block above
                if (BlocksTowers[block.PositionInChunk.X, block.PositionInChunk.Y].Upper.Id != Block.BlockId.Void)
                    return;

                Color blockColor = new Color(0.5f, 0.5f, 0.5f);

                //SetTexture
                effect.Parameters["Texture"].SetValue(blocksLibrary.GetBlockTexture(block.Id));
                effect.CurrentTechnique.Passes[0].Apply();

                DrawBlock(graphicsDevice, block, blockColor);


                EffectsPack.Apply(EffectsPack.ColorEffect);

                //VideoManager.DrawRectangleFrame(graphicsDevice, block.Bounds, Color.Black) ;
               

            });

            /// Draw Upper blocks
            UpperBlocksPooler.ActionForeachActive(block => {

                Color blockColor = Color.White;

                //SetTexture
                effect.Parameters["Texture"].SetValue(blocksLibrary.GetBlockTexture(block.Id));
                effect.CurrentTechnique.Passes[0].Apply();

                DrawBlock(graphicsDevice, block, blockColor);

                DrawFrameForBlock(graphicsDevice, block, Color.LightGray);

            });

            //Save texture

            if (Texture == null)
                Texture = VideoManager.CopyTexture(graphicsDevice, target);
            else VideoManager.CopyTexture(Texture, target);

            IsChanged = false;
            InQueueToUpdate = false;

            //if (firstUpdate)
            //{
            //    IsChanged = true;
            //    firstUpdate = false;
            //}

        }


        private void DrawFrameForBlock(GraphicsDevice graphicsDevice, Block block, Color color)
        {
            Point blockPos = block.PositionInChunk;


            RectangleF blockBounds = block.Bounds;


            // Up
            if (blockPos.Y > 0)
            {
                if (BlocksTowers[blockPos.X, blockPos.Y - 1].Upper.Id == Block.BlockId.Void)
                    VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.RightTop, blockBounds.LeftTop, color, color);
            }
           // else VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.RightTop, blockBounds.LeftTop, color, color);

            //Down
            if (blockPos.Y < ChunkParameters.ChunkCellsSize.Y - 1)
            {
                if (BlocksTowers[blockPos.X, blockPos.Y + 1].Upper.Id == Block.BlockId.Void)
                    VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.LeftBot, blockBounds.RightBot, color, color);
            }
           // else VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.LeftBot, blockBounds.RightBot, color, color);

            //Left
            if (blockPos.X > 0)
            {
                if (BlocksTowers[blockPos.X - 1, blockPos.Y].Upper.Id == Block.BlockId.Void)
                    VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.LeftTop, blockBounds.LeftBot, color, color);
            }
            //else VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.LeftTop, blockBounds.LeftBot, color, color);

            //Right
            if (blockPos.X < ChunkParameters.ChunkCellsSize.X - 1 )
            {
                if (BlocksTowers[blockPos.X + 1, blockPos.Y].Upper.Id == Block.BlockId.Void)
                    VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.RightBot, blockBounds.RightTop, color, color);
            }
            //else VideoManager.DrawLine(graphicsDevice, EffectsPack.ColorEffect, blockBounds.RightBot, blockBounds.RightTop, color, color);

            



        }

        private void DrawBlock(GraphicsDevice graphicsDevice, Block block, Color color)
        {
            RectangleF bounds = new RectangleF(Vector2.Zero, Vector2.One);

            Vector2[] texBounds = new Vector2[4];

            //Set block rotation
            if (block.Variant == 1)
            {
                texBounds[0] = bounds.LeftBot;
                texBounds[1] = bounds.LeftTop;
                texBounds[2] = bounds.RightTop;
                texBounds[3] = bounds.RightBot;
            }
            else if (block.Variant == 2)
            {
                texBounds[0] = bounds.RightBot;
                texBounds[1] = bounds.LeftBot;
                texBounds[2] = bounds.LeftTop;
                texBounds[3] = bounds.RightTop;

            }
            else if (block.Variant == 3)
            {

                texBounds[0] = bounds.RightTop;
                texBounds[1] = bounds.RightBot;
                texBounds[2] = bounds.LeftBot;
                texBounds[3] = bounds.LeftTop;

            }
            else //block.Variant == 0 or else
            {
                texBounds[0] = bounds.LeftTop;
                texBounds[1] = bounds.RightTop;
                texBounds[2] = bounds.RightBot;
                texBounds[3] = bounds.LeftBot;
            }

            RectangleF blockBounds = block.Bounds;

            VideoManager.DrawTextureRectanglePrimitive(graphicsDevice,
                blockBounds.LeftTop,
                blockBounds.RightTop,
                blockBounds.RightBot,
                blockBounds.LeftBot,
                texBounds[0],
                texBounds[1],
                texBounds[2],
                texBounds[3],
                color
            );
        }
        //TEST

        public void Draw(VideoManager videoManager, BlocksLibrary blocksLibrary, Camera camera)
        {


            Effect effect = EffectsPack.ColorEffect;
            /*
            //Translate cell map coords to texture coords
            Matrix matrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) * Matrix.CreateScale(1 / (ChunkParameters.ChunkCellsSize.X * ChunkParameters.BlockSize.X), 1 / (ChunkParameters.ChunkCellsSize.Y * ChunkParameters.BlockSize.Y), 1);

            Matrix matrix2 = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateScale(2f, -2f, 1);
            Matrix worldMatrix = matrix * matrix2;
            */

            Matrix worldMatrix = camera.CameraMatrix;
            //apply current pass

            //effect.Parameters["WorldViewProjection"].SetValue(matrix * matrix2);
            //EffectsPack.ColorEffect.Parameters["WorldViewProjection"].SetValue(matrix * matrix2);

            //effect.CurrentTechnique.Passes[0].Apply();


            LowerBlocksPooler.ActionForeachActive(block => {

                //check if there is block above
                if (BlocksTowers[block.PositionInChunk.X, block.PositionInChunk.Y].Upper.Id != Block.BlockId.Void)
                    return;



                var blockTemplate = blocksLibrary.GetBlockTemplate(block.Id);

                Vector2 pos = block.Bounds.Center;

                Matrix localMatrix = Matrix.CreateTranslation(pos.X, pos.Y, 0);

                blockTemplate.Draw(videoManager, localMatrix * worldMatrix);
                //Set Matrix
                //Matrix localMatrix = Matrix.CreateTranslation()
            });

            
        }

        internal void Draw2(VideoManager videoManager, BlocksLibrary blocksLibrary, Camera camera)
        {

            EffectsPack.SetMatrix(EffectsPack.TextureEffect, camera.CameraMatrix);

            LowerBlocksPooler.ActionForeachActive(block => {

                //check if there is block above
                if (BlocksTowers[block.PositionInChunk.X, block.PositionInChunk.Y].Upper.Id != Block.BlockId.Void)
                    return;



                var blockTemplate = blocksLibrary.GetBlockTemplate(block.Id);

                Vector2 pos = block.Bounds.Center;

                Color blockColor = new Color(0.5f, 0.5f, 0.5f);

                //SetTexture
                EffectsPack.SetTextureEffect(blocksLibrary.GetBlockTexture(block.Id));

                EffectsPack.Apply(EffectsPack.TextureEffect);

                DrawBlock(videoManager.GraphicsDevice, block, blockColor);
                //Set Matrix
                //Matrix localMatrix = Matrix.CreateTranslation()
            });
        }

        ///
        public void SetLowerBlock(Block.BlockId id, Point pos, short variant = 0)
        {
            Block block = BlocksTowers[pos.X, pos.Y].Lower;

            if (id == Block.BlockId.Void)
            {
                ClearLowerBlock(block);
                return;
            }

            if (block.Id == Block.BlockId.Void)
            {
                block.Id = id;
                block.Variant = variant;

                LowerBlocksPooler.Add(block);
            }
            else
            {
                block.Id = id;
                block.Variant = variant;
            }
        }

        private void ClearLowerBlock(Block block)
        {
            if (block.Id != Block.BlockId.Void)
            {
                LowerBlocksPooler.Remove(block);
                block.Id = Block.BlockId.Void;
            }
        }

        public void SetUpperBlock(Block.BlockId id, Point pos, short variant = 0)
        {
            Block block = BlocksTowers[pos.X, pos.Y].Upper;

            if (id == Block.BlockId.Void)
            {
                ClearUpperBlock(block);
                return;
            }

            if (block.Id == Block.BlockId.Void)
            {
                block.Id = id;
                block.Variant = variant;

                UpperBlocksPooler.Add(block);
            }

            else
            {
                block.Id = id;
                block.Variant = variant;
            }
        }
        private void ClearUpperBlock(Block block)
        {
            if (block.Id != Block.BlockId.Void)
            {
                UpperBlocksPooler.Remove(block);
                block.Id = Block.BlockId.Void;
            }
        }
        public void SetBlock(Block.BlockId id, Point pos, short variant = 0, bool lower = false)
        {
            if (lower)
                SetLowerBlock(id, pos, variant);
            else SetUpperBlock(id, pos, variant);
        }

        internal BlocksTower GetBlocksTower(int x, int y) => BlocksTowers[x, y];
        internal BlocksTower GetBlocksTower(Point pos) => GetBlocksTower(pos.X, pos.Y);

        public void Changed()
        {
            IsChanged = true;
        }

        public void Dispose()
        {
            if (Texture != null)
                Texture.Dispose();
        }
    }
}
