using Engine.EngineMath;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics.Light
{
    public class CircleLatern : Latern
    {
        public CircleLatern(Transform transform, float range, Color color, bool isShadowCaster, bool isActive)
        {
            Transform = transform;
            Range = range;
            Color = color;
            IsShadowCaster = isShadowCaster;
            IsActive = isActive;

            Bounds = RectangleF.CreateFromCenter(transform.Position2, new Vector2(range * 2.0f));
        }

        public override Type GetType() => Type.Circle;
    }
}
