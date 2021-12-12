using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control
{
    public abstract class Command
    {

        public abstract string Name();
        public abstract void Run(GameManager gameManager, string[] args);
    }
}
