using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    /// <summary>
    /// Transform Light -
    /// No matrix updates
    /// </summary>
    public class TransformL : Transform
    {

        public override void SetRotation(float val) => _rotation = val;
        public override void Update(bool forceUpdate = false)
        {
            //Empty
        }
    }
}
