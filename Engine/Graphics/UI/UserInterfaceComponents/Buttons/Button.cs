using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Control.Input;
using Engine.Core;
using Engine.World;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.UI
{
    public class Button : UserInterfaceComponent
    {
        public bool IsInArea { get; private set; }

        public delegate void _OnLeftClick(object sender, EventArgs eventArgs);
        public event _OnLeftClick OnLeftClick;

        public delegate void _OnRightClick(object sender, EventArgs eventArgs);
        public event _OnRightClick OnRightClick;

        public delegate void _OnEntryArea(object sender, EventArgs eventArgs);
        public event _OnEntryArea OnEntryArea;

        public delegate void _OnLeaveArea(object sender, EventArgs eventArgs);
        public event _OnLeaveArea OnLeaveArea;

        public Button()
        {
            IsInArea = false;
        }

        public override void Draw(VideoManager videoManager, Transform transform)
        {

        }

        protected virtual bool CheckIfIsInArea(Vector2 pos, Transform transform)
        {
            return false;
        }

        public override void Update(InputState inputState, Transform transform)
        {

            if(CheckIfIsInArea(inputState.Mouse.Position, transform))
            {
                //in pos
                if(inputState.Mouse.IsLeftButtonOnClick())
                {
                    //on left click
                    OnLeftClick?.Invoke(this, null);
                }

                if (inputState.Mouse.IsRightButtonOnClick())
                {
                    //on left click
                    OnRightClick?.Invoke(this, null);
                }

                if(!IsInArea)
                {
                    IsInArea = true;
                    OnEntryArea?.Invoke(this, null);
                }
            }
            else
            {
                if(IsInArea)
                {
                    IsInArea = false;
                    OnLeaveArea?.Invoke(this, null);
                }
            }


        }

    }
}
