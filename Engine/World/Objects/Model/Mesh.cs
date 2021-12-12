using EngineXML;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class Mesh
    {
        public List<Vector2> Vertices { get; private set; }

        public List<Vector2> TexCoords { get; private set; }

        public List<TriangleIds> TriangleIds { get; private set; }

        public Mesh()
        {
            Vertices = new List<Vector2>();
            TexCoords = new List<Vector2>();
            TriangleIds = new List<TriangleIds>();
        }

        /// <summary>
        /// Copy
        /// </summary>
        public Mesh(Mesh mesh)
        {
            Vertices = new List<Vector2>(mesh.Vertices);
            TexCoords = new List<Vector2>(mesh.TexCoords);
            TriangleIds = new List<TriangleIds>(mesh.TriangleIds);
        }

        public Mesh(MeshXML meshXML, float scale = 1)
        {
            Vertices = new List<Vector2>(meshXML.Vertices);

            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i] *= scale;

            TexCoords = new List<Vector2>(meshXML.TexCoords);
            TriangleIds = new List<TriangleIds>();

            for (int i = 0; i < meshXML.TriangleIds.Length; i+=3)
            {
                TriangleIds.Add(new TriangleIds(meshXML.TriangleIds[i], meshXML.TriangleIds[i + 1], meshXML.TriangleIds[i + 2]));
            }
        }

        public void AddVertex(Vector2 vertex)
        {
            Vertices.Add(vertex);
        }

        public void AddVertex(Vector2 vertex, Vector2 texCoords)
        {
            Vertices.Add(vertex);
            TexCoords.Add(texCoords);
        }

        public void AddTexCoords(Vector2 texCoords)
        {
            TexCoords.Add(texCoords);
        }

        public void AddTriangleIds(TriangleIds triangleIds)
        {
            TriangleIds.Add(triangleIds);
        }

        internal VertexPositionColorTexture GetVertexPositionColorTexture(int id, Color color)
        {
            return new VertexPositionColorTexture(new Vector3(Vertices[id], 0), color, TexCoords[id]);
        }

        internal VertexPositionColor GetVertexPositionColor(int id, Color color)
        {
            return new VertexPositionColor(new Vector3(Vertices[id], 0), color);
        }

    }
}
