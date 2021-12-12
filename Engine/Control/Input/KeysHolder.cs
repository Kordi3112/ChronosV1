using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Control.Input
{
    public class KeysHolder
    {
        public KeyboardState CurrentState { get; private set; }
        public KeyboardState PrevState { get; private set; }

        public KeysHolder()
        {
            CurrentState = Keyboard.GetState();

            Update();
        }

        public void Update()
        {
            PrevState = CurrentState;
            CurrentState = Keyboard.GetState();
        }


        public bool IsKeyDown(Keys KEY)
        {
            return CurrentState.IsKeyDown(KEY);
        }

        public bool IsKeyUp(Keys KEY)
        {
            return CurrentState.IsKeyUp(KEY);
        }
        public bool WasKeyDown(Keys KEY)
        {
            return PrevState.IsKeyDown(KEY);
        }

        public bool WasKeyUp(Keys KEY)
        {
            return PrevState.IsKeyUp(KEY);
        }
        /// <summary>
        /// return true only once per click button
        /// </summary>
        public bool ButtonOnClick(Keys KEY)
        {
            if (CurrentState.IsKeyDown(KEY) && !PrevState.IsKeyDown(KEY))
                return true;
            else return false;
        }

        public bool ButtonOnRelease(Keys KEY)
        {
            if (!CurrentState.IsKeyDown(KEY) && PrevState.IsKeyDown(KEY))
                return true;
            else return false;
        }

        //public Keys[] GetOnClickKeys(KeyString[] activeKeys)
        //{
        //    //foreach()
        //}
        public IEnumerable<string> GetOnClickKeysStrings(KeyString[] activeKeys)
        {
            //string

            foreach (var keyString in activeKeys)
            {
                if(ButtonOnClick(keyString.Key))
                {
                    if (IsKeyDown(Keys.LeftShift))
                        yield return keyString.HighChar.ToString();
                    else yield return keyString.LowChar.ToString();
                }
            }
        }

        public static Keys[] GetStandardKeysCollection()
        {
            return new Keys[] {
                Keys.Q,
                Keys.W,
                Keys.E,
                Keys.R,
                Keys.T,
                Keys.Y,
                Keys.U,
                Keys.I,
                Keys.O,
                Keys.P,
                Keys.A,
                Keys.S,
                Keys.D,
                Keys.F,
                Keys.G,
                Keys.H,
                Keys.J,
                Keys.K,
                Keys.L,
                Keys.Z,
                Keys.X,
                Keys.C,
                Keys.V,
                Keys.B,
                Keys.N,
                Keys.M,

                //
                Keys.D0,
                Keys.D1,
                Keys.D2,
                Keys.D3,
                Keys.D4,
                Keys.D5,
                Keys.D6,
                Keys.D7,
                Keys.D8,
                Keys.D9,
                

            };
        }

        public static KeyString[] GetStandardKeysStringsCollection()
        {
            return new KeyString[] {

                new KeyString(Keys.Q, 'q', 'Q'),
                new KeyString(Keys.W, 'w', 'W'),
                new KeyString(Keys.E, 'e', 'E'),
                new KeyString(Keys.R, 'r', 'R'),
                new KeyString(Keys.T, 't', 'T'),
                new KeyString(Keys.Y, 'y', 'Y'),
                new KeyString(Keys.U, 'u', 'U'),
                new KeyString(Keys.I, 'i', 'I'),
                new KeyString(Keys.O, 'o', 'O'),
                new KeyString(Keys.P, 'p', 'P'),
                new KeyString(Keys.A, 'a', 'A'),
                new KeyString(Keys.S, 's', 'S'),
                new KeyString(Keys.D, 'd', 'D'),
                new KeyString(Keys.F, 'f', 'F'),
                new KeyString(Keys.G, 'g', 'G'),
                new KeyString(Keys.H, 'h', 'H'),
                new KeyString(Keys.J, 'j', 'J'),
                new KeyString(Keys.K, 'k', 'K'),
                new KeyString(Keys.L, 'l', 'L'),
                new KeyString(Keys.Z, 'z', 'Z'),
                new KeyString(Keys.X, 'x', 'X'),
                new KeyString(Keys.C, 'c', 'C'),
                new KeyString(Keys.V, 'v', 'V'),
                new KeyString(Keys.B, 'b', 'B'),
                new KeyString(Keys.N, 'n', 'N'),
                new KeyString(Keys.M, 'm', 'M'),

                new KeyString(Keys.D0, '0', ')'),
                new KeyString(Keys.D1, '1', '!'),
                new KeyString(Keys.D2, '2', '@'),
                new KeyString(Keys.D3, '3', '#'),
                new KeyString(Keys.D4, '4', '$'),
                new KeyString(Keys.D5, '5', '%'),
                new KeyString(Keys.D6, '6', '^'),
                new KeyString(Keys.D7, '7', '&'),
                new KeyString(Keys.D8, '8', '*'),
                new KeyString(Keys.D9, '9', '('),


                new KeyString(Keys.OemComma, ',', '<'),
                new KeyString(Keys.OemPeriod, '.', '>'),
                new KeyString(Keys.Space, ' ', ' '),
                new KeyString(Keys.OemQuestion, '/', '?'),
                new KeyString(Keys.OemMinus, '-', '_'),
                new KeyString(Keys.OemPlus, '=', '+'),
                new KeyString(Keys.OemQuotes, '\'', '\"'),
                new KeyString(Keys.OemSemicolon, ';', ':'),
                new KeyString(Keys.OemPipe, '\\', '|'),
                new KeyString(Keys.OemOpenBrackets, '[', '{'),
                new KeyString(Keys.OemCloseBrackets, ']', '}'),

                //new KeyString(Keys.Oem, '9', '('),
                //new KeyString(Keys.D9, '9', '('),



            };
        }
    }
}
