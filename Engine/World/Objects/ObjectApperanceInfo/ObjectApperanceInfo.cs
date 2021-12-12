using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class ObjectApperanceInfo
    {
        /// <summary>
        /// Use to determine if u have to draw Model Texture
        /// </summary>
        public bool IsInCameraView { get; internal set; }


        /// <summary>
        /// Use to determine if u have to update shadow polygons
        /// </summary>
        public bool IsInLightView { get; internal set; }

        /// <summary>
        /// Use to determine if u have to update hitbox
        /// </summary>
        public bool IsHitboxUsed { get; set; }

        /// <summary>
        /// Use to determine if u have to update physics
        /// </summary>
        public bool IsPhysicsUpdate { get; set; }

        public bool IsObjectsCollisionOn { get; set; }


        public bool IsLandCollisionOn { get; set; }

        public bool ApplyMotionResistance { get; set; }

        public void SetDefault()
        {
            IsInCameraView = true;
            IsInLightView = true;
            IsPhysicsUpdate = true;
            IsHitboxUsed = true;
            IsObjectsCollisionOn = true;
            IsLandCollisionOn = true;
            ApplyMotionResistance = true;
        }

        public ObjectApperanceInfo()
        {
            SetDefault();
        }
    }
}
