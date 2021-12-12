using Engine.Api;
using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class GameObject : IDisposable, IPollerObject
    {
        int _poolerId;
        bool _isDeleted = false;

        public GameManager GameManager => WorldManager.GameManager;
        public WorldManager WorldManager { get; internal set; }


        public ObjectInfo Info { get; set; }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Copy<T>(T gameObject) where T : GameObject
        {

        }

        public int GetPoolerId()
        {
            return _poolerId;
        }

        public void SetPollerId(int id)
        {
            _poolerId = id;
        }

        public virtual void Dispose()
        {

        }

        public void Delete(bool isDelete)
        {
            _isDeleted = isDelete;
        }

        public bool IsDeleted()
        {
            return _isDeleted;
        }
    }
}
