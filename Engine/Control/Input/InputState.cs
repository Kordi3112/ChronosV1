using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control.Input
{
    public class InputState
    {
        public KeysHolder Keys { get; set; }

        public MouseHolder Mouse { get; set; }

        public bool IsWindowActive { get; set; }
        public InputState()
        {
            Keys = new KeysHolder();

            Mouse = new MouseHolder();
        }

        public void Update()
        {
            
            Keys.Update();
            Mouse.Update();
        }
    }
}
