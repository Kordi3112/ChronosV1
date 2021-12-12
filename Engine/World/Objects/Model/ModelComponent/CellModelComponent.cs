using Engine.Core;
using Engine.EngineMath;
using Engine.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class ComponentCell
    {
        public Color Color { get; set; }
        public Point Position { get; set; }


        internal RectangleF Bounds { get; set; }

        public ComponentCell(Point position, Color color)
        {
            Color = color;
            Position = position;
        }
    }

    public class CellModelComponent : ModelComponent
    {
        private List<ComponentCell> Cells { get; set; }

        public Color Color { get; set; }
        public Vector2 CellSize { get; set; }
        public Vector2 CenterPointOffset { get; set; }
        public bool DrawFrame { get; set; }
        public Color FrameColor { get; set; }


        public CellModelComponent(ComponentCell[] componentCells, Vector2 cellSize, Vector2 centerPointOffset)
        {
            Cells = new List<ComponentCell>();

            CellSize = cellSize;

            ShadowPolygons = new List<Polygon>();

            RealShadowPolygons = new List<Polygon>();

            CenterPointOffset = centerPointOffset;

            SetCells(componentCells);
        }

        public CellModelComponent(Texture2D template, Point centerPos, Vector2 cellSize, Vector2 centerPointOffset)
        {
            Cells = new List<ComponentCell>();

            CellSize = cellSize;

            CenterPointOffset = centerPointOffset;

            ShadowPolygons = new List<Polygon>();

            RealShadowPolygons = new List<Polygon>();

            Color[] colorsData = new Color[template.Bounds.Width * template.Bounds.Height];

            template.GetData(colorsData);

            for (int i = 0; i < colorsData.Length; i++)
            {
                if (colorsData[i] == Color.Transparent || colorsData[i] == new Color(255, 0, 255, 255))
                    continue;

                Point pixelCoord = new Point(i % template.Bounds.Width, i / template.Bounds.Width);

                Point pixelPos = pixelCoord - centerPos;

                Cells.Add(new ComponentCell(pixelPos, colorsData[i]));

            }

            CalculateBounds();
        }

        /// <summary>
        /// Copy
        /// </summary>
        public CellModelComponent(CellModelComponent modelComponent)
        {
            ///VARIABLES TO NOT COPY
            ShadowPolygons = modelComponent.ShadowPolygons;

            Cells = modelComponent.Cells;
            Color = modelComponent.Color;
            CellSize = modelComponent.CellSize;
            CenterPointOffset = modelComponent.CenterPointOffset;
            DrawFrame = modelComponent.DrawFrame;
            FrameColor = modelComponent.FrameColor;

            ///VARIABLES TO COPY

            RealShadowPolygons = new List<Polygon>();

            for (int i = 0; i < modelComponent.RealShadowPolygons.Count; i++)
            {
                RealShadowPolygons.Add(new Polygon(modelComponent.RealShadowPolygons[i]));
            }

        }

        private void CalculateBounds()
        {
            foreach (var cell in Cells)
            {
                Vector2 centerPoint = CenterPointOffset + cell.Position.ToVector2() * CellSize;
                cell.Bounds = RectangleF.CreateFromCenter(centerPoint, CellSize);
            }
        }

        private void SetCells(ComponentCell[] cells)
        {

            foreach (var cell in cells)
            {
                Vector2 centerPoint = CenterPointOffset + cell.Position.ToVector2() * CellSize;
                cell.Bounds = RectangleF.CreateFromCenter(centerPoint, CellSize);
                Cells.Add(cell);
            }
        }

        public override void Draw(VideoManager videoManager)
        {
            //Set effect 
            EffectsPack.SetMatrix(EffectsPack.ColorEffect, Matrix);
            EffectsPack.Apply(EffectsPack.ColorEffect);


            foreach (ComponentCell cell in Cells)
            {
               
                videoManager.DrawRectanglePrimitive(cell.Bounds, cell.Color);

                if (DrawFrame)
                    videoManager.DrawRectangleFrame(cell.Bounds, FrameColor);
            }
        }


        public override ComponentType GetType()
        {
            return ComponentType.Cell;
        }
    }
}
