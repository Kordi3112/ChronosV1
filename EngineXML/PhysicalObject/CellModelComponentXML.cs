using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineXML
{
    public class CellModelComponentXML
    {
        public class ShadowPolygon
        {
            public Vector2[] Vertices;
        }

        public string TextureName;
        public Vector4 Color;
        public Point CenterPosition;
        public Vector2 CellSize;
        public Vector2 CenterPointOffset;
        public bool DrawFrame;
        public Vector4 FrameColor;
        public ShadowPolygon[] ShadowPolygons;
    }
}
