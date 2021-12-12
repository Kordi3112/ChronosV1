using Engine.Resource;
using Engine.World;
using EngineXML;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Resource
{
    public class ResourcePackManager : IDisposable
    {
        public ResourcePack<Texture2D> TexturesPack;
        public ResourcePack<CellModelComponent> ModelComponentsPack;
        public ResourcePack<ObjectXML> ObjectsXMLPack;
        public ResourcePack<MeshXML> MeshesXMLPack;

        public ResourcePack<SpriteFont> FontsPack;

        public ResourcePackManager(ContentManager content)
        {
            TexturesPack = new ResourcePack<Texture2D>(content);
            ModelComponentsPack = new ResourcePack<CellModelComponent>(content);
            ObjectsXMLPack = new ResourcePack<ObjectXML>(content);
            MeshesXMLPack = new ResourcePack<MeshXML>(content);

            FontsPack = new ResourcePack<SpriteFont>(content);
        }

        /*
        public void LoadComponents(ContentManager content)
        {
            var componentsToLoadXML = content.Load<ComponentsToLoadXML>("XML/componentsToLoad");

            foreach (string name in componentsToLoadXML.Names)
            {
                ModelComponentXML modelComponentXML = content.Load<ModelComponentXML>("XML/ModelComponents/" + name);

                ModelComponent modelComponent = new ModelComponent();
                modelComponent.Texture = TexturesPack.Get(modelComponentXML.TextureName);
                modelComponent.LoadMeshFromXML(modelComponentXML);
            }
        }

        */
        public void Load(ContentManager content)
        {

            //LOAD MESHES

            MeshesToLoad meshesToLoad = content.Load<MeshesToLoad>("XML/meshesToLoad");

            foreach (var meshPack in meshesToLoad.MeshPacks)
            {
                foreach (var name in meshPack.Names)
                {
                    MeshesXMLPack.Add(name, meshPack.FolderPath + name);
                }
            }
            //LOAD TEXTURES

            TexturesToLoad texturesToLoad = content.Load<TexturesToLoad>("XML/texturesToLoad");

            foreach (var texturePack in texturesToLoad.TexturePacks)
            {
                foreach (var name in texturePack.Names)
                {
                    TexturesPack.Add(name, texturePack.FolderPath + name);
                }
            }

            LoadCustomTextures();

            //LOAD COMPONENTS


            ComponentsToLoad componentsToLoad = content.Load<ComponentsToLoad>("XML/componentsToLoad2");

            foreach (var componentsPack in componentsToLoad.ComponentPacks)
            {
                foreach (var name in componentsPack.Names)
                {
                    CellModelComponentXML modelComponentXML = content.Load<CellModelComponentXML>(componentsPack.FolderPath + name);
                    //MeshModelComponent modelComponent = new MeshModelComponent(modelComponentXML, MeshesXMLPack, TexturesPack);
                    CellModelComponent cellModelComponent = new CellModelComponent(TexturesPack.Get(modelComponentXML.TextureName), modelComponentXML.CenterPosition, modelComponentXML.CellSize, modelComponentXML.CenterPointOffset);

                    cellModelComponent.DrawFrame = cellModelComponent.DrawFrame;
                    cellModelComponent.FrameColor = cellModelComponent.FrameColor;

                    foreach (var polygon in modelComponentXML.ShadowPolygons)
                        cellModelComponent.AddShadowPolygon(new Engine.EngineMath.Polygon(polygon.Vertices));

                    ModelComponentsPack.Add(name, cellModelComponent);
                }
            }
            //LOAD OBJECTSXML


            ObjectsToLoad objectsToLoad = content.Load<ObjectsToLoad>("XML/objectsToLoad2");

            foreach (var objectPack in objectsToLoad.ObjectPacks)
            {
                foreach (var name in objectPack.Names)
                {
                    ObjectsXMLPack.Add(name, objectPack.FolderPath + name);
                }
            }

            LoadFonts();
        }

        private void LoadFonts()
        {
            FontsPack.Add("Default", "Fonts/Default");
        }

        private void LoadCustomTextures()
        {
            TexturesPack.Add("hull3", "XML/Textures/hull3");
            TexturesPack.Add("laser4", "XML/Textures/laser4");
            TexturesPack.Add("bullet1", "XML/Textures/bullet1");
            TexturesPack.Add("model1", "XML/Textures/model1");
            TexturesPack.Add("model2", "XML/Textures/model2");
            TexturesPack.Add("model3", "XML/Textures/model3");
            TexturesPack.Add("engine1", "XML/Textures/engine1");


        }

        public void Dispose()
        {
            TexturesPack.Dispose();
            ModelComponentsPack.Dispose();
        }
    }
}
