using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Control
{
    class Command_Tp : Command
    {
        public override string Name() => "tp";

        public override void Run(GameManager gameManager, string[] args)
        {
            CommandLine line = new CommandLine();

            for (int i = 1; i < args.Length; i++)
            {
                line.AddPhrase(args[i], i % 2 == 0 ? Color.Red : Color.Green);
            }

            gameManager.CommandPanel.AddLine(line);
        }
    }
}
