using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Scene
{
    public abstract class Scene
    {

        public GameManager GameManager { get; internal set; }

        public enum Type
        {
            //not implement WorldManager
            Menu,
            Game,
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public void Identify(string name, int id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Calling once when scene is add to SceneManager
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Calling every time scene is change to this
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Calling during Changing/Reloading Scene
        /// </summary>
        public abstract void Close();

        public abstract void Update();

        public abstract void Draw();

        public new abstract Type GetType();

        internal abstract void CreateWorldManager();

        public abstract void Dispose();

        public void Restart()
        {
            //call close event
            Close();

            //call LoadMethod
            Load();

        }
    }
}
