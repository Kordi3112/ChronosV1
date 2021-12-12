using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public class GTime
    {
        /// <summary>
        /// Time which include TimeSpeed: RealElapsed * TimeSpeed;
        /// </summary>
        public float DeltaTime { get; private set; }

        public float DeltaTimePow2 => DeltaTime * DeltaTime;
        /// <summary>
        /// Time without TimeSpeed
        /// </summary>
        public float RealDeltaTime { get; private set; }

        public float RealDeltaTimePow2 => RealDeltaTime * RealDeltaTime;

        public float TimeSpeed { get; set; }

        public float TotalTime { get; private set; }

        public float TotalRealTime { get; private set; }

        public GTime()
        {
            TimeSpeed = 1.0f;
        }


        public GTime(float timeSpeed)
        {
            TimeSpeed = timeSpeed;
        }

        /// <summary>
        /// Call in GameManager update method, calculate elapsedtime
        /// </summary>
        public void Update(GameTime gameTime)
        {

            RealDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            TotalRealTime += RealDeltaTime;

            DeltaTime = RealDeltaTime * TimeSpeed;

            TotalTime += DeltaTime;

        }



        /// <summary>
        /// Return ApparentElapsed * mul
        /// </summary>
        public float GetModifiedTime(float mul)
        {
            return DeltaTime * mul;
        }
    }
}
