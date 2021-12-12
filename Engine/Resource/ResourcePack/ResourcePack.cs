using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Resource
{
    public class ResourcePack<T> : IDisposable
    {
        ContentManager Content;

        Dictionary<string, T> _keyValuePairs;

        public ResourcePack(ContentManager content)
        {
            _keyValuePairs = new Dictionary<string, T>();
            Content = content;
        }

        public void Add(string name, string path)
        {
            T data = Content.Load<T>(path);

            _keyValuePairs.Add(name, data);
        }

        public void Add(string name, T data)
        {
            _keyValuePairs.Add(name, data);
        }

        public T Get(string name)
        {
            T val = default;

            _keyValuePairs.TryGetValue(name, out val);

            return val;
        }

        public void Dispose()
        {
            _keyValuePairs.GetEnumerator().Dispose();
        }
    }
}
