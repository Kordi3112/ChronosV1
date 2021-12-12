using System.Collections.Generic;
using Engine.Control.Input;
using Engine.Core;
using Engine.World;

namespace Engine.Graphics.UI
{
    public class Panel : UserInterfaceComponent
    {
        List<UserInterfaceComponent> _components;

        public Panel()
        {
            _components = new List<UserInterfaceComponent>();
        }

        public void Add(UserInterfaceComponent component)
        {
            _components.Add(component);
        }

        public UserInterfaceComponent GetComponent(string name)
        {
            foreach (var item in _components)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public override void Draw(VideoManager videoManager, Transform transform)
        {
            foreach (var item in _components)
            {
                item.Draw(videoManager, transform + item.Tranform);
            }
        }

        public override void Update(InputState inputState, Transform transform)
        {
            foreach (var item in _components)
            {
                item.Update(inputState, transform + item.Tranform);
            }
        }
    }
}
