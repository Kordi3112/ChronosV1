using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Control
{
    class Command_Show : Command
    {


        public override string Name() => "show";

        public override void Run(GameManager gameManager, string[] args)
        {
            if(args.Length == 1)
            {
                CommandLine command = new CommandLine();
                command.AddPhrase("Wrong ", Color.Red);
                command.AddPhrase("number of args!", Color.White);
                gameManager.CommandPanel.AddLine(command);
                return;
            }

            if(args[1] == "cameraPos")
            {
                if (gameManager.UsedWorldManager == null)
                    return;

                CommandLine command = new CommandLine();
                command.AddPhrase("CameraTransform: ");
                command.AddPhrase(gameManager.UsedWorldManager.Camera.Transform.ToString(), Color.Gray);
                gameManager.CommandPanel.AddLine(command);
                return;
            }


            CommandLine commandLine = new CommandLine();
            commandLine.AddPhrase("Argument ", Color.White);
            commandLine.AddPhrase(args[1] + " ", Color.Red);
            commandLine.AddPhrase("is not defined!", Color.White);
            gameManager.CommandPanel.AddLine(commandLine);

        }
    }
}
