using Engine.Resource;
using Engine.World.Board;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class Map : IDisposable
    {
        List<Dimension> _dimensions;

        public Dimension CurrentDimension { get; private set; }

        public BlocksLibrary BlocksLibrary { get; private set; }

        public bool IsActive { get; set; }

        WorldManager WorldManager { get; set; }

        public Map(WorldManager worldManager)
        {
            BlocksLibrary = new BlocksLibrary(10);
            _dimensions = new List<Dimension>();

            WorldManager = worldManager;
        }

        public void LoadBlockLibrary(ContentManager content)
        {
            BlocksLibrary.DefineBlock(Block.BlockId.Red, "Red");
            BlocksLibrary.DefineBlock(Block.BlockId.Orange, "Orange");
            BlocksLibrary.DefineBlock(Block.BlockId.Yellow, "Yellow");
            BlocksLibrary.DefineBlock(Block.BlockId.Green, "Green");
            BlocksLibrary.DefineBlock(Block.BlockId.Blue, "Blue");
            BlocksLibrary.DefineBlock(Block.BlockId.Purple, "Purple");

            BlocksLibrary.Load("TexturePacks/TestPack1/", content);
        }

        public void AddDimension(Dimension dimension)
        {
            _dimensions.Add(dimension);
        }

        public void SetDimensionById(int id)
        {
            foreach (Dimension dimension in _dimensions)
                if (dimension.Id == id)
                    CurrentDimension = dimension;
        }

        public void Update()
        {
            if (!IsActive)
                return;

            CurrentDimension.Update();
            
        }

        public void Draw(RenderTarget2D renderTarget)
        {
            if (!IsActive)
                return;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            CurrentDimension.UpdateChunks(BlocksLibrary);

            //Debug.WriteLine("Visual1: " + stopwatch.Elapsed.TotalMilliseconds);
            

            //Repair effects texture matrix
            EffectsPack.SetMatrix(EffectsPack.TextureEffect, WorldManager.Camera.CameraMatrix);

            //Repair effects color matrix
            EffectsPack.SetMatrix(EffectsPack.ColorEffect, WorldManager.Camera.CameraMatrix);

            stopwatch.Restart();
            CurrentDimension.Draw(renderTarget);
            //CurrentDimension.Draw3(renderTarget, BlocksLibrary);
            //Debug.WriteLine("Visual2: " + stopwatch.Elapsed.TotalMilliseconds);
        }

        public void Dispose()
        {
            foreach (Dimension dimension in _dimensions)
                dimension.Dispose();
        }
    }
}
