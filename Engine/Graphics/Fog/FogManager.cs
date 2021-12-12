using Engine.EngineMath;
using Engine.Resource;
using Engine.World;
using Engine.World.Board;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics
{
    public class FogManager
    {
        public Effect Effect { get; set; }
        WorldManager WorldManager { get; set; }

        Texture2D Texture { get; set; }

        public Vector2 Scale { get; set; }

        public Color Color { get; set; }

        public bool BorderLines { get; set; }

        public bool IsActive { get; set; }

        public FogManager(WorldManager worldManager)
        {
            WorldManager = worldManager;

            IsActive = true;
        }

        public void Init()
        {
            Color color1 = new Color(1.0f, 0.3f, 0.3f, 1);
            Color color2 = new Color(0.3f, 1.0f, 0.3f, 1);
            Color color3 = new Color(0.3f, 0.3f, 1.0f, 1);

            Texture = WorldManager.GameManager.VideoManager.CreateTexture(new Point(6, 6), (x, y) => {

                if (x == 1 && y == 1)
                    return color1;
                if (x == 4 && y == 1)
                    return color1;

                if (x == 3 && y == 2)
                    return color2;
                if (x == 2 && y == 4)
                    return color2;

                if (x == 1 && y == 3)
                    return color3;
                if (x == 3 && y == 4)
                    return color3;

                else return Color.White;
            });
        }

        public void Draw(RenderTarget2D target, float time)
        {
            if (!IsActive)
                return;

            if (!WorldManager.Map.IsActive)
                return;


            WorldManager.Camera.SetEffectMatrix(Effect);

            ///Movement parameters
            float a = 8f;
            float p = (float)(time * Math.Sin(a) + Math.Sin(4 * time) * Math.Cos(a) + 2);

            Vector2 u_time = new Vector2((float)Math.Sin(p), -0.4f * (float)Math.Sin(2 * p));


            ///set params
            Effect.Parameters["u_time"].SetValue(u_time);
            Effect.Parameters["u_scale"].SetValue(Scale);
            Effect.Parameters["u_borderLines"].SetValue(BorderLines);

            EffectsPack.SetTextureEffect(EffectsPack.FogWave, Texture);
            int counter = 0;
            //Draw background for each visible chunk
            foreach (Quarter quarter in WorldManager.Map.CurrentDimension.Quarters)
                quarter.ActionForEachVisibleChunk((x, y) => {
                    Chunk chunk = quarter.GetChunk(x, y);

                    counter++;

                    if (chunk == null)
                        return;


                    RectangleF chunkRect = chunk.Bounds;

                    Vector2 offset = Vector2.Zero;

                    if (chunk.Quarter == Quarter.QuarterAlign.RightBot)
                    {
                        offset = new Vector2(x, y);
                    }
                    else if (chunk.Quarter == Quarter.QuarterAlign.RightTop)
                    {
                        offset = new Vector2(x, -y - 1);
                    }
                    else if (chunk.Quarter == Quarter.QuarterAlign.LeftTop)
                    {
                        offset = new Vector2(-x - 1, -y - 1);
                    }
                    else if (chunk.Quarter == Quarter.QuarterAlign.LeftBot)
                    {
                        offset = new Vector2(-x - 1, y);
                    }

                    Effect.Parameters["u_offset"].SetValue(offset);
                    EffectsPack.Apply(Effect);

                    WorldManager.GameManager.VideoManager.DrawTextureRectanglePrimitive(chunkRect, Color);

                });

        }
    }
}
