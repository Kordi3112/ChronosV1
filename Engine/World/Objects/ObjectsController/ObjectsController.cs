using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    /// <summary>
    /// Determines Objects behaviour
    /// </summary>
    abstract class ObjectsController
    {
        protected WorldManager WorldManager { get; set; }
        protected GameManager GameManager => WorldManager.GameManager;

        public ObjectsController(WorldManager worldManager)
        {
            WorldManager = worldManager;
        }

        public abstract void UpdateObjects(ObjectsPooler objects, Map map);


    }
}
