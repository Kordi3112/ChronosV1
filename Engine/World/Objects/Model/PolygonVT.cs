using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.World.Objects.Model
{
    public struct VertexTCoords
    {
        public Vector2 Vertex { get; set; }
        public Vector2 TCoords { get; set; }

        public VertexTCoords(Vector2 vertex, Vector2 tCoords)
        {
            Vertex = vertex;
            TCoords = tCoords;
        }
    }

    /// <summary>
    /// Polygon which stores (V)ertex and (T)exture Coords
    /// </summary>
    public class PolygonVT
    {

        List<VertexTCoords> _vertexTCoords;

        public List<Vector2> RealVertices { get; private set; }

        public PolygonVT()
        {
            _vertexTCoords = new List<VertexTCoords>();
            RealVertices = new List<Vector2>();
        }

        public void AddVertex(VertexTCoords vertexTCoords)
        {
            _vertexTCoords.Add(vertexTCoords);
            RealVertices.Add(Vector2.Zero);
        }

        public void AddVertex(Vector2 vertex)
        {
            _vertexTCoords.Add(new VertexTCoords(vertex, Vector2.Zero));
            RealVertices.Add(Vector2.Zero);
        }

        public void AddVertex(Vector2 vertex, Vector2 tCoords)
        {
            _vertexTCoords.Add(new VertexTCoords(vertex, tCoords));
            RealVertices.Add(Vector2.Zero);
        }

        public VertexTCoords GetVertexTCoords(int id)
        {
            return new VertexTCoords(RealVertices[id], _vertexTCoords[id].TCoords);
        }

        /// <summary>
        /// Update Real Vertices
        /// </summary>
        /// <param name="matrix"></param>
        public void Update(Matrix matrix)
        {
            for (int i = 0; i < _vertexTCoords.Count; i++)
            {
                RealVertices[i] = Tools.Mul(_vertexTCoords[i].Vertex, matrix);
            }
        }

        public PolygonVT Clone()
        {
            PolygonVT polygonVT = new PolygonVT()
            {
                RealVertices = new List<Vector2>(RealVertices)
            };

            return polygonVT;
        }
    }
}
