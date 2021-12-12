using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;

namespace Engine.Control
{
    public class Command_Move : Command
    {
        public override string Name() => "move";

        public override void Run(GameManager gameManager, string[] args)
        {
            if (gameManager.UsedWorldManager == null)
                return;

            if (args.Length == 1)
                return;

            if(args[1] == "camera")
            {
                gameManager.UsedWorldManager.Camera.Transform.Position2 = new Microsoft.Xna.Framework.Vector2(float.Parse(args[2]), float.Parse(args[3]));
            
            }
        }
    }
}
