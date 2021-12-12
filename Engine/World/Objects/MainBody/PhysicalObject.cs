using Engine.Resource;
using EngineXML;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class PhysicalObject : GameObject
    {
        public Transform Transform { get; set; }

        public Hitbox Hitbox { get; set; }

        public Model2D Model { get; set; }

        public Rigidbody Rigidbody { get; set; }

        public ObjectApperanceInfo ObjectApperanceInfo { get; set; }

        public virtual void OnCollision(ObjectInfo collider)
        {

        }

        public void SetObjectFromXML(ObjectXML objectXML, ResourcePack<MeshModelComponent> resourcePack)
        {
            ///ADD COMPONENTS
            ///
            for (int i = 0; i < objectXML.Components.Length; i++)
            {
                var component = objectXML.Components[i];
                MeshModelComponent modelComponent = new MeshModelComponent(resourcePack.Get(component.Name));
                modelComponent.Transform = new Transform(component.Position, component.Rotation);
                modelComponent.Tag = objectXML.Components[i].Tag;

                Model.AddModelComponent(modelComponent);
            }

            ///SET HITBOX
            ///
            
            for (int i = 0; i < objectXML.Hitboxes.Length; i++)
            {
                Hitbox.AddPolygon(new EngineMath.Polygon(new Vector3(objectXML.HitboxScale),objectXML.Hitboxes[i].Vertices));
            }

            Hitbox.Radius = objectXML.HitboxRadius * objectXML.HitboxScale;
            
        }


        public void SetObjectFromXML(ObjectXML objectXML, ResourcePack<CellModelComponent> resourcePack)
        {
            Debug.WriteLine("Scale1: " + objectXML.Hitboxes[0].Vertices[0]);
            ///ADD COMPONENTS
            ///
            for (int i = 0; i < objectXML.Components.Length; i++)
            {
                var component = objectXML.Components[i];
                CellModelComponent modelComponent = new CellModelComponent(resourcePack.Get(component.Name));
                modelComponent.Transform = new Transform(component.Position, component.Rotation);
                modelComponent.Tag = objectXML.Components[i].Tag;

                Model.AddModelComponent(modelComponent);
            }

            ///SET HITBOX
            ///

            

            for (int i = 0; i < objectXML.Hitboxes.Length; i++)
            {
                Hitbox.AddPolygon(new EngineMath.Polygon(new Vector3(objectXML.HitboxScale), objectXML.Hitboxes[i].Vertices));
            }

            Hitbox.Radius = objectXML.HitboxRadius * objectXML.HitboxScale;
            Debug.WriteLine("Scale2: " + objectXML.Hitboxes[0].Vertices[0]);
        }
        /*
        public virtual void OnCollision(Block block)
        {

        }
        */
    }
}
