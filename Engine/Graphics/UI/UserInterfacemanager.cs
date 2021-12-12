using Engine.Control.Input;
using Engine.Core;
using Engine.Resource;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics.UI
{
    public class UserInterfaceManager
    {
        List<UserInterfaceComponent> _components;
        GameManager GameManager { get; set; }

        public UserInterfaceManager(GameManager gameManager)
        {
            _components = new List<UserInterfaceComponent>();

            GameManager = gameManager;
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

        public void Update()
        {
            if (_components.Count == 0)
                return;

            foreach (var component in _components)
            {
                component.Update(GameManager.Input, component.Tranform);
            }
        }

        public void Draw()
        {
            if (_components.Count == 0)
                return;
            //Update matrixes

            Matrix matrix = Camera.CreateDefaultViewMatrix(GameManager.VideoManager);

            EffectsPack.SetMatrix(EffectsPack.ColorEffect, matrix);
            EffectsPack.SetMatrix(EffectsPack.TextureEffect, matrix);

            foreach (var component in _components)
            {
                component.Draw(GameManager.VideoManager, component.Tranform);
            }
        }
    }
}
