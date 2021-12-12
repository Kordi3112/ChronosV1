using Engine.EngineMath;
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
    public partial class WorldManager
    {
        private void UpdateSkipFrames()
        {
            if (GameManager.UserSettings.VideoSettings.SkipFrames == 0)
                return;

            if (_skipFramesCounter % GameManager.UserSettings.VideoSettings.SkipFrames == 0)
                FrameToSkip = true;
            else FrameToSkip = false;
        }


        private void UpdateCamera()
        {
            Camera.DefaultViewSize = GameManager.VideoManager.ViewSize;

            Camera.Update();

            //update effects
            Camera.SetEffectMatrix(EffectsPack.ColorEffect);
            Camera.SetEffectMatrix(EffectsPack.TextureEffect);
        }


        
        private void UpdateObjectsApperanceInfo3()
        {
            ObjectsPooler.ActionForeachActive(element =>
            {
                ObjectApperanceInfo info = element.ObjectApperanceInfo;
                info.IsInLightView = false;
                info.IsInCameraView = false;

                if (Camera.ViewBounds.Intersect(element.Hitbox.RealBounds))
                    info.IsInCameraView = true;

                if (!element.Model.HaveShadowPolygon())
                    return;

                if (AddObjectToLightsIdsInRange(element))
                    info.IsInLightView = true;

            });


        }

        private void UpdateLightApperanceInfo()
        {
                    
            ObjectsPooler.ActionForeachActive(element =>
            {
                element.ObjectApperanceInfo.IsInLightView = false;


                if (!element.Model.HaveShadowPolygon())
                    return;

                if (AddObjectToLightsIdsInRange(element))
                    element.ObjectApperanceInfo.IsInLightView = true;

            });

        }

        private bool AddObjectToLightsIdsInRange(PhysicalObject physicalObject)
        {
            bool inLightRange = false;

            for (int i = 0; i < physicalObject.Model.ComponentsCount; i++)
            {
                ModelComponent modelComponent = physicalObject.Model.GetModelComponent(i);

                foreach (Polygon polygon in modelComponent.RealShadowPolygons)
                {
                    polygon.LightsIdsInRange.Clear();

                    for (int id = 0; id < LightManager.LightBounds.Count; id++)
                    {
                        if(LightManager.LightBounds[id].Intersect(physicalObject.Hitbox.RealBounds))
                        {
                            polygon.LightsIdsInRange.Add(id);
                            inLightRange = true;
                        }
                            
                    }
                }

            }

            return inLightRange;
        }

        private void AddPolygonsToLightManager()
        {
            ObjectsPooler.ActionForeachActive(element =>
            {
                if (element.ObjectApperanceInfo.IsInLightView)
                {
                    for (int i = 0; i < element.Model.ComponentsCount; i++)
                        LightManager.AddShadowPolygons(element.Model.GetModelComponent(i).RealShadowPolygons);

                }                   
            }
            );

            
        }

        private void UpdateObjectsModels()
        {
            ObjectsPooler.ActionForeachActive(element =>
            {
                if (!element.ObjectApperanceInfo.IsInLightView)
                    return;

                if(!element.Hitbox.LastTransform.Equals(element.Hitbox.PrevTransform)) //if are equal that means that transform is like before (no update is need)
                    element.Model.Update(element.Hitbox.LastTransform, Camera, element.ObjectApperanceInfo.IsInCameraView);

            });
        }

        private void DrawObjects()
        {
            ObjectsPooler.ActionForeachActive(element =>
            {
                if (!element.ObjectApperanceInfo.IsInCameraView)
                    return;

                element.Model.Draw(this);
            });
        }


        private void DrawFinalWithLight()
        {

            RenderTarget2D lightMap = LightManager.Render();
            
            EffectsPack.Combine1.Parameters["pixelSize"].SetValue(Vector2.One / mapRenderTarget.Bounds.Size.ToVector2());
            //EffectsPack.Combine1.Parameters["antyaliasingIsActive"].SetValue(GameManager.UserSettings.VideoSettings.Antyaliasing);
            if(GameManager.Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                EffectsPack.Combine1.Parameters["antyaliasingIsActive"].SetValue(true);
            else EffectsPack.Combine1.Parameters["antyaliasingIsActive"].SetValue(false);

            EffectsPack.Combine1.Parameters["colorMap"].SetValue(lightMap);
            /*
            EffectsPack.Combine1.Parameters["power1"].SetValue(1.0f);
            EffectsPack.Combine1.Parameters["power2"].SetValue(0.15f);
            EffectsPack.Combine1.Parameters["power3"].SetValue(0.05f);
            */
            float centerPower = 0.7f;
            float outsidePower = (1 - centerPower) / 8;
            EffectsPack.Combine1.Parameters["power1"].SetValue(centerPower);
            EffectsPack.Combine1.Parameters["power2"].SetValue(outsidePower);
            EffectsPack.Combine1.Parameters["power3"].SetValue(outsidePower);
            
            //EffectsPack.Apply(EffectsPack.Combine1);



            GameManager.VideoManager.SetFinalRenderTarget();
            GameManager.VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate, effect: EffectsPack.Combine1, blendState: BlendState.AlphaBlend);

            GameManager.VideoManager.SpriteBatch.Draw(mapRenderTarget, Vector2.Zero, Color.White);
            GameManager.VideoManager.SpriteBatch.End();

        }

        private void DrawFinalWithoutLight()
        {
            GameManager.VideoManager.SetFinalRenderTarget();
            GameManager.VideoManager.SpriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            GameManager.VideoManager.SpriteBatch.Draw(mapRenderTarget, Vector2.Zero, Color.White);
            GameManager.VideoManager.SpriteBatch.End();

        }

        private void DrawRayLines()
        {
            EffectsPack.Apply(EffectsPack.ColorEffect);

            foreach(List<RayLine> lineList in LightManager._rays)
            {
                foreach(RayLine line in lineList)
                {
                    line.Draw(GameManager.VideoManager.GraphicsDevice, GameManager.UserSettings.DebugSettings.DebugLightRaysColor);
                }
            }
        }


        private void DrawDebugHitboxBound()
        {
            EffectsPack.Apply(EffectsPack.ColorEffect);

            ObjectsPooler.ActionForeachActive(element =>
            {
                //element.Hitbox.
                GameManager.VideoManager.DrawRectangleFrame(element.Hitbox.RealBounds, GameManager.UserSettings.DebugSettings.DebugHitboxBoundColor);
            });
        }

        private void DrawDebugHitbox()
        {
            Color color = GameManager.UserSettings.DebugSettings.DebugHitboxColor;

            ObjectsPooler.ActionForeachActive(element =>
            {
                foreach (Polygon polygon in element.Hitbox.RealPolygons)
                {
                    for (int i = 0; i < polygon.Size; i++)
                    {
                       
                        GameManager.VideoManager.DrawLine(EffectsPack.ColorEffect, polygon.Vertices[i], polygon.Vertices[(i + 1) % polygon.Size], color, color);
                    }
                }

            });

                
        }

        private void DrawDebugShadowPolygon()
        {
            Color color = GameManager.UserSettings.DebugSettings.DebugShadowPolygonColor;

            EffectsPack.Apply(EffectsPack.ColorEffect);

            ObjectsPooler.ActionForeachActive(element =>
            {
                for (int i = 0; i < element.Model.ComponentsCount; i++)
                {
                    ModelComponent component = element.Model.GetModelComponent(i);

                    foreach (Polygon polygon in component.RealShadowPolygons)
                    {
                        for (int j = 0; j < polygon.Size; j++)
                        {

                            GameManager.VideoManager.DrawLine(polygon.Vertices[j], polygon.Vertices[(j + 1) % polygon.Size], color, color);
                        }
                    }
                }



            });
        }
        private void DrawBlocksGrid()
        {
            if (Camera.Zoom < 0.4f)
                return;

            EffectsPack.Apply(EffectsPack.ColorEffect);

            Color color = GameManager.UserSettings.DebugSettings.DebugDrawBlocksGridColor;

            foreach (Quarter quarter in Map.CurrentDimension.Quarters)
            {
                for(int x = quarter.VisibleChunksArea.xMin; x <= quarter.VisibleChunksArea.xMax; x++)
                {
                    Chunk chunk = quarter.GetChunk(x, quarter.VisibleChunksArea.yMin);

                    if (chunk == null)
                       return;

                    //Draw Y

                    for (int xC = 0; xC < ChunkParameters.ChunkCellsSize.X; xC++)
                    {
                        float xPos = chunk.Position.X + xC * ChunkParameters.BlockSize.X;

                        GameManager.VideoManager.DrawLine(new Vector2(xPos, Camera.Transform.Position2.Y - Camera.ViewSize.Y), new Vector2(xPos, Camera.Transform.Position2.Y + Camera.ViewSize.Y), color, color);
                    }
                }

                for (int y = quarter.VisibleChunksArea.yMin; y <= quarter.VisibleChunksArea.yMax; y++)
                {
                    Chunk chunk = quarter.GetChunk(quarter.VisibleChunksArea.xMin, y);

                    if (chunk == null)
                       return;

                    //Draw X

                    for (int yC = 0; yC < ChunkParameters.ChunkCellsSize.Y; yC++)
                    {
                        float yPos = chunk.Position.Y + yC * ChunkParameters.BlockSize.Y;

                        GameManager.VideoManager.DrawLine(new Vector2(Camera.Transform.Position2.X - Camera.ViewSize.X, yPos), new Vector2(Camera.Transform.Position2.X + Camera.ViewSize.X, yPos), color, color);
                    }


                }
            }

        }

        private void DrawChunkLines()
        {
            EffectsPack.Apply(EffectsPack.ColorEffect);

            //Vector2 viewSize = Camera.ViewSize;
            foreach (Quarter quarter in Map.CurrentDimension.Quarters)
                quarter.ActionForEachVisibleChunk((x, y) =>
                {

                    Chunk chunk = quarter.GetChunk(x, y);

                    if (chunk == null)
                        return;

                    GameManager.VideoManager.DrawRectangleFrame(chunk.Bounds, GameManager.UserSettings.DebugSettings.DebugDrawChunkLinesColor);

                });

        }
    }
}
