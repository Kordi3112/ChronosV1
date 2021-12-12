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

namespace Engine.World.Board
{
    public class BlockCell
    {
        public Color Color { get; set; }
        public Point Position { get; set; }


        internal RectangleF Bounds { get; set; }

        public BlockCell(Point position, Color color)
        {
            Color = color;
            Position = position;
        }

    }
    public class BlockTemplate
    {
        BlockCell[] _blockCells;

        public BlockTemplate(Texture2D template)
        {
            //BlockTemplate(template, )
        }
        public BlockTemplate(Texture2D template, Point centerPos, Vector2 cellSize, Vector2 centerPointOffset)
        {
            _blockCells = new BlockCell[template.Bounds.Width * template.Bounds.Height];

            Color[] colorsData = new Color[template.Bounds.Width * template.Bounds.Height];

            template.GetData(colorsData);

            for (int i = 0; i < colorsData.Length; i++)
            {
                if (colorsData[i] == Color.Transparent || colorsData[i] == new Color(255, 0, 255, 255))
                    continue;

                Point pixelCoord = new Point(i % template.Bounds.Width, i / template.Bounds.Width);

                Point pixelPos = pixelCoord - centerPos;

                Color color = colorsData[i] * 0.5f;
                color.A = 255;

                _blockCells[i] = new BlockCell(pixelPos, color);

            }

            CalculateBounds(centerPointOffset, cellSize);
        }


        private void CalculateBounds(Vector2 centerPointOffset, Vector2 cellSize)
        {
            foreach (var cell in _blockCells)
            {
                Vector2 centerPoint = centerPointOffset + cell.Position.ToVector2() * cellSize;
                cell.Bounds = RectangleF.CreateFromCenter(centerPoint, cellSize);
            }
        }


        public void Draw(VideoManager videoManager, Matrix matrix)
        {
            //Set effect 
            EffectsPack.SetMatrix(EffectsPack.ColorEffect, matrix);
            EffectsPack.Apply(EffectsPack.ColorEffect);


            foreach (var cell in _blockCells)
            {

                videoManager.DrawRectanglePrimitive(cell.Bounds, cell.Color);

            }
        }

    }
}
