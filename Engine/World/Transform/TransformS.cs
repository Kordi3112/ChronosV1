using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.World
{

    /// <summary>
    /// Transform Safe
    /// Auto updating matrix rotation, u dont have to call "Update" Method
    /// </summary>
    public class TransformS : Transform
    {

        public override void SetPosition(Vector3 position)
        {
            _position = position;

            UpdatePositionChange();
        }

        public override void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;

            UpdatePositionChange();
        }

        public override void SetZ(float val)
        {
            _position.Z = val;

            UpdatePositionChange();
        }

        public override void SetX(float val)
        {
            _position.X = val;

            UpdatePositionChange();
        }

        public override void SetY(float val)
        {
            _position.Y = val;

            UpdatePositionChange();
        }

        public override void SetRotation(float val)
        {
            _rotation = val;

            UpdateRotationChange();
        }

        private void UpdateRotationChange()
        {
            UpdatePositionMatrix();
            UpdateRotationMatrix();
            UpdateMatrix();
        }

        private void UpdatePositionChange()
        {
            UpdatePositionMatrix();
            UpdateMatrix();
        }

        public override void Update(bool forceUpdate = false)
        {
            //empty
        }

    }
}
