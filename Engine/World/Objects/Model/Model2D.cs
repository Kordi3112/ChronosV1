using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class Model2D
    {
        List<ModelComponent> _modelComponents;

        public int ComponentsCount => _modelComponents.Count();

        public Model2D()
        {
            Init();
        }


        void Init()
        {
            _modelComponents = new List<ModelComponent>();
        }


        public void AddModelComponent(ModelComponent modelComponent)
        {
            _modelComponents.Add(modelComponent);
        }

        internal ModelComponent GetModelComponent(int id)
        {
            return _modelComponents[id];
        }

        public ModelComponent GetModelComponent(string tag)
        {
            foreach (ModelComponent modelComponent in _modelComponents)
                if (tag == modelComponent.Tag)
                    return modelComponent;

            return null;
        }

        internal void Update(Transform transform, Camera camera, bool IsInCameraView)
        {
            foreach (ModelComponent component in _modelComponents)
            {
                //(Rot x Pos) <- Local Matrix         WorldMatrix 
                Matrix matrix = component.Transform.Matrix * transform.Matrix;

                component.UpdateRealShadowPolygons(matrix);

                if(IsInCameraView)
                    component.Update(matrix, camera);

            }
        }

        internal void Draw(WorldManager worldManager)
        {
            foreach (ModelComponent component in _modelComponents)
            {
                component.Draw(worldManager.GameManager.VideoManager);
            }
        }

        public bool HaveShadowPolygon()
        {
            foreach (ModelComponent component in _modelComponents)
            {
                if (component.ShadowPolygons.Count > 0)
                    return true;
            }

            return false;
        }

    }
}
