using Engine.Core;
using Engine.EngineMath;
using Engine.Graphics;
using Engine.Graphics.Light;
using Engine.Resource;
using Engine.World.Board;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public partial class WorldManager : IDisposable
    {
        public GameManager GameManager { get; private set; }
        public Camera Camera { get; set; }
        public ObjectsPooler ObjectsPooler { get; private set; }
        public LightManager LightManager { get; private set; }
        public FogManager FogManager { get; private set; }
        public Map Map { get; set; }

        ObjectsController ObjectsController { get; set; }

        RenderTarget2D mapRenderTarget;

        int _skipFramesCounter = 1;

        bool FrameToSkip { get; set; }

        public WorldManager(GameManager gameManager)
        {
            GameManager = gameManager;

            //set ObjectsController
            ObjectsController = new ObjectsController1(this);

            //create Poller
            ObjectsPooler = new ObjectsPooler(this, 1);

            //Init Camera
            Camera = new Camera(); //Zoom = 1;
            Camera.Zoom = 1.0f;
            //LightManager 
            LightManager = new LightManager(this);
            LightManager.IsLightingActive =true;
            //Fog
            FogManager = new FogManager(this)
            {
                Effect = EffectsPack.FogWave,
                Color = Color.White,
                BorderLines = true,
                Scale = new Vector2(0.01f),
                IsActive = true
            };

            FogManager.Init();

            //Render
            mapRenderTarget = GameManager.VideoManager.CreateRenderTargetPreserveContents();
            //mapRenderTarget = GameManager.VideoManager.CreateRenderTarget();

            Map = new Map(this);
            Map.IsActive = true;
            Map.LoadBlockLibrary(GameManager.Mono.Content);

            Dimension dimension1 = new Dimension(this, Point.Zero, new TerraFormer2(1000), Quarter.QuarterAlign.All, 0);

            Map.AddDimension(dimension1);
            Map.SetDimensionById(0);


        }



        public void Update()
        {
            UpdateSkipFrames();

            if (FrameToSkip)
                return;

            UpdateCamera();

            LightManager.Clear();

            
            Map.Update();

            ObjectsController.UpdateObjects(ObjectsPooler, Map);   // Calculate RigidBody Parameters


            LightManager.AddLightsFromQueue();

            LightManager.UpdateLightBounds();

            UpdateLightApperanceInfo();
            //UpdateObjectsApperanceInfo3();

            UpdateObjectsModels();

            AddPolygonsToLightManager();

            LightManager.CalculateRays();
            
        }


        public void Draw()
        {
            GraphicsDevice graphicsDevice = GameManager.VideoManager.GraphicsDevice;

            //Map Load Chunks

            GameManager.VideoManager.SetRenderTarget(mapRenderTarget);
            //GameManager.VideoManager.ClearTarget(Color.Transparent);
            GameManager.VideoManager.ClearTarget(Color.Transparent);

            //Draw Fog
            FogManager.Draw(mapRenderTarget, 0.02f * GameManager.Time.TotalTime);

            Map.Draw(mapRenderTarget);

            DrawObjects();

            Camera.SetEffectMatrix(EffectsPack.ColorEffect);
            Camera.SetEffectMatrix(EffectsPack.TextureEffect);

            DrawFinal(); // combine Map and Lights

            DebugRender();

            Vector2 mousePos = Camera.MousePositionToWorld(GameManager.VideoManager.ViewSize, GameManager.Input);
            //GameManager.VideoManager.DrawLine(new Vector2(2, -2), mousePos, Color.Green, Color.Green);

        }

        private void DebugRender()
        {
            if (GameManager.UserSettings.DebugSettings.DebugHitbox)
                DrawDebugHitbox();

            if (GameManager.UserSettings.DebugSettings.DebugHitboxBound)
                DrawDebugHitboxBound();

            if (GameManager.UserSettings.DebugSettings.DebugShadowPolygon)
                DrawDebugShadowPolygon();

            if (GameManager.UserSettings.DebugSettings.DebugLightRays)
                DrawRayLines();

            if (GameManager.UserSettings.DebugSettings.DebugDrawBlocksGrid)
                DrawBlocksGrid();

            if (GameManager.UserSettings.DebugSettings.DebugDrawChunkLines)
                DrawChunkLines();

            //

            EffectsPack.Apply(EffectsPack.ColorEffect);

            GameManager.VideoManager.DrawRectangleFrame(RectangleF.CreateFromCenter((ObjectsController as ObjectsController1).closestOnA, new Vector2(3, 3)), Color.Pink);
            GameManager.VideoManager.DrawRectangleFrame(RectangleF.CreateFromCenter((ObjectsController as ObjectsController1).closestOnB, new Vector2(2, 2)), Color.Brown);

            

            GameManager.VideoManager.DrawLine((ObjectsController as ObjectsController1).closestOnA, (ObjectsController as ObjectsController1).closestOnA + (ObjectsController as ObjectsController1).Normal, Color.Red, Color.Green);

            //To test

            //Vector2 normal = new Vector2(20, -50);
            Vector2 normal = 50 * Tools.AngleToNormal(0.6f + 0.1f * GameManager.Time.TotalTime);
            GameManager.VideoManager.DrawLine(Vector2.Zero, normal, Color.Red, Color.Green);

            Vector2 inputVector = new Vector2(20, 30);
            GameManager.VideoManager.DrawLine(Vector2.Zero - inputVector, Vector2.Zero, Color.Black, Color.White);

            if (Vector2.Dot(inputVector, normal) >= 0)
                return;

            normal.Normalize();

            float cosAlpha = normal.X;
            float sinAlpha = normal.Y;

            inputVector *= -1;

            inputVector = Tools.Rotate(inputVector, -sinAlpha, cosAlpha);

            inputVector.Y = -inputVector.Y;
            //rotate -Alpha
            //inputVector = Tools.Rotate(inputVector, initAlpha);

            inputVector = Tools.Rotate(inputVector, sinAlpha, cosAlpha);

           
           GameManager.VideoManager.DrawLine(Vector2.Zero, inputVector, Color.Blue, Color.Red);

            //if (Vector2.Dot(inputVector, normal) >= 0)
            //   return;

            inputVector *= -1;

            inputVector = Tools.Rotate(inputVector, -sinAlpha, cosAlpha);

            inputVector.Y = -inputVector.Y;
            //rotate -Alpha
            //inputVector = Tools.Rotate(inputVector, initAlpha);

            inputVector = Tools.Rotate(inputVector, sinAlpha, cosAlpha);

            GameManager.VideoManager.DrawLine(Vector2.Zero, inputVector, Color.Yellow, Color.Pink);

        }


        private void DrawFinal()
        {

            if (LightManager.IsLightingActive)
                DrawFinalWithLight();
            else DrawFinalWithoutLight();

        }

        public void Restart()
        {
            LightManager.Clear();

            Camera = new Camera
            {
                Zoom = 1,
            };

            ObjectsPooler.Clear();

            _skipFramesCounter = 1;
        }

        public void Dispose()
        {
            Map.Dispose();
            mapRenderTarget.Dispose();
            LightManager.Dispose();
        }
    }
}
