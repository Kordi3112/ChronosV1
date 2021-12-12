using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public class BlocksLibrary : IDisposable
    {

        Tuple<string, Texture2D>[] _blocksLibrary;

        Queue<Tuple<uint, string>> _idsToLoad;

        BlockTemplate[] _blockTemplates;

        public BlocksLibrary(int maxBlocksNumber)
        {
            _blocksLibrary = new Tuple<string, Texture2D>[maxBlocksNumber];
            _blockTemplates = new BlockTemplate[maxBlocksNumber];
            //fill
            for (int i = 0; i < maxBlocksNumber; i++)
            {
                _blocksLibrary[i] = new Tuple<string, Texture2D>("Null", null);
            }

            _idsToLoad = new Queue<Tuple<uint, string>>();
        }

        public void DefineBlock(Block.BlockId id, string name)
        {
            DefineBlock((uint)id, name);

        }
        public void DefineBlock(uint id, string name)
        {
            _idsToLoad.Enqueue(new Tuple<uint, string>(id, name));
        }

        public bool Load(string path, ContentManager content)
        {
            while (_idsToLoad.Count > 0)
            {
                Tuple<uint, string> tuple = _idsToLoad.Dequeue();

                Texture2D texture = content.Load<Texture2D>(path + tuple.Item2);

                _blocksLibrary[tuple.Item1] = new Tuple<string, Texture2D>(tuple.Item2, texture);

                //cell
                _blockTemplates[tuple.Item1] = new BlockTemplate(content.Load<Texture2D>("TexturePacks/TestPack2/" + tuple.Item2), new Point(3, 3), new Vector2(2, 2), new Vector2(1f, 1f));

                //
            }

            //

            


            //
            return true;
        }

        public Texture2D GetBlockTexture(int id) => _blocksLibrary[id].Item2;
        public Texture2D GetBlockTexture(Block.BlockId id) => _blocksLibrary[(int)id].Item2;
        public BlockTemplate GetBlockTemplate(int id) => _blockTemplates[id];
        public BlockTemplate GetBlockTemplate(Block.BlockId id) => _blockTemplates[(int)id];
        public string GetBlockName(int id) => _blocksLibrary[id].Item1;

        public void Dispose()
        {
            foreach(Tuple<string, Texture2D> tuple in _blocksLibrary)
            {
                if (tuple.Item2 != null)
                    tuple.Item2.Dispose();
            }
        }
    }
}
