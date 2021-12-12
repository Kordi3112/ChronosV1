using Engine.Core;
using Engine.EngineMath;
using Engine.Resource;
using Engine.World.Objects.Model;
using EngineXML;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class MeshModelComponent : ModelComponent, IDisposable
    {

        public Mesh Mesh { get; private set; }
        public Material Material { get; set; }

        public ApperanceMode Mode { get; set; }

        public MeshModelComponent(Mesh mesh, Material material, ApperanceMode mode = ApperanceMode.All)
        {
            SetMesh(mesh);
            SetMaterial(material);

            ShadowPolygons = new List<Polygon>();
            RealShadowPolygons = new List<Polygon>();

            
        }

        public MeshModelComponent(ModelComponentXML modelComponentXML, ResourcePack<MeshXML> meshResource, ResourcePack<Texture2D> textureResource)
        {
            MeshXML meshXML = meshResource.Get(modelComponentXML.Mesh.Name);

            SetMesh(new Mesh(meshXML, modelComponentXML.Mesh.Scale));

            Color color = new Color(modelComponentXML.Material.Color[0], modelComponentXML.Material.Color[1], modelComponentXML.Material.Color[2], modelComponentXML.Material.Color[3]);

            Material = new Material(color, textureResource.Get(modelComponentXML.Material.TextureName));

            Transform = new Transform();

            ShadowPolygons = new List<Polygon>();
            RealShadowPolygons = new List<Polygon>();

            foreach (var polygon in modelComponentXML.ShadowPolygons)
            {
                Vector2[] vertices = new Vector2[polygon.Vertices.Length];

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = polygon.Vertices[i] * modelComponentXML.Mesh.Scale;
                }

                AddShadowPolygon(new Polygon(vertices));
            }

            

        }

        /// <summary>
        /// Copy
        /// </summary>
        public MeshModelComponent(MeshModelComponent modelComponent)
        {
            ///VARIABLES TO NOT COPY
            Mesh = modelComponent.Mesh;
            Material = modelComponent.Material;
            ShadowPolygons = modelComponent.ShadowPolygons;
            Mode = modelComponent.Mode;

            ///VARIABLES TO COPY

            RealShadowPolygons = new List<Polygon>();

            for (int i = 0; i < modelComponent.RealShadowPolygons.Count; i++)
            {
                RealShadowPolygons.Add(new Polygon(modelComponent.RealShadowPolygons[i]));
            }

        }

        public void SetMesh(Mesh mesh)
        {
            Mesh = mesh;
        }

        public void SetMaterial(Material material)
        {
            Material = material;
        }


        public void AddShadowPolygon(Polygon polygon)
        {
            ShadowPolygons.Add(polygon);
            RealShadowPolygons.Add(new Polygon(polygon.Size));
        }



        public override void Draw(VideoManager videoManager)
        {
            if (Mesh == null || Material == null)
                return;

            if (Mode == ApperanceMode.All)
                DrawTexture(videoManager, Material.Color);
            else if (Mode == ApperanceMode.OnlyTexture)
                DrawTexture(videoManager, Color.White);
            else DrawColor(videoManager, Material.Color); //OnlyColor _
        }

        private void DrawTexture(VideoManager videoManager, Color color)
        {
            if (Mesh.TriangleIds.Count == 0)
                return;

            int lenght = Mesh.TriangleIds.Count * 3;

            VertexPositionColorTexture[] vertexPositionColorTexture = new VertexPositionColorTexture[lenght];
        
            for (int i = 0; i < Mesh.TriangleIds.Count; i++)
            {
                TriangleIds id = Mesh.TriangleIds[i];

                int vert = i * 3;

                vertexPositionColorTexture[vert] = GetVertexPositionColorTexture(id.A, color);
                vertexPositionColorTexture[vert + 1] = GetVertexPositionColorTexture(id.B, color);
                vertexPositionColorTexture[vert + 2] = GetVertexPositionColorTexture(id.C, color);
            }

            EffectsPack.TextureEffect.Parameters["Texture"].SetValue(Material.Texture);
            //Set Matrix
            EffectsPack.SetMatrix(EffectsPack.TextureEffect, Matrix);

            EffectsPack.TextureEffect.CurrentTechnique.Passes[0].Apply();

            videoManager.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionColorTexture, 0, Mesh.TriangleIds.Count);
        }

        private void DrawColor(VideoManager videoManager, Color color)
        {
            if (Mesh.TriangleIds.Count == 0)
                return;

            int lenght = Mesh.TriangleIds.Count * 3;

            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[lenght];


            for (int i = 0; i < Mesh.TriangleIds.Count; i++)
            {
                TriangleIds id = Mesh.TriangleIds[i];

                int vert = i * 3;

                vertexPositionColor[vert] = GetVertexPositionColor(id.A, color);
                vertexPositionColor[vert + 1] = GetVertexPositionColor(id.B, color);
                vertexPositionColor[vert + 2] = GetVertexPositionColor(id.C, color);
            }

            //SetMatrix

            EffectsPack.SetMatrix(EffectsPack.ColorEffect, Matrix);


            EffectsPack.ColorEffect.CurrentTechnique.Passes[0].Apply();

            videoManager.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionColor, 0, Mesh.TriangleIds.Count);
        }


        internal VertexPositionColorTexture GetVertexPositionColorTexture(int id, Color color)
        {
            return new VertexPositionColorTexture(new Vector3(Mesh.Vertices[id], 0), color, Mesh.TexCoords[id]);
        }

        internal VertexPositionColor GetVertexPositionColor(int id, Color color)
        {
            return new VertexPositionColor(new Vector3(Mesh.Vertices[id], 0), color);
        }

        public void Dispose()
        {
            
        }

        public override ComponentType GetType()
        {
            return ComponentType.Mesh;
        }
    }
}
