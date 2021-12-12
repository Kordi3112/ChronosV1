using Engine.EngineMath;
using Engine.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public partial class VideoManager
    {
        public RenderTarget2D CreateRenderTarget(Vector2 size) => new RenderTarget2D(GraphicsDevice, (int)size.X, (int)size.Y);
        /// <summary>
        /// Create a Target with View Size
        /// </summary>
        /// <returns></returns>
        public RenderTarget2D CreateRenderTarget() => new RenderTarget2D(GraphicsDevice, (int)ViewSize.X, (int)ViewSize.Y);
        public RenderTarget2D CreateRenderTargetPreserveContents() => new RenderTarget2D(GraphicsDevice, (int)ViewSize.X, (int)ViewSize.Y, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }


        public Texture2D CreateTexture(Point size, Func<int, Color> paint)
        {
            return CreateTexture(GraphicsDevice, size.X, size.Y, paint);
        }

        public Texture2D CreateTexture(Point size, Func<int, int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(GraphicsDevice, size.X, size.Y);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[size.X * size.Y];


            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    //the function applies the color according to the specified pixel
                    data[x + y * size.X] = paint(x, y);
                }
            }


            //set the color
            texture.SetData(data);

            return texture;
        }

        public Texture2D CreateGrainTexture(Vector2 textureSize, int grainsNumber, Color backgroundColor, Color grainColor, float aplfaOffset)
        {
            ///CREATE TEXTURE
            Texture2D texture = new Texture2D(GraphicsDevice, (int)textureSize.X, (int)textureSize.Y);

            ///CREATE BACKGROUND COLOR DATA  
            ///

            int pixelsNumber = texture.Bounds.Width * texture.Bounds.Height;

            Color[] data = new Color[pixelsNumber];

            for (int i = 0; i < pixelsNumber; i++)
                data[i] = backgroundColor;

            ///DRAW GRAIN POINTS

            Random random = new Random();

            for (int i = 0; i < grainsNumber; i++)
            {
                //rand grain position
                int pos = random.Next(0, pixelsNumber - 1);

                //rand alfa
                float alfa = 1 - (float)random.NextDouble() * aplfaOffset;

                data[pos] = grainColor * alfa;
            }

            //SET DATA

            texture.SetData(data);

            return texture;

        }


        /// <summary>
        /// Rectangle ABCD
        /// </summary>
        /// 
        public static void DrawTextureRectanglePrimitive(GraphicsDevice graphicsDevice, Vector2 aPos, Vector2 bPos, Vector2 cPos, Vector2 dPos, Vector2 aTexCoords, Vector2 bTexCoords, Vector2 cTexCoords, Vector2 dTexCoords, Color color)
        {
            VertexPositionColorTexture[] vertexPositionColorTextures = new VertexPositionColorTexture[6];

            VertexPositionColorTexture A = new VertexPositionColorTexture(new Vector3(aPos, 0), color, aTexCoords);
            VertexPositionColorTexture B = new VertexPositionColorTexture(new Vector3(bPos, 0), color, bTexCoords);
            VertexPositionColorTexture C = new VertexPositionColorTexture(new Vector3(cPos, 0), color, cTexCoords);
            VertexPositionColorTexture D = new VertexPositionColorTexture(new Vector3(dPos, 0), color, dTexCoords);

            vertexPositionColorTextures[0] = A;
            vertexPositionColorTextures[1] = B;
            vertexPositionColorTextures[2] = C;

            vertexPositionColorTextures[3] = A;
            vertexPositionColorTextures[4] = C;
            vertexPositionColorTextures[5] = D;

            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionColorTextures, 0, 2);

        }
        public void DrawTextureRectanglePrimitive(Vector2 aPos, Vector2 bPos, Vector2 cPos, Vector2 dPos, Vector2 aTexCoords, Vector2 bTexCoords, Vector2 cTexCoords, Vector2 dTexCoords, Color color)
        {
            DrawTextureRectanglePrimitive(GraphicsDevice, aPos, bPos, cPos, dPos, aTexCoords, bTexCoords, cTexCoords, dTexCoords, color);
        }

        public void DrawTextureRectanglePrimitive(RectangleF coords, Color color)
        {
            DrawTextureRectanglePrimitive(coords.LeftTop, coords.RightTop, coords.RightBot, coords.LeftBot, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), color);
        }

        public static void DrawTextureRectanglePrimitive(GraphicsDevice graphicsDevice, RectangleF coords, Color color)
        {
            DrawTextureRectanglePrimitive(graphicsDevice, coords.LeftTop, coords.RightTop, coords.RightBot, coords.LeftBot, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), color);
        }
        public  void DrawTextureRectanglePrimitive(RectangleF coords, Vector2 aTexCoords, Vector2 bTexCoords, Vector2 cTexCoords, Vector2 dTexCoords, Color color)
        {
            DrawTextureRectanglePrimitive(coords.LeftTop, coords.RightTop, coords.RightBot, coords.LeftBot, aTexCoords, bTexCoords, cTexCoords, dTexCoords, color);
        }


        /// <summary>
        /// Rectangle ABCD
        /// </summary>
        public void DrawRectanglePrimitive(RectangleF rectangle, Color aColor, Color bColor, Color cColor, Color dColor)
        {
            VertexPositionColor[] vertexPositionColors = new VertexPositionColor[6];

            VertexPositionColor A = new VertexPositionColor(new Vector3(rectangle.LeftTop, 0), aColor);
            VertexPositionColor B = new VertexPositionColor(new Vector3(rectangle.RightTop, 0), bColor);
            VertexPositionColor C = new VertexPositionColor(new Vector3(rectangle.RightBot, 0), cColor);
            VertexPositionColor D = new VertexPositionColor(new Vector3(rectangle.LeftBot, 0), dColor);

            vertexPositionColors[0] = A;
            vertexPositionColors[1] = B;
            vertexPositionColors[2] = C;

            vertexPositionColors[3] = A;
            vertexPositionColors[4] = C;
            vertexPositionColors[5] = D;

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionColors, 0, 2);
        }

        public void DrawRectanglePrimitive(RectangleF rectangle, Color color)
        {
            DrawRectanglePrimitive(rectangle, color, color, color, color);
        }

        public static void DrawRectangleFrame(GraphicsDevice graphicsDevice, RectangleF rectangle, Color color)
        {
            VertexPositionColor[] vertexPositionColors = new VertexPositionColor[8];

            VertexPositionColor A = new VertexPositionColor(new Vector3(rectangle.LeftTop, 0), color);
            VertexPositionColor B = new VertexPositionColor(new Vector3(rectangle.RightTop, 0), color);
            VertexPositionColor C = new VertexPositionColor(new Vector3(rectangle.RightBot, 0), color);
            VertexPositionColor D = new VertexPositionColor(new Vector3(rectangle.LeftBot, 0), color);

            vertexPositionColors[0] = A;
            vertexPositionColors[1] = B;

            vertexPositionColors[2] = B;
            vertexPositionColors[3] = C;

            vertexPositionColors[4] = C;
            vertexPositionColors[5] = D;

            vertexPositionColors[6] = D;
            vertexPositionColors[7] = A;

            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColors, 0, 4);
        }

        public void DrawRectangleFrame(Effect effect, Color color, Vector2 position, Vector2 size)
        {
            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[5];


            Vector3 topLeft = new Vector3(position - 0.5f * size, 0);
            Vector3 topRight = new Vector3(position + new Vector2(0.5f * size.X, -0.5f * size.Y), 0);
            Vector3 botLeft = new Vector3(position + new Vector2(-0.5f * size.X, 0.5f * size.Y), 0);
            Vector3 botRight = new Vector3(position + 0.5f * size, 0);

            vertexPositionColor[0] = new VertexPositionColor(topLeft, color);
            vertexPositionColor[1] = new VertexPositionColor(topRight, color);
            vertexPositionColor[2] = new VertexPositionColor(botRight, color);
            vertexPositionColor[3] = new VertexPositionColor(botLeft, color);
            vertexPositionColor[4] = new VertexPositionColor(topLeft, color);

            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexPositionColor, 0, 4);
        }

       

        public void DrawRectangleFrame(RectangleF rectangle, Color aColor, Color bColor, Color cColor, Color dColor)
        {
            VertexPositionColor[] vertexPositionColors = new VertexPositionColor[8];

            VertexPositionColor A = new VertexPositionColor(new Vector3(rectangle.LeftTop, 0), aColor);
            VertexPositionColor B = new VertexPositionColor(new Vector3(rectangle.RightTop, 0), bColor);
            VertexPositionColor C = new VertexPositionColor(new Vector3(rectangle.RightBot, 0), cColor);
            VertexPositionColor D = new VertexPositionColor(new Vector3(rectangle.LeftBot, 0), dColor);

            vertexPositionColors[0] = A;
            vertexPositionColors[1] = B;

            vertexPositionColors[2] = B;
            vertexPositionColors[3] = C;

            vertexPositionColors[4] = C;
            vertexPositionColors[5] = D;

            vertexPositionColors[6] = D;
            vertexPositionColors[7] = A;

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColors, 0,4);
        }

        public void DrawRectangleFrame(RectangleF rectangle, Color color)
        {
            DrawRectangleFrame(rectangle, color, color, color, color);
        }

        public void DrawLine(Effect effect, Vector2 pointA, Vector2 pointB, Color colorA, Color colorB)
        {
            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[2];

            vertexPositionColor[0] = new VertexPositionColor(new Vector3(pointA, 0), colorA);
            vertexPositionColor[1] = new VertexPositionColor(new Vector3(pointB, 0), colorB);

            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColor, 0, 1);

            VideoManager.DrawLine(GraphicsDevice, effect, pointA, pointB, colorA, colorB);
        }


        public void DrawLine(Vector2 pointA, Vector2 pointB, Color colorA, Color colorB)
        {
            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[2];

            vertexPositionColor[0] = new VertexPositionColor(new Vector3(pointA, 0), colorA);
            vertexPositionColor[1] = new VertexPositionColor(new Vector3(pointB, 0), colorB);


            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColor, 0, 1);

            VideoManager.DrawLine(GraphicsDevice, EffectsPack.ColorEffect, pointA, pointB, colorA, colorB);
        }

        public static void DrawLine(GraphicsDevice graphicsDevice, Effect effect, Vector2 pointA, Vector2 pointB, Color colorA, Color colorB)
        {
            VertexPositionColor[] vertexPositionColor = new VertexPositionColor[2];

            vertexPositionColor[0] = new VertexPositionColor(new Vector3(pointA, 0), colorA);
            vertexPositionColor[1] = new VertexPositionColor(new Vector3(pointB, 0), colorB);

            effect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColor, 0, 1);
        }
        public void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, RectangleF bounds, Color color, Vector2 origin = new Vector2(), SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Vector2 textSize = font.MeasureString(text );

            Vector2 scale;

            if (bounds.Width == 0)
                scale = new Vector2(bounds.Height / textSize.Y);
            else if (bounds.Height == 0)
                scale = new Vector2(bounds.Width / textSize.X);
            else scale = bounds.Size / textSize;

            spriteBatch.DrawString(font, text, bounds.Point, color, 0, origin / scale, scale, spriteEffects, 0);
        }

        public static Texture2D CopyTexture(GraphicsDevice graphicsDevice, RenderTarget2D target)
        {
            Texture2D texture = new Texture2D(graphicsDevice, target.Bounds.Width, target.Bounds.Width);

            Color[] texdata = new Color[texture.Bounds.Width * texture.Bounds.Height];

            target.GetData(texdata);

            texture.SetData(texdata);

            return texture;
        }

        public static void CopyTexture(Texture2D texture, RenderTarget2D target)
        {

            Color[] texdata = new Color[texture.Bounds.Width * texture.Bounds.Height];

            target.GetData(texdata);

            texture.SetData(texdata);
        }

        /// <summary>
        /// Screen coords matrix
        /// </summary>
        public Matrix GetDefaultViewMatrix() => Matrix.CreateScale(1 / ViewSize.X * 2, -1 / ViewSize.Y * 2, 1) * Matrix.CreateTranslation(-1, 1, 0);
    }
}
