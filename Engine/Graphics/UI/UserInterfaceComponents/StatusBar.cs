using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Control.Input;
using Engine.Core;
using Engine.EngineMath;
using Engine.Resource;
using Engine.World;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.UI
{
    public class StatusBar : UserInterfaceComponent
    {

        public Vector2 Size { get; set; }

        public float Value { get; set; }

        public bool IsVertical { get; set; }

        public StatusBar()
        {
            IsVertical = false;
        }
        public override void Draw(VideoManager videoManager, Transform transform)
        {
            EffectsPack.Apply(EffectsPack.ColorEffect);

            RectangleF area = RectangleF.CreateFromCenter(transform.Position2, Size);


            //Draw Bg
            videoManager.DrawRectanglePrimitive(area, Color.Gray);

            //DrawProgress
            RectangleF progresArea;
            if (IsVertical)
            {
                progresArea = new RectangleF(area.LeftBot - new Vector2(0, area.Size.Y * Value), new Vector2(area.Size.X, area.Size.Y * Value));
            }
            else progresArea = new RectangleF(area.LeftTop, new Vector2(area.Size.X * Value, area.Size.Y));

            videoManager.DrawRectanglePrimitive(progresArea, Color.Yellow);
            //DrawFrame
            videoManager.DrawRectangleFrame(area, Color.Black);

        }

        public override void Update(InputState inputState, Transform transform)
        {
           
        }
    }
}
