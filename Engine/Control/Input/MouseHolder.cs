using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Engine.Control.Input
{
    public class MouseHolder
    {
        public MouseState CurrentState { get; private set; }
        public MouseState PrevState { get; private set; }

        /// <summary>
        /// returns current position
        /// </summary>
        public Vector2 Position => CurrentState.Position.ToVector2();
        public Vector2 PrevPosition => PrevState.Position.ToVector2();

        public MouseHolder()
        {
            CurrentState = Mouse.GetState();

            Update();
        }

        public void Update()
        {
            PrevState = CurrentState;
            CurrentState = Mouse.GetState();

        }

        public bool IsLeftButton(ButtonState buttonState)
        {
            if (CurrentState.LeftButton == buttonState)
                return true;
            else return false;
        }

        public bool IsRightButton(ButtonState buttonState)
        {
            if (CurrentState.RightButton == buttonState)
                return true;
            else return false;
        }

        public bool IsButton(ButtonState buttonState)
        {
            if (CurrentState.LeftButton == buttonState)
                return true;
            else return false;
        }

        public bool WasLeftButton(ButtonState buttonState)
        {
            if (PrevState.LeftButton == buttonState)
                return true;
            else return false;
        }

        public bool WasRightButton(ButtonState buttonState)
        {
            if (PrevState.RightButton == buttonState)
                return true;
            else return false;
        }

        public bool IsLeftButtonOnClick()
        {
            if (IsLeftButton(ButtonState.Pressed) && !WasLeftButton(ButtonState.Pressed))
                return true;
            else return false;
        }

        public bool IsRightButtonOnClick()
        {
            if (IsRightButton(ButtonState.Pressed) && !WasRightButton(ButtonState.Pressed))
                return true;
            else return false;
        }
    }
}
