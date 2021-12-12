using Engine.Control;
using Engine.Control.Input;
using Engine.Core.Instance;
using Engine.Core.Scene;
using Engine.Resource;
using Engine.World;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Timers;

namespace Engine.Core
{
    public abstract partial class GameManager : IDisposable
    {
        public Mono Mono { get; internal set; }

        public CommandPanel CommandPanel { get; private set; }
        //INPUT
        public InputState Input { get; private set; }

        //TIME
        public GTime Time { get; private set; }
        public GTime DrawTime { get; private set; }

        //SCENE
        public SceneManager SceneManager { get; private set; }

        //VIDEO
        public VideoManager VideoManager { get; internal set; }

        //SETTINGS
        public UserSettings UserSettings { get; private set; }


        ///<summary>Current WorldManager</summary>
        public WorldManager UsedWorldManager { get; set; }

        public bool IsActivated { get; set; }

        public GameManager()
        {
            SceneManager = new SceneManager(this);

            Mono = new Mono();

            Time = new GTime();

            DrawTime = new GTime();

            Input = new InputState();

            

            UserSettings = new UserSettings();

            IsActivated = true;

            SetTimers();

        }

        private void SetTimers()
        {
            Timer timer15 = new Timer()
            {
                Interval = 15,
            };

            timer15.Elapsed += Test;
            //timer15.Start();
            //timer15.SynchronizingObject

        }
        public void Test(object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("Test15");
        }

        protected abstract void LoadScenes();

        protected abstract void LoadContent(ContentManager content);

        internal void Activated()
        {
            VideoManager.ApplyChanges();
            IsActivated = true;
            Debug.WriteLine("Activated");
        }

        internal void Deactivated()
        {
            Debug.WriteLine("Deactivated");
            IsActivated = false;
        }

        public void Exit()
        {
            Mono.MonogameInstance.Exit();
        }

        public void Run()
        {
            using (var instance = new MonoGameInstance(this))
            {
                instance.Run();
            }

        }

        public void Dispose()
        {

        }
    }
}
