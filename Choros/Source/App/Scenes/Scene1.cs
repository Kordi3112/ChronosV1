using Choros.Source.Creation;
using Choros.Source.Resource;
using Engine.Core.Scene;
using Engine.EngineMath;
using Engine.Graphics.Light;
using Engine.World;
using Engine.World.Board;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.App.Scenes
{
    public class Scene1 : GameScene
    {
        ResourcePackManager ResourcePackManager { get; set; }
        public Scene1(ResourcePackManager resourcePackManager)
        {
            ResourcePackManager = resourcePackManager;
        }

        public override void Init()
        {
            Identify("Scene1", 1);
        }

        public override void Load()
        {
            GameManager.UsedWorldManager = WorldManager;

            WorldManager.ObjectsPooler.Clear();
            
            WorldManager.ObjectsPooler.Add(new TestObject1(ResourcePackManager));
            WorldManager.ObjectsPooler.Add(new TestObject2(ResourcePackManager, new Transform(new Vector2(0,200), -MathHelper.Pi / 4)));
            WorldManager.ObjectsPooler.Add(new TestObject2(ResourcePackManager, new Transform(new Vector2(0,400), -MathHelper.Pi / 2)));
            
        }

        

        public override void Close()
        {
            WorldManager.Restart();
        }

        public override void Dispose()
        {
            WorldManager.Dispose();

        }

        public override void Update()
        {
            CameraMovement();

            //WorldManager.LightManager.Add(new CircleLatern(new Transform(new Vector2(-300, 300)), 2000, Color.Cyan, true, true));
            //WorldManager.LightManager.Add(new CircleLatern(new Transform(new Vector2(1200, -500)), 2000, Color.Blue, true, true));

           // WorldManager.LightManager.Add(new CircleLatern(new Transform(new Vector2(-300, 300)), 2000, Color.Cyan, true, true));
           // WorldManager.LightManager.Add(new CircleLatern(new Transform(new Vector2(1200, -500)), 2000, Color.Blue, true, true));
           // WorldManager.LightManager.Add(new CircleLatern(new Transform(new Vector2(500,1300)), 2000, Color.Pink, true, true));


            //WorldManager.LightManager.Add(new CircleLatern(new Transform(WorldManager.Camera.Transform.Position2), 2000, Color.Pink, true, true));




            WorldManager.Update();

            if (!GameManager.IsActivated)
                return;

            Vector2 mousePos = WorldManager.Camera.MousePositionToWorld(GameManager.VideoManager.ViewSize, GameManager.Input);

            
            BlockInfo blockInfo = WorldManager.Map.CurrentDimension.GetBlockInfo(mousePos);
            Random random = new Random();

            if (GameManager.Input.Mouse.IsLeftButtonOnClick())
            {

                CircleDimensionBrush circleDimensionBrush = new CircleDimensionBrush();
                circleDimensionBrush.MaxOffsetFromCenter = new Point(5,5);

                WorldManager.Map.CurrentDimension.SetBlocks(circleDimensionBrush, mousePos);
            }

            /*
            if (GameManager.Input.Mouse.IsRightButtonOnClick())
            {
                blockInfo.Lower = true;
                Random random = new Random();

                WorldManager.Map.CurrentDimension.SetBlock(blockInfo, (Block.BlockId)random.Next(0, 5), 0);
                WorldManager.Map.CurrentDimension.SetChunkInChange(blockInfo);


            }
            */
            if (GameManager.Input.Mouse.IsRightButtonOnClick())
            {
                BlockInfo blockInfo1 = WorldManager.Map.CurrentDimension.GetBlockInfo(new Vector2(2, -2), mousePos);



                /*
                WorldManager.Map.CurrentDimension.SetBlock(blockInfo1, (Block.BlockId)random.Next(0, 5), 0);
                WorldManager.Map.CurrentDimension.SetChunkInChange(blockInfo1);
                */

                WorldManager.Map.CurrentDimension.SetBlock(blockInfo1, Block.BlockId.Void, 0);
                WorldManager.Map.CurrentDimension.SetChunkInChange(blockInfo1);
            }



        }

        public override void Draw()
        {
            WorldManager.Draw();
        }

        private void CameraMovement()
        {
            if (GameManager.CommandPanel.IsActive)
                return;

            if (GameManager.Input.Keys.ButtonOnClick(Keys.R))
                GameManager.SceneManager.CurrentScene.Restart();

            float speed = 250;

            Camera camera = WorldManager.Camera;



            if (GameManager.Input.Keys.IsKeyDown(Keys.W))
            {
                camera.Zoom *= (1 + 1f * GameManager.Time.RealDeltaTime);
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.S))
            {
                camera.Zoom *= (1 - 1f * GameManager.Time.RealDeltaTime);
            }


            if (GameManager.Input.Keys.IsKeyDown(Keys.Left))
            {
                camera.Transform.X -= speed * GameManager.Time.RealDeltaTime / camera.Zoom;
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.Right))
            {
                camera.Transform.X += speed * GameManager.Time.RealDeltaTime / camera.Zoom;
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.Up))
            {
                camera.Transform.Y -= speed * GameManager.Time.RealDeltaTime / camera.Zoom;
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.Down))
            {
                camera.Transform.Y += speed * GameManager.Time.RealDeltaTime / camera.Zoom;
            }
        }


    }
}
