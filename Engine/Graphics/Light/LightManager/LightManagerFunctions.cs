using Engine.EngineMath;
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
    //Less important functionts
    public partial class LightManager
    {

        private void InitDefaultTexture()
        {
            DefaultCircleLaternRadius = 100;
        
            DefaultCircleLaternTexture = CreateCircle(VideoManager.GraphicsDevice, DefaultCircleLaternRadius, Color.White);
        }

        private void UpdateSceneSize()
        {
            LightMapTexture = new RenderTarget2D(VideoManager.GraphicsDevice, SceneSize.X, SceneSize.Y);

            foreach (RenderTarget2D renderTarget in ShadowsMapTextures)
                renderTarget.Dispose();

            foreach (RenderTarget2D renderTarget in LaternMapTextures)
                renderTarget.Dispose();

            ShadowsMapTextures.Clear();
            LaternMapTextures.Clear();
        }

        /// <summary>
        /// Calculate rays for ShadowLatern[id]
        /// </summary>
        private void CalculateLatern(int id)
        {
            Latern latern = ShadowLaterns[id];
            List<RayLine> rayLines = _rays[id];

            rayLines.Clear();

            foreach (Polygon polygon in Polygons)
            {
                foreach (Vector2 vertex in polygon.Vertices)
                {
                    RayLine basicRay = new RayLine(latern.Position2, vertex);

                    rayLines.Add(RayLine.OffsetRay(basicRay, -_raysAngleOffset, -_raysAngleSin, _raysAngleCos));

                    if(UserSettings.VideoSettings.UseMoreRaysInLighting)
                        rayLines.Add(basicRay);

                    rayLines.Add(RayLine.OffsetRay(basicRay, +_raysAngleOffset, _raysAngleSin, _raysAngleCos));

                }
            }

            //cast additional rays to create triangles where there is no polygons
            
            //                                                      sqrt(2) + 0.01
            rayLines.Add(new RayLine(latern.Position2, latern.Position2 + 1.42f * latern.Range * new Vector2(-1, 0), 1));
            rayLines.Add(new RayLine(latern.Position2, latern.Position2 + 1.42f * latern.Range * new Vector2(0, -1), 1));
            rayLines.Add(new RayLine(latern.Position2, latern.Position2 + 1.42f * latern.Range * new Vector2(1, 0), 1));
            rayLines.Add(new RayLine(latern.Position2, latern.Position2 + 1.42f * latern.Range * new Vector2(0, 1), 1));
            


            if (rayLines.Count > 100 && UserSettings.VideoSettings.ThreadingLevel > 1)
            {
                ThreadingRayCalculates(5, rayLines, id);
            }
            else
            {
                CalculateRays(0, rayLines.Count, ref rayLines, id);
            }

            //Sort
            rayLines.Sort(new RaysAngleComparer());


        }

        private void ThreadingRayCalculates(int threadsCount, List<RayLine> lines, int id)
        {
            int ration = lines.Count / (threadsCount + 1);

            Thread[] threads = new Thread[threadsCount];

            for (int i = 0; i < threadsCount; i++)
            {
                int a = i;

                threads[a] = new Thread(x => {
                    CalculateRays(a * ration, (a + 1) * ration, ref lines, id);
                });
                threads[a].Start();
            }

            CalculateRays(threadsCount * ration, lines.Count, ref lines, id);

            for (int i = 0; i < threadsCount; i++)
                threads[i].Join();
        }

        private void CalculateRays(int left, int right, ref List<RayLine> lines, int id)
        {
            for (int i = left; i < right; i++)
            {
                RayLine ray = lines[i];

                for (int j = 0; j < Polygons.Count; j++)
                {
                    Polygon polygon = Polygons[j];

                    //draw if polygon is in this latern range
                    if (polygon.LightsIdsInRange.Contains(id))
                        ray.Calculate(polygon);
                }

                ray.RayPoint = ray.Line.GetPoint(ray.MinT);
            }
        }

        private void CheckCreatedRenderTargets()
        {
            while (ShadowsMapTextures.Count < ShadowLaternsCount)
            {

                ShadowsMapTextures.Add(new RenderTarget2D(VideoManager.GraphicsDevice, SceneSize.X, SceneSize.Y));
                LaternMapTextures.Add(new RenderTarget2D(VideoManager.GraphicsDevice, SceneSize.X, SceneSize.Y));

            }
        }

        private void DrawLaternTexture(Latern latern)
        {
            if (latern.GetType() == Latern.Type.Circle)
            {
                Vector2 textureSize = DefaultCircleLaternTexture.Bounds.Size.ToVector2();

                #pragma warning disable CS0618 // Type or member is obsolete
                VideoManager.SpriteBatch.Draw(DefaultCircleLaternTexture, position: latern.Position2, color: latern.Color, origin: textureSize * 0.5f, scale: new Vector2(latern.Range / DefaultCircleLaternRadius));
                #pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        Texture2D CreateCircle(GraphicsDevice graphics, float radius, Color color)
        {

            float R2 = radius * radius;

            int size = 2 * ((int)radius + 20);

            Texture2D texture = new Texture2D(graphics, size, size);

            Color[] data = new Color[size * size];

            Color fixedColor = color;
            fixedColor.A = 1;


            for (int i = 0; i < size * size; i++)
            {
                Vector2 pos = new Vector2(i / size, i % size);
                //pos relative to texture center
                pos -= new Vector2(size * 0.5f);

                float d2 = pos.X * pos.X + pos.Y * pos.Y;


                

                if (d2 < R2)
                {
                    float d = (float)Math.Sqrt(d2);

                    float x = (float)Math.Sqrt(d / radius);
                    float value = Tools.HyperbolicValue(x, 2.0f);

                    data[i] = fixedColor * value;
                    data[i].A = 1;
                }

                else data[i] = Color.Transparent;
            }

            texture.SetData(data);

            return texture;
        }
    }
}
