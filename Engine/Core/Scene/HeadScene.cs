using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Scene
{
    public abstract class HeadScene
    {
        public GameManager GameManager { get; internal set; }

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
        public abstract void Dispose();
    }
}
