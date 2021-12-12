using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using Engine.Core;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System;

namespace Engine.Resource
{
    public static class EffectsPack
    {
        /// <summary>
        /// Drawing texture using wvp matrix and color
        /// </summary>
        public static Effect TextureEffect { get; set; }



        /// <summary>
        /// Combining Light and Map
        /// </summary>
        public static Effect Combine1 { get; set; }

        /// <summary>
        /// Combining ShadowMap and Latern texture
        /// </summary>
        public static Effect Combine2 { get; set; }

        /// <summary>
        /// Drawing primitives using wvp position color 
        /// </summary>
        public static Effect ColorEffect { get; set; }

        /// <summary>
        /// Drawing primitives using wvp position color 
        /// </summary>
        public static Effect Fog { get; set; }

        /// <summary>
        /// Drawing primitives using wvp position color with wave parameters 
        /// </summary>
        public static Effect FogWave { get; set; }

        public static void Apply(Effect effect)
        {
            effect.CurrentTechnique.Passes[0].Apply();
        }

        public static void SetTextureEffect(Texture2D texture)
        {
            TextureEffect.Parameters["Texture"].SetValue(texture);
        }

        public static void SetTextureEffect(Effect effect, Texture2D texture)
        {
            effect.Parameters["Texture"].SetValue(texture);
        }

        public static void SetMatrix(Effect effect, Matrix matrix)
        {
            effect.Parameters["WorldViewProjection"].SetValue(matrix);
        }

    }

}
