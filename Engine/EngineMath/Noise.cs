using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    public class Noise
    {

        Vector2[,] vectors;

        Point size;

        public Noise(Point size, int seed)
        {
            Random random = new Random(seed);

            this.size = size;

            vectors = new Vector2[size.X, size.Y];

            for(int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    double angle = 6.28 * random.NextDouble();

                    Vector2 vector = new Vector2(-(float)Math.Sin(angle), (float)Math.Cos(angle));
                   
                    vectors[x, y] = vector;
                }
            }


        }


        Vector2 GetVector(Point point)
        {
            //double rotation = (permutationsX[point.X] + permutationsY[point.Y]) * 0.1592;

            //return new Vector2(-(float)Math.Sin(rotation), (float)Math.Cos(rotation));

            return vectors[point.X, point.Y];
        }

        public float Perlin(Vector2 coords)
        {
            

            Point leftTopSquarePos = Tools.Floor(coords);

            //fix coords

            leftTopSquarePos.X = leftTopSquarePos.X % (size.X-1) ;
            leftTopSquarePos.Y = leftTopSquarePos.Y % (size.Y-1) ;

            if (leftTopSquarePos.X < 0)
                leftTopSquarePos.X *= -1;
            if (leftTopSquarePos.Y < 0)
                leftTopSquarePos.Y *= -1;

            Vector2 fractPoint = Tools.Fract(coords);

            ///Get random vector of each corner
            ///

            Vector2 leftTop = GetVector(leftTopSquarePos);
            Vector2 rightTop = GetVector(leftTopSquarePos + new Point(1, 0));
            Vector2 rightBot = GetVector(leftTopSquarePos + new Point(1, 1));
            Vector2 leftBot = GetVector(leftTopSquarePos + new Point(0, 1));


            Vector2 pToLeftTop = fractPoint;
            Vector2 pToRightTop = fractPoint - new Vector2(1,0);
            Vector2 pToRightBot = fractPoint - new Vector2(1, 1); 
            Vector2 pToLeftBot = fractPoint - new Vector2(0, 1);



            float ltDot = Tools.Dot(leftTop, pToLeftTop);
            float rtDot = Tools.Dot(rightTop, pToRightTop);
            float rbDot = Tools.Dot(rightBot, pToRightBot);
            float lbDot = Tools.Dot(leftBot, pToLeftBot);

            float topLerp = Tools.Lerp(ltDot, rtDot, Fade(fractPoint.X));
            float botLerp = Tools.Lerp(lbDot, rbDot, Fade(fractPoint.X));

            float wholeLerp = Tools.Lerp(topLerp, botLerp, Fade(fractPoint.Y));


            //Min: -0.7057517, Max: 0.7052013
            return (wholeLerp + 0.7f) * 0.7143f;
            //return (wholeLerp + 1) / 2;
        }

        public float FbmPerlin(Vector2 coords, int octaves)
        {
            float normalize_factor = 0.0f;

            float value = 0.0f;
            float scale = 0.5f;

            for (int i = 0; i < octaves; i++)
            {
                value += Perlin(coords) * scale;
                normalize_factor += scale;
                coords *= 2.0f;
                scale *= 0.5f;
            }

            return value / normalize_factor;
        }

        public float ValueNoise(Vector2 coords)
        {
            Point leftTopSquarePos = Tools.Floor(coords);

            Vector2 fractPoint = Tools.Fract(coords);



            float leftTop = (GetVector(leftTopSquarePos).X + 1) / 2;
            float rightTop = (GetVector(leftTopSquarePos + new Point(1, 0)).X + 1) / 2;
            float rightBot = (GetVector(leftTopSquarePos + new Point(1, 1)).X + 1) / 2;
            float leftBot = (GetVector(leftTopSquarePos + new Point(0, 1)).X + 1) / 2;

            float topLerp = Tools.Lerp(leftTop, rightTop, Fade(fractPoint.X));
            float botLerp = Tools.Lerp(leftBot, rightBot, Fade(fractPoint.X));

            float wholeLerp = Tools.Lerp(topLerp, botLerp, Fade(fractPoint.Y));



            return wholeLerp;
        }

        public float CellularNoise(Vector2 coords)
        {
            Point leftTopSquarePos = Tools.Floor(coords);
            Vector2 f = Tools.Fract(coords);
            //fix coords

            leftTopSquarePos.X = leftTopSquarePos.X % (size.X - 1);
            leftTopSquarePos.Y = leftTopSquarePos.Y % (size.Y - 1);

            float min_dist = 9999999.0f;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {

                    // generate a random point in each tile,
                    // but also account for whether it's a farther, neighbouring tile
                    //Vector2 node = rand2(i + vec2(x, y)) + vec2(x, y);

                    Vector2 node = Rand2(leftTopSquarePos.ToVector2() + new Vector2(x, y)) + new Vector2(x, y);

                    // check for distance to the point in that tile
                    // decide whether it's the minimum
                    float dist = (float)Math.Sqrt((f - node).X * (f - node).X + (f - node).Y * (f - node).Y);

                    min_dist = Math.Min(min_dist, dist);
                }
            }
            return min_dist;
        }

        public float FbmCellular(Vector2 coords, int octaves)
        {
            float normalize_factor = 0.0f;

            float value = 0.0f;
            float scale = 0.5f;

            for (int i = 0; i < octaves; i++)
            {
                value += CellularNoise(coords) * scale;
                normalize_factor += scale;
                coords *= 2.0f;
                scale *= 0.5f;
            }

            return value / normalize_factor;
        }

        Vector2 Rand2(Vector2 coord)
        {
            // prevents randomness decreasing from coordinates too large
            coord.X = coord.X % 10000.0f;
            coord.Y = coord.Y % 10000.0f;
            // returns "random" vec2 with x and y between 0 and 1
            Vector2 result = new Vector2();
            result.X = Tools.Fract((float)Math.Sin(Tools.Dot(coord, new Vector2(127.1f, 311.7f)) * 43758.5453f));
            result.Y = Tools.Fract((float)Math.Sin(Tools.Dot(coord, new Vector2(269.5f, 183.3f)) * 43758.5453f));

            return result;

            

            
        }

        public float Fade(float f)
        {
            //6t5-15t4+10t3

            return f * f * (3.0f - 2.0f * f);
        }
    }
}
