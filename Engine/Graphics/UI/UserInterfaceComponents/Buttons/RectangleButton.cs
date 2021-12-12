using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Engine.EngineMath;
using Engine.Resource;
using Engine.World;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.UI
{
    public class RectangleButton : Button
    {

        public Vector2 Size { get; set; }

        public Color Color { get; set; }

        protected override bool CheckIfIsInArea(Vector2 pos, Transform transform)
        {
            RectangleF area = RectangleF.CreateFromCenter(transform.Position2, Size);

            if (area.Contains(pos))
                return true;
            else return false;
        }

        public override void Draw(VideoManager videoManager, Transform transform)
        {
            EffectsPack.Apply(EffectsPack.ColorEffect);

            RectangleF area = RectangleF.CreateFromCenter(transform.Position2, Size);
            videoManager.DrawRectanglePrimitive(area, Color);
        }


    }
}
