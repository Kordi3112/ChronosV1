using Engine.Core;
using Engine.EngineMath;
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
    public partial class Dimension : IDisposable
    {
        public int Id { get; set; }
        public TerraFormer TerraFormer { get; private set; }

        internal List<Quarter> Quarters { get; set; }


        internal WorldManager WorldManager { get; private set; }
        internal VideoManager VideoManager => WorldManager.GameManager.VideoManager;
        internal GraphicsDevice GraphicsDevice => WorldManager.GameManager.VideoManager.GraphicsDevice;
        internal RenderTarget2D ChunkDrawTarget { get; private set; }


        public Dimension(WorldManager worldManager, Point maxChunksCapacity, TerraFormer terraFormer, Quarter.QuarterAlign align, int id)
        {
            Id = id;
            TerraFormer = terraFormer;
            WorldManager = worldManager;

            CreateQuarters(align);

            int scale = WorldManager.GameManager.UserSettings.VideoSettings.ChunkTextureQuality;
            ChunkDrawTarget = new RenderTarget2D(GraphicsDevice, scale * (int)ChunkParameters.ChunkSize.X, scale * (int)ChunkParameters.ChunkSize.Y);


            foreach (Quarter quarter in Quarters)
                quarter.Init(maxChunksCapacity);
        }

        private void CreateQuarters(Quarter.QuarterAlign align)
        {
            Quarters = new List<Quarter>();

            if (align.HasFlag(Quarter.QuarterAlign.RightBot))
            {
                Quarters.Add(new Quarter(Quarter.QuarterAlign.RightBot, this));
            }

            if (align.HasFlag(Quarter.QuarterAlign.RightTop))
            {
                Quarters.Add(new Quarter(Quarter.QuarterAlign.RightTop, this));
            }

            if (align.HasFlag(Quarter.QuarterAlign.LeftTop))
            {
                Quarters.Add(new Quarter(Quarter.QuarterAlign.LeftTop, this));
            }

            if (align.HasFlag(Quarter.QuarterAlign.LeftBot))
            {
                Quarters.Add(new Quarter(Quarter.QuarterAlign.LeftBot, this));
            }
        }

        internal void Update()
        {
            Camera camera = WorldManager.Camera;

            Vector2 sizeDiff = ChunkParameters.ChunkSize;
            RectangleF extendedViewBounds = new RectangleF(camera.ViewBounds.LeftTop - 0.5f * sizeDiff, camera.ViewSize + sizeDiff);

            foreach (Quarter quarter in Quarters)
                quarter.UpdateVisibleArea(extendedViewBounds);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            CheckChunks();

            //Debug.WriteLine("Generate: " + stopwatch.Elapsed.TotalMilliseconds);
        }
        /// <summary>
        /// Generating chunks
        /// </summary>
        internal void CheckChunks()
        {
            foreach (Quarter quarter in Quarters)
                quarter.CheckChunks();

        }
        /// <summary>
        /// Update chunk Texture
        /// </summary>
        internal void UpdateChunks(BlocksLibrary blocksLibrary)
        {
            foreach (Quarter quarter in Quarters)
                quarter.UpdateVisibleChunks(blocksLibrary);

        }

        internal void Draw(RenderTarget2D renderTarget)
        {
            foreach (Quarter quarter in Quarters)
                quarter.Draw(renderTarget);
        }

        internal void Draw2(RenderTarget2D renderTarget, BlocksLibrary blocksLibrary)
        {
            foreach (Quarter quarter in Quarters)
                quarter.Draw2(renderTarget, blocksLibrary);

        }

        internal void Draw3(RenderTarget2D renderTarget, BlocksLibrary blocksLibrary)
        {
            foreach (Quarter quarter in Quarters)
                quarter.Draw3(renderTarget, blocksLibrary);

        }

        public void Dispose()
        {
            //dispose all quarters

            foreach (Quarter quarter in Quarters)
                quarter.Dispose();
        }
    }
}
