using Choros.Source.Resource;
using Engine.Core.Scene;
using Engine.Graphics.UI;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Choros.Source.App.Scenes
{
    public class Scene2 : GameScene
    {
        UserInterfaceManager UserInterfaceManager { get; set; }

        Panel Panel1 { get; set; }
        StatusBar statusBar1;

        public Scene2(ResourcePackManager resourcePackManager)
        {

        }
        public override void Close()
        {

        }

        public override void Dispose()
        {

        }


        public override void Init()
        {
            Identify("Scene2", 2);


            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Test;
            //timer.Start();
        }

        public void Test(object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("Test");
        }

        public override void Load()
        {
            UserInterfaceManager = new UserInterfaceManager(GameManager);

            RectangleButton rectangleButton1 = new RectangleButton()
            {
                Size = new Vector2(10, 10),
                Color = Color.Red,
                Tranform = new Transform(new Vector2(200,200))
            };
            rectangleButton1.OnEntryArea += OnEnter;
            rectangleButton1.OnLeaveArea += OnLeave;
            rectangleButton1.OnLeftClick += OnClick;

            RectangleButton rectangleButton2 = new RectangleButton()
            {
                Size = new Vector2(20, 20),
                Color = Color.Red,
                Tranform = new Transform(new Vector2(800, 400))
            };
            rectangleButton2.OnEntryArea += OnEnter;
            rectangleButton2.OnLeaveArea += OnLeave;
            rectangleButton2.OnLeftClick += OnClick;

            statusBar1 = new StatusBar()
            {
                Name = "statusBar1",
                Size = new Vector2(10, 100),
                Tranform = new Transform(new Vector2(100, 300)),
                Value = 0.3f,
                IsVertical = true
            };


            Panel1 = new Panel()
            {
                Tranform = new Transform(new Vector2(0, 0)),
                Name = "panel1"
            };
            Panel1.Add(rectangleButton1);
            Panel1.Add(rectangleButton2);
            Panel1.Add(statusBar1); 

            UserInterfaceManager.Add(Panel1);

        }

        public override void Update()
        {
            UserInterfaceManager.Update();

            statusBar1.Value += 0.5f * GameManager.Time.DeltaTime;
            if (statusBar1.Value > 1)
                statusBar1.Value = statusBar1.Value - 1;
        }

        public override void Draw()
        {
            UserInterfaceManager.Draw();
        }

        public void OnEnter(object sender, EventArgs args)
        {
            
            if(sender.GetType() == typeof(RectangleButton))
            {
                var comp = (sender as RectangleButton);
                comp.Color = Color.Green;
                comp.Size *= 4.0f;
            }
                
        }

        public void OnLeave(object sender, EventArgs args)
        {
            if (sender.GetType() == typeof(RectangleButton))
            {
                var comp = (sender as RectangleButton);
                comp.Color = Color.Yellow;

                comp.Size *= 0.25f;
            }
                
        }

        public void OnClick(object sender, EventArgs args)
        {
            if (sender.GetType() == typeof(RectangleButton))
                (sender as RectangleButton).Color = Color.Pink;
        }
    }
}
