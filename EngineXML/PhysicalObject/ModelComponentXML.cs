using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineXML
{
    public class ModelComponentXML
    {
        public class ShadowPolygon
        {
            public Vector2[] Vertices;
        }

        public class MaterialInfo
        {
            public string TextureName;
            public float[] Color;
        }

        public class MeshInfo
        {
            public float Scale;
            public string Name;
        }


        public MeshInfo Mesh;
        public MaterialInfo Material;

        public ShadowPolygon[] ShadowPolygons;
        // public 
    }
}
