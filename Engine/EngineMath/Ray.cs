using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    class RayLine
    {
        public Line Line { get; set; }

        float _minT = 0;

        /// <summary>
        /// Closest collision = Pos + Direction * MinT
        /// </summary>
        public float MinT { get => _minT; set { _minT = value; } }

        /// <summary>
        /// Represents angle - but its not in radians - is used for sorting method
        /// </summary>
        public double Alpha { get; set; }

        public Vector2 RayPoint { get; set; }


        RayLine()
        {

        }

        public RayLine(Vector2 pointA, Vector2 pointB, float minT = 10000)
        {
            Line = new Line(pointA, pointB);

            Vector2 direction = Line.Direction;

            Alpha = Math.Atan2(direction.Y,direction.X);

            MinT = minT;

        }

        public void Calculate(Polygon polygon)
        {

            int size = polygon.Size;

            for (int y = 0; y < size; y++)
            {
                Line line = new Line(polygon.Vertices[y], polygon.Vertices[(y + 1) % size]);

                if(Line.CheckCollisionFast(ref line, ref _minT))
                {

                    ////infinity collisions

                    //u have to check if clossest point is open or closed (if its open u have to set MinT for this)
                    // polygon.Vertices[y] is always closed / polygon.Vertices[(y + 1) % size] is always open 

                    Vector2 linePointToClosedVertex = polygon.Vertices[y] - Line.Point;
                    Vector2 linePointToOpenedVertex = polygon.Vertices[(y + 1) % size] - Line.Point;

                    if(Tools.Dot(line.Direction, linePointToClosedVertex) > Tools.Dot(line.Direction, linePointToOpenedVertex))
                    {
                        //if (IsHelperLine)
                        //  continue; //if its helper line - pass

                        //linePointToClosedVertex have greater DotProd so its closer in given direction
                        float minT;

                        if (line.Direction.X != 0)
                            minT = linePointToClosedVertex.X / line.Direction.X;
                        else minT = linePointToClosedVertex.Y / line.Direction.Y;

                        if (minT < MinT)
                            MinT = minT;

                    }

                }
            }

        }
        public void Draw(GraphicsDevice device, Color color = new Color())
        {
            RayPoint = Line.GetPoint(MinT);

            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[]
            {
                //screen center
                new VertexPositionColor(new Vector3(Line.Point.X ,Line.Point.Y ,0), color),
                new VertexPositionColor(new Vector3(RayPoint.X ,RayPoint.Y ,0), color),
            };


            device.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColor, 0, 1);
        }

        public static void DrawLights(GraphicsDevice device, List<RayLine> rays, Color lightColor)
        {

            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[rays.Count() * 3];


            for (int i = 0; i < rays.Count() - 1; i++)
            {
                int vert = i * 3;

                vertexPositionColor[vert] = new VertexPositionColor(new Vector3(rays[i].RayPoint, 0), lightColor);
                vertexPositionColor[vert + 1] = new VertexPositionColor(new Vector3(rays[i + 1].RayPoint, 0), lightColor);
                //center
                vertexPositionColor[vert + 2] = new VertexPositionColor(new Vector3(rays[i].Line.Point, 0), lightColor);
            }

            int lastVert = rays.Count() * 3 - 1;

            vertexPositionColor[lastVert - 2] = new VertexPositionColor(new Vector3(rays[rays.Count() - 1].RayPoint, 0), lightColor);
            vertexPositionColor[lastVert - 1] = new VertexPositionColor(new Vector3(rays[0].RayPoint, 0), lightColor);
            //center
            vertexPositionColor[lastVert] = new VertexPositionColor(new Vector3(rays[rays.Count() - 1].Line.Point, 0), lightColor);

            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionColor, 0, rays.Count);
        }

        public static RayLine OffsetRay(RayLine ray, float offsetAngle)
        {
            double alpha = ray.Alpha + offsetAngle;

            
            // if u will rotate above 360deg u can make sorting issues
            if (alpha >= -3.0d)
                alpha = 1.0d + (alpha + 3.0d);
            

            return new RayLine
            {
                Line = new Line(ray.Line.Point, Tools.Rotate(ray.Line.Direction, offsetAngle), true),
                Alpha = alpha,           
                MinT = ray.MinT
            };

        }

        public static RayLine OffsetRay(RayLine ray, float offsetAngle, float sinAngle, float cosAngle)
        {
            double alpha = ray.Alpha + offsetAngle;

            /*
            // if u will rotate above 360deg u can make sorting issues
            if (alpha >= -3.0d)
                alpha = 1.0d + (alpha + 3.0d);
            */

            return new RayLine
            {
                Line = new Line(ray.Line.Point, Tools.Rotate(ray.Line.Direction, sinAngle, cosAngle), true),

                Alpha = alpha,

                MinT = ray.MinT
            };

        }
    }
}
