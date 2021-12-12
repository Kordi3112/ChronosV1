using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control.Input
{
    public struct KeyString
    {
        public Keys Key;
        public char LowChar;
        public char HighChar;

        public KeyString(Keys key, char lowChar, char highChar)
        {
            Key = key;
            LowChar = lowChar;
            HighChar = highChar;
        }
    }
}
