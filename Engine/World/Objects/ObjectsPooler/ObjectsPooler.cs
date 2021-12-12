using Engine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class ObjectsPooler : Pooler<PhysicalObject>
    {
        WorldManager WorldManager { get; set; }

        public int ContainerId { get; private set; }

        public ObjectsPooler(WorldManager worldManager, int containerId)
        {
            WorldManager = worldManager;
            ContainerId = containerId;
        }

        protected override void AfterAddAction(PhysicalObject newObject)
        {
            newObject.WorldManager = WorldManager;
            newObject.Start();
        }
    }
}
