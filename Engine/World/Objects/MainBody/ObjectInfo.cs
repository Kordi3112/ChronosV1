using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class ObjectInfo
    {
        public int Id { get; internal set; }

        string[] _tags;

        public string Name { get; set; }

        public ObjectInfo()
        {

        }

        public void SetTags(params string[] tags)
        {
            _tags = tags;
        }

        public bool HasTag(string tag)
        {
            for (int i = 0; i < _tags.Length; i++)
                if (_tags[i] == tag)
                    return true;

            return false;
        }

        public bool HasOneOfTags(string[] tags)
        {
            if (_tags == null)
                return false;

            for (int i = 0; i < _tags.Length; i++)
            {
                for (int j = 0; j < tags.Length; j++)
                {
                    if (_tags[i] == tags[j])
                        return true;
                }
            }

            return false;
        }

        public bool HasOneOfTags(List<string> tags)
        {
            if (_tags == null)
                return false;

            for (int i = 0; i < _tags.Length; i++)
            {
                for (int j = 0; j < tags.Count; j++)
                {
                    if (_tags[i] == tags[j])
                        return true;
                }
            }

            return false;
        }
    }
}
