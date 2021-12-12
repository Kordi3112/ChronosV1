using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control
{
    public partial class UserSettings
    {
        public class Video
        {
            //LIGHT
            
            /// <summary>
            /// 0 - no threading 1 - calculate laterns in threads 2 - calculate laterns & split rays calculation
            /// </summary>
            public uint ThreadingLevel { get; set; } 
            public bool UseMoreRaysInLighting { get; set; }

            public int SkipFrames { get; set; }

            public bool Antyaliasing { get; set; }

            /// <summary>
            /// All chunks are rendered on the texture, it size is: ChunkTextureQuality * ChunkSize, higher number gives more details: 1 is default
            /// </summary>
            public int ChunkTextureQuality { get; set; }

            public int MaxQuarterChunksNumberToUpdatePerSecond { get; set; }

            public void SetToDefault()
            {
                SkipFrames = 0;

                ThreadingLevel = 0;
                UseMoreRaysInLighting = false;
                Antyaliasing = true;

                ChunkTextureQuality = 2;

                MaxQuarterChunksNumberToUpdatePerSecond = 1;
            }
        }
    }
}
