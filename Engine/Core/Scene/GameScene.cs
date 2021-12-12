using Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Scene
{
    public abstract class GameScene : Scene
    {

        public WorldManager WorldManager { get; set; }

        public abstract override void Close();

        public abstract override void Dispose();

        public abstract override void Draw();

        public sealed override Type GetType()
        {
            return Type.Game;
        }

        public abstract override void Init();

        public abstract override void Load();

        public abstract override void Update();

        internal sealed override void CreateWorldManager()
        {
            WorldManager = new WorldManager(GameManager);
        }
    }
}
