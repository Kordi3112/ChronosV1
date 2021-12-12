using Engine.EngineMath;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Engine.World
{
    public class Hitbox
    {
        PhysicalObject PhysicalObject { get; set; }

        ObjectApperanceInfo ObjectApperanceInfo { get; set; }

        public Transform PrevTransform { get; private set; }
        public Transform LastTransform { get; private set; }
        Transform Transform { get; set; }

        public List<Polygon> Polygons { get; private set; }
        public List<Polygon> RealPolygons { get; private set; }


        //internal List<Polygon> ShadowPolygons { get; private set; }
        //internal List<Polygon> RealShadowPolygons { get; private set; }

        public RectangleF RealBounds { get; private set; }

        public float Radius { get; set; }

        public Hitbox(PhysicalObject physicalObject)
        {
            Transform = physicalObject.Transform;
            ObjectApperanceInfo = physicalObject.ObjectApperanceInfo;
            PhysicalObject = physicalObject;

            PrevTransform = new Transform(Transform);
            LastTransform = new Transform(Transform);
            LastTransform.Z = -23237; //random number to force update

            Polygons = new List<Polygon>();
            RealPolygons = new List<Polygon>();

            Radius = float.PositiveInfinity;
        }

        public void AddPolygon(Polygon polygon)
        {
            Polygons.Add(polygon);
            RealPolygons.Add(new Polygon(polygon.Size));
        }


        public void CalculateRadius()
        {
            float r2 = 0;

            for (int i = 0; i < Polygons.Count; i++)
            {
                for (int j = 0; j < Polygons[i].Size; j++)
                {
                    float val = Polygons[i].Vertices[j].LengthSquared();
                    if (val > r2)
                        r2 = val;
                }
            }

            Radius = (float)Math.Sqrt(r2);
        }

        internal void Update(bool forceUpdate = false)
        {
            Transform.Update();

            
            bool isTranformChanged = LastTransform.Equals(Transform);

            if (!ObjectApperanceInfo.IsHitboxUsed)
                return;

            if (isTranformChanged && !forceUpdate)
               return;
                
            PrevTransform.Copy(LastTransform);
            LastTransform.Copy(Transform);
            ///BOUNDS
            ///
            RealBounds = RectangleF.CreateFromCenter(LastTransform.Position2, new Vector2(2 * Radius));

            UpdateRealPolygons();

        }

        private void UpdateRealPolygons()
        {
            
            for (int i = 0; i < Polygons.Count; i++)
            {

                RealPolygons[i].Transform(Polygons[i], LastTransform.Matrix);

            }

        }

    }
}
