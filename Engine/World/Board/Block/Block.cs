using Engine.Api;
using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World.Board
{
    public partial class Block : IPollerObject
    {
        bool _isDeleted = false;

        public BlockId Id { get; set; }

        /// <summary>
        /// Id for pooler using
        /// </summary>
        int PoolerId { get; set; }

        public short Variant { get; set; }

        public bool IsUnderMap { get; set; }

        /// <summary>
        /// Left Top corner
        /// </summary>
        public Vector2 Position { get; private set; }

        public Point PositionInChunk { get; private set; }

        public RectangleF Bounds => new RectangleF(Position, ChunkParameters.BlockSize);

        public Block(BlockId id)
        {
            Id = id;

            Variant = 0;
        }

        public Block(BlockId id, Vector2 position, Point positionInChunk)
        {
            Id = id;

            Position = position;

            PositionInChunk = positionInChunk;

            Variant = 0;
        }

        public int GetPoolerId()
        {
            return PoolerId;
        }

        public void SetPollerId(int id)
        {
            PoolerId = id;
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
