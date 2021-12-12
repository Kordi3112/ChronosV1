using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Control.Input;
using Engine.Core;
using Engine.EngineMath;
using Engine.Resource;
using Engine.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Control
{
    public class CommandPanel
    {
        private List<Command> Commands;
        private List<CommandLine> CommandLines;

        private List<string> InputTextHistory;
        private int currentHistoryId = 0;
        private RenderTarget2D RenderTarget2D { get; set; }

        public enum PanelAlign
        {
            TopLeft,
            TopRight,
            BotLeft,
            BotRight
        }

        public enum ApperanceMode
        {
            Hidden,
            Partly,
            OnlyWhenActive,
        }

        private GameManager GameManager;
        public SpriteFont SpriteFont { get; set; }
        public bool IsActive { get;  set; }

        //Align

        public PanelAlign Align { get; set; }

        public ApperanceMode Mode { get; set; }
        /// <summary>Distance from corner </summary>
        public Vector2 Margin { get; set; }

        //
        public Vector2 Size { get; set; }

        //DESIGN
        public Color BackgroundColor { get; set; }

        public float InputTextPanelHeight { get; set; }

        public string InputText { get; private set; }
        private bool isInputTextActive;

        public float MessageRowHeight { get; set; }
        public float LineSpaceHeight { get; set; }

        float counter = 0;

        public CommandPanel(GameManager gameManager)
        {
            CommandLines = new List<CommandLine>();
            InputTextHistory = new List<string>();
            GameManager = gameManager;
            IsActive = false;
            isInputTextActive = false;
            InputText = "";
            SetDefaultSettings();

            RestartRenderTarget();

            LoadCommands();

            CommandLine cl = new CommandLine();
            cl.AddPhrase("Line1 ", Color.Red);
            cl.AddPhrase("Line2 ", Color.Green);
            cl.AddPhrase("Line3 ", Color.Blue);

            //CommandLines.Add(cl);
           // CommandLines.Add(cl);
           // CommandLines.Add(cl);
        }

        private void LoadCommands()
        {
            Commands = new List<Command>();
            Commands.Add(new Command_Tp());
            Commands.Add(new Command_Show());
            Commands.Add(new Command_Move());
        }

        private void SetDefaultSettings()
        {
            Align = PanelAlign.BotLeft;
            Mode = ApperanceMode.Partly;
            Margin = new Vector2(20,20);
            Size = new Vector2(500, 500);

            BackgroundColor = Color.Black * 0.4f;

            InputTextPanelHeight = 30;

            MessageRowHeight = 20;
            LineSpaceHeight = 2;
        }

        private void RestartRenderTarget()
        {
            RenderTarget2D = new RenderTarget2D(GameManager.VideoManager.GraphicsDevice, (int)Size.X, (int)Size.Y);
        }

        public void AddLine(CommandLine commandLine)
        {
            commandLine.Time = GameManager.Time.TotalRealTime;
            CommandLines.Add(commandLine);

            
        }

        public void Update()
        {



            //if(gameManager.Input.Keys.ButtonOnClick(Microsoft.Xna.Framework.Input.Keys.b))


            UpdateTextInput();


            
            /*
            if(GameManager.Input.Mouse.IsLeftButtonOnClick())
            {
                Vector2 pos = GetLeftTopCornerPosition(GameManager.VideoManager.ViewSize) + new Vector2(0, Size.Y - InputTextPanelHeight);

                RectangleF inputTextPanelBounds = new RectangleF(pos, new Vector2(Size.X, InputTextPanelHeight));

                if (inputTextPanelBounds.Contains(GameManager.Input.Mouse.Position))
                    isInputTextActive = true;
                else isInputTextActive = false;
            }
            */

        }

        private void UpdateTextInput()
        {
            
            if(GameManager.Input.Keys.ButtonOnClick(Keys.OemQuestion))
            {
                if (!IsActive)
                {
                    
                    IsActive = true;
                    if (InputText.Length == 0)
                        InputText = "/";

                    return;
                }

                
            }

            if (!IsActive)
                return;

            if(GameManager.Input.Keys.ButtonOnClick(Keys.Enter))
            {
                InputTextHistory.Add(InputText);
                ProccedCommand();
                
            }
            else if (GameManager.Input.Keys.ButtonOnClick(Keys.Back))
            {
                if (InputText.Length == 0)
                    return;
       
                InputText = InputText.Substring(0, InputText.Length - 1);
                
            }
            else if(GameManager.Input.Keys.ButtonOnClick(Keys.Up))
            {
                if (InputTextHistory.Count == 0)
                    return;

                if (currentHistoryId < 2)
                    currentHistoryId = 1;
                else currentHistoryId--;

                InputText = InputTextHistory[InputTextHistory.Count - currentHistoryId];

            }
            else if (GameManager.Input.Keys.ButtonOnClick(Keys.Down))
            {
                if (InputTextHistory.Count == 0)
                    return;

                if (currentHistoryId + 1 > InputTextHistory.Count)
                    currentHistoryId = InputTextHistory.Count;
                else currentHistoryId++;

                

                Debug.WriteLine(currentHistoryId);

                InputText = InputTextHistory[InputTextHistory.Count - currentHistoryId];
            }
            else
            {
                

                List<string> input = new List<string>(GameManager.Input.Keys.GetOnClickKeysStrings(KeysHolder.GetStandardKeysStringsCollection()));

                if (input.Count == 0)
                    return;

                currentHistoryId = 0;

                foreach (var str in input)
                    InputText += str;

            }
        }

        private void ProccedCommand()
        {
            if (InputText.Length == 0)
                return;

            if (InputText.Substring(0, 1) != "/")
                return;
           
            string[] args = InputText.Substring(1).Split(new char[] { ' ' });

            if (args.Length == 0)
                return;

            foreach (Command command in Commands)
            {
                if(command.Name() == args[0])
                {
                    command.Run(GameManager, args);
                    InputText = "";
                    return;
                }
            }

            var line = new CommandLine();
            line.AddPhrase("Command: ", Color.White);
            line.AddPhrase(args[0], Color.Red);
            line.AddPhrase(" is undefined!", Color.White);


            AddLine(line);

            InputText = "";

            //InputText = InputText.Substring(1);
        }

        public void Draw(VideoManager videoManager)
        {
            if (Mode == ApperanceMode.Hidden)
                return;
            else if (Mode == ApperanceMode.OnlyWhenActive && !IsActive)
                return;

            //Clear Render Target
            videoManager.SetRenderTarget(RenderTarget2D);



            videoManager.ClearTarget(IsActive ? BackgroundColor : Color.Transparent);

            //if (SpriteFont == null)
            // return;

            SetMatrix(videoManager);

            DrawText(videoManager);

            DrawInputPanel(videoManager);


            //

            videoManager.SetFinalRenderTarget();
            videoManager.SpriteBatch.Begin(SpriteSortMode.Immediate);

            videoManager.SpriteBatch.Draw(RenderTarget2D, GetLeftTopCornerPosition(videoManager.ViewSize), Color.White);

            videoManager.SpriteBatch.End();

        }

        public void SetMatrix(VideoManager videoManager)
        {
            Matrix defaultMatrix = Camera.CreateDefaultViewMatrix(Size);

            EffectsPack.SetMatrix(EffectsPack.ColorEffect, defaultMatrix);
            EffectsPack.SetMatrix(EffectsPack.TextureEffect, defaultMatrix);
        }

        private void DrawInputPanel(VideoManager videoManager)
        {
            if (!IsActive)
                return;
            //Drame
            EffectsPack.Apply(EffectsPack.ColorEffect);

            Vector2 leftTopPosition =  new Vector2(1, Size.Y - InputTextPanelHeight);
            Vector2 size = new Vector2(Size.X - 1, InputTextPanelHeight);


            if (isInputTextActive)
            {
                videoManager.DrawRectanglePrimitive(new RectangleF(leftTopPosition, size), Color.Black * 0.8f);

                if(InputText.Length == 0 && GameManager.Time.TotalRealTime % 1.0f > 0.5f)
                    videoManager.DrawRectanglePrimitive(new RectangleF(leftTopPosition + new Vector2(InputTextPanelHeight) * 0.2f, new Vector2(InputTextPanelHeight * 0.1f, InputTextPanelHeight * 0.6f)), Color.White);
            }
                

            videoManager.DrawRectangleFrame(new RectangleF(leftTopPosition, size), Color.Pink);
        }

        private void DrawText(VideoManager videoManager)
        {
           
            videoManager.SpriteBatch.Begin();

            DrawInputText(videoManager);
            DrawPhrases(videoManager);
          
            videoManager.SpriteBatch.End();
        }
        private void DrawInputText(VideoManager videoManager)
        {
            if (!IsActive)
                return;

            Vector2 pos = new Vector2(InputTextPanelHeight * 0.2f, Size.Y - InputTextPanelHeight * 0.2f);


            RectangleF rectangle = new RectangleF(pos, new Vector2(0, InputTextPanelHeight * 0.6f));


            string text = (GameManager.Time.TotalRealTime % 1.0f > 0.5f) ? InputText : InputText + "_";
            videoManager.DrawString(videoManager.SpriteBatch, SpriteFont, text, rectangle, Color.AntiqueWhite, new Vector2(0, rectangle.Height));
        }

        private void DrawPhrases(VideoManager videoManager)
        {
            Vector2 phrasesMargin = new Vector2(10, 10);

            Vector2 currentPos =  new Vector2(phrasesMargin.X, Size.Y - (InputTextPanelHeight + phrasesMargin.Y));

            for (int i = CommandLines.Count - 1; i >= 0; i--)
            {
                CommandLine commandLine = CommandLines[i];

                float colorMultiplier = 0;

                if (IsActive)
                    colorMultiplier = 1;
                else
                {
                    float diff = GameManager.Time.TotalTime - commandLine.Time;
                    if (diff > 6.0f)
                        continue;

                    if (diff < 3.0f)
                        colorMultiplier = 1;
                    else colorMultiplier = 1 - diff / 6.0f;
                }


                string firstText = "[" + commandLine.Time.ToString("0000.00") + "] ";

                DrawPhrases_StringLane(videoManager, ref currentPos, "[", Color.Gray * colorMultiplier);
                DrawPhrases_StringLane(videoManager, ref currentPos, commandLine.Time.ToString("0000.00"), Color.DarkGray * colorMultiplier);


                DrawPhrases_StringLane(videoManager, ref currentPos, "] ", Color.Gray * colorMultiplier);

                for (int phrase = 0; phrase < commandLine.PhrasesCount; phrase++)
                {
                    string text = commandLine.GetPhraseString(phrase);
                    Color color = commandLine.GetPhraseColor(phrase) * colorMultiplier;

                    DrawPhrases_StringLane(videoManager, ref currentPos, text, color);
                }
                
                currentPos.Y -= MessageRowHeight + LineSpaceHeight;
                currentPos.X = phrasesMargin.X;
            }
        }

        private void DrawPhrases_StringLane(VideoManager videoManager, ref Vector2 currentPos, string text, Color color)
        {
            videoManager.DrawString(videoManager.SpriteBatch, SpriteFont, text, new RectangleF(currentPos, new Vector2(0, MessageRowHeight)), color, new Vector2(0, MessageRowHeight));


            Vector2 textSize = SpriteFont.MeasureString(text);
            float scale = MessageRowHeight / textSize.Y;
            currentPos.X += SpriteFont.MeasureString(text).X * scale;

        }

        private Vector2 GetLeftTopCornerPosition(Vector2 viewSize)
        {
            if (Align == PanelAlign.TopLeft)
                return Margin;
            else if (Align == PanelAlign.TopRight)
                return new Vector2(viewSize.X - (Margin.X + Size.X), Margin.Y);
            else if (Align == PanelAlign.BotLeft)
                return new Vector2(Margin.X, viewSize.Y - Margin.Y - Size.Y);
            else //BotRight
                return viewSize - (Margin + Size);

        }
    }
}
