using Engine.Core;
using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public abstract class ModelComponent
    {
        public enum ApperanceMode
        {
            OnlyTexture,
            OnlyColor,
            All,
        }

        public enum ComponentType
        {
            Mesh,
            Cell
        }

        /// <summary>
        /// Component Transform, offset*
        /// </summary>
        public Transform Transform { get; set; }
        // basicComponentCoords to ScreenCoords
        protected Matrix Matrix { get; set; }

        public List<Polygon> ShadowPolygons { get; protected set; }
        public List<Polygon> RealShadowPolygons { get; protected set; }

        public string Tag { get; set; }


        public void Update(Matrix matrix, Camera camera)
        {
            Transform.Update();

            //update transform matrix: basic component coords to screen
            Matrix = matrix * camera.CameraMatrix;
        }

        public abstract void Draw(VideoManager videoManager);

        public void AddShadowPolygon(Polygon polygon)
        {
            ShadowPolygons.Add(polygon);
            RealShadowPolygons.Add(new Polygon(polygon.Size));
        }

        internal void UpdateRealShadowPolygons(Matrix matrix)
        {
            for (int i = 0; i < ShadowPolygons.Count; i++)
            {
                RealShadowPolygons[i].Transform(ShadowPolygons[i], matrix);
            }
        }

        public new abstract ComponentType GetType();
    }
}
