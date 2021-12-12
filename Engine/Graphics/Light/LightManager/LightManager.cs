using Engine.EngineMath;
using Engine.Resource;
using Engine.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Graphics.Light
{
    public partial class LightManager : IDisposable
    {
        internal LightManager(WorldManager worldManager)
        {
            WorldManager = worldManager;

            ShadowLaterns = new List<Latern>();
            CasualLaterns = new List<Latern>();

            ShadowsMapTextures = new List<RenderTarget2D>();
            LaternMapTextures = new List<RenderTarget2D>();

            Polygons = new List<Polygon>();

            SceneSize = VideoManager.ViewSize.ToPoint();

            UpdateSceneSize();

            InitDefaultTexture();

            LightBounds = new List<RectangleF>();

            _rays = new List<List<RayLine>>();

            laternsQueue = new Queue<Latern>();

            ///
            _raysAngleSin = (float)Math.Sin(_raysAngleOffset);
            _raysAngleCos = (float)Math.Cos(_raysAngleOffset);
        }

        internal void UpdateLightBounds()
        {
            LightBounds.Clear();

            foreach (Latern latern in ShadowLaterns)
            {
                if (!latern.Bounds.Intersect(Camera.ViewBounds))
                    ShadowLaterns.Remove(latern);
            }
            foreach (Latern latern in ShadowLaterns)
            {

                if(Camera.ViewBounds.Contains(latern.Bounds.Center))
                {
                    LightBounds.Add(RectangleF.CommonPart(latern.Bounds, Camera.ViewBounds));
                    continue;
                }

                Vector2 size = 2 * Tools.Abs(latern.Bounds.Center - Camera.Transform.Position2);

                //fix
                if (size.X < Camera.ViewSize.X)
                    size.X = Camera.ViewSize.X;

                if (size.Y < Camera.ViewSize.Y)
                    size.Y = Camera.ViewSize.Y;

                RectangleF extendedViewRange = RectangleF.CreateFromCenter(Camera.Transform.Position2, size);

                LightBounds.Add(RectangleF.CommonPart(latern.Bounds, extendedViewRange));

            }

        }
        public void Add(Latern latern)
        {
            if (!latern.IsActive)
                return;

            if (directAddingLight)
            {
                //check if light is in view range
                if (!Camera.ViewBounds.Intersect(latern.Bounds))
                    return;

                if (latern.IsShadowCaster)
                    ShadowLaterns.Add(latern);
                else
                    CasualLaterns.Add(latern);
            }
            else laternsQueue.Enqueue(latern);

        }
        internal void AddShadowPolygon(Polygon polygon)
        {
            Polygons.Add(polygon);
        }
        internal void AddShadowPolygons(List<Polygon> polygons)
        {
            foreach (Polygon polygon in polygons)
                Polygons.Add(polygon);
        }
        internal void Clear()
        {
            CasualLaterns.Clear();

            ShadowLaterns.Clear();

            Polygons.Clear();

            //after clear u can adding lights without using queue
            directAddingLight = true;
        }

        internal void AddLightsFromQueue()
        {
            //Add queue lights
            for (int i = 0; i < laternsQueue.Count; i++)
                Add(laternsQueue.Dequeue());

            //after clear u can adding lights without using queue
            directAddingLight = false;
        }

        /// <summary>
        /// Calculate rays before drawing
        /// </summary>
        internal void CalculateRays()
        {
            _rays.Clear();

            while (_rays.Count < ShadowLaternsCount)
            {
                _rays.Add(new List<RayLine>());
            }

            if (ShadowLaternsCount < 2 || UserSettings.VideoSettings.ThreadingLevel < 1)
            {
                for (int i = 0; i < ShadowLaternsCount; i++)
                {
                    CalculateLatern(i);
                }
            }
            else //use threads
            {
                Thread[] threads = new Thread[ShadowLaternsCount - 1];

                for (int i = 0; i < threads.Length; i++)
                {
                    int p = i; //to avoid memory issues

                    threads[p] = new Thread(x => {
                        CalculateLatern(p);
                    });

                    threads[p].Start();
                }

                CalculateLatern(ShadowLaternsCount - 1);

                for (int i = 0; i < threads.Length; i++)
                    threads[i].Join();
            }
        }


        public RenderTarget2D Render()
        {
            CheckCreatedRenderTargets();

            for (int i = 0; i < ShadowLaternsCount; i++)
            {
                Latern latern = ShadowLaterns[i];
                List<RayLine> rays = _rays[i];

                //DRAW RAYS
                VideoManager.SetRenderTarget(ShadowsMapTextures[i]);

                VideoManager.ClearTarget(Color.Transparent);

                EffectsPack.Apply(EffectsPack.ColorEffect); //Shadows are triangles filled with color

                RayLine.DrawLights(VideoManager.GraphicsDevice, rays, new Color(1.0f, 1.0f, 1.0f, 0.5f));

                //DRAW LATERN MAP - 
                VideoManager.SetRenderTarget(LaternMapTextures[i]);
                VideoManager.ClearTarget(Color.Transparent);

                VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate, effect: EffectsPack.TextureEffect, blendState: BlendState.AlphaBlend); // EffectsPack.TextureEffect - drawing textures on render target
                DrawLaternTexture(latern);
                VideoManager.SpriteBatch.End();
            }

            //COMBINE SHADOWS WITH LATERNS TEXTURES

            
            VideoManager.SetRenderTarget(LightMapTexture);
            VideoManager.ClearTarget(Color.Transparent);

            VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: EffectsPack.Combine2);

            for (int i = 0; i < ShadowLaternsCount; i++)
            {
                EffectsPack.Combine2.Parameters["colorMap"].SetValue(ShadowsMapTextures[i]);
                VideoManager.SpriteBatch.Draw(LaternMapTextures[i], Vector2.Zero, Color.White);
            }

            VideoManager.SpriteBatch.End();

            
            //DRAW CASUAL LATERNS
            VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate, effect: EffectsPack.TextureEffect, blendState: BlendState.AlphaBlend); // EffectsPack.TextureEffect - drawing textures on render target

            for (int i = 0; i < CasualLaternsCount; i++)
            {
                DrawLaternTexture(CasualLaterns[i]);
            }

            VideoManager.SpriteBatch.End();
            
            

            return LightMapTexture;
        }

        public void Dispose()
        {
            LightMapTexture.Dispose();

            foreach (RenderTarget2D target in ShadowsMapTextures)
                target.Dispose();

            foreach (RenderTarget2D target in LaternMapTextures)
                target.Dispose();
        }
    }
}
