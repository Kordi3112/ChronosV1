using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control
{
    public class CommandLine
    {
        public static Color DefaultColor = Color.White;

        List<Tuple<string, Color>> _phrases;

        public int PhrasesCount => _phrases.Count;

        public string TotalString { get; private set; }
        /// <summary>Message time</summary>
        public float Time { get; internal set; }

        public CommandLine()
        {
            _phrases = new List<Tuple<string, Color>>();
        }

        public void AddPhrase(string text)
        {
            _phrases.Add(new Tuple<string, Color>(text, DefaultColor));
            TotalString += text;
        }

        public void AddPhrase(string text, Color color)
        {
            _phrases.Add(new Tuple<string, Color>(text, color));
            TotalString += text;
        }


        public Color GetPhraseColor(int id) => _phrases[id].Item2;
        public string GetPhraseString(int id) => _phrases[id].Item1;
    }
}
