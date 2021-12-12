using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control
{
    public partial class UserSettings
    {
        public class Debug
        {

            public bool DebugHitbox { get; set; }
            public Color DebugHitboxColor { get; set; }
            public bool DebugHitboxBound { get; set; }
            public Color DebugHitboxBoundColor { get; set; }

            public bool DebugShadowPolygon { get; set; }
            public Color DebugShadowPolygonColor { get; set; }

            public bool DebugLightRays { get; set; }
            public Color DebugLightRaysColor { get; set; }

            public bool DebugDrawChunkLines { get; set; }
            public Color DebugDrawChunkLinesColor { get; set; }

            public bool DebugDrawBlocksGrid { get; set; }
            public Color DebugDrawBlocksGridColor { get; set; }

            public void SetToDefault()
            {
                DebugHitbox = false;
                DebugHitboxColor = Color.Green;

                DebugHitboxBound = false;
                DebugHitboxBoundColor = Color.Pink;

                DebugShadowPolygon = false;
                DebugShadowPolygonColor = Color.Red;

                DebugLightRays = false;
                DebugLightRaysColor = Color.Green;

                DebugDrawChunkLines = false;
                DebugDrawChunkLinesColor = Color.Cyan;

                DebugDrawBlocksGrid = false;
                DebugDrawBlocksGridColor = new Color(0.02f, 0.02f, 0.02f, 0.1f);


            }
        }

    }
}
