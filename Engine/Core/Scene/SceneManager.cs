using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Scene
{
    public class SceneManager : IDisposable
    {
        List<Scene> scenes;

        GameManager GameManager { get; set; }

        public Scene CurrentScene { get; private set; }

        public HeadScene HeadScene { get; private set; }

        public SceneManager(GameManager gameManager)
        {
            scenes = new List<Scene>();

            GameManager = gameManager;
        }

        public void AddScene(Scene scene)
        {
            scene.GameManager = GameManager;

            scene.CreateWorldManager();

            scene.Init();

            scenes.Add(scene);
        }

        public void SetHeadScene(HeadScene headScene)
        {
            HeadScene = headScene;

            HeadScene.GameManager = GameManager;
            HeadScene.Init();
        }

        private void SceneChange(Scene scene)
        {
            if (CurrentScene != null)
            {
                CurrentScene.Close();
            }


            CurrentScene = scene;
            CurrentScene.Load();
        }

        public bool SetCurrentSceneById(int id)
        {
            foreach (Scene scene in scenes)
            {
                if (scene.Id == id)
                {
                    SceneChange(scene);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update HeadScene and then Scene
        /// </summary>
        public void UpdateActiveScenes()
        {

            if (HeadScene != null)
                HeadScene.Update();


            if (CurrentScene != null)
                CurrentScene.Update();
        }

        /// <summary>
        /// Update Scene and then HeadScene
        /// </summary>
        public void DrawActiveScenes()
        {
            if (CurrentScene != null)
                CurrentScene.Draw();

            if (HeadScene != null)
                HeadScene.Draw();
        }

        public void Dispose()
        {
            foreach (Scene scene in scenes)
                scene.Dispose();

            HeadScene.Dispose();
        }
    }
}
