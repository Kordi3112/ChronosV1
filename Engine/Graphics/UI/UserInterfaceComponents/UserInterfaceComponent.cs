using Engine.Control.Input;
using Engine.Core;
using Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics
{
    public abstract class UserInterfaceComponent
    {
        public string Name { get; set; }
        public Transform Tranform { get; set; }

        public abstract void Update(InputState inputState, Transform transform);

        public abstract void Draw(VideoManager videoManager, Transform transform);

    }
}
