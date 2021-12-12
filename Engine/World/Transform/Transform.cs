using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    /// <summary>
    /// Full Tranforms funcionality - Matrices, Updating only by user (no auto)
    /// </summary>
    public class Transform : IEquatable<Transform>
    {
        protected Vector3 _position;
        protected float _rotation;


        bool _rotChange = true;
        public Matrix RotationMatrix { get; private set; }
        public Matrix PositionMatrix { get; private set; }
        public Matrix Matrix { get; private set; }

        public static Transform Zero => new Transform();

        public Vector3 Position
        {
            get => _position;

            set => SetPosition(value);
        }

        public Vector2 Position2
        {
            get => new Vector2(_position.X, _position.Y);

            set => SetPosition(value.X, value.Y);
        }

        public float X
        {
            get => _position.X;

            set => SetX(value);
        }

        public float Y
        {
            get => _position.Y;

            set => SetY(value);
        }

        public float Z
        {
            get => _position.Z;

            set => SetZ(value);
        }

        public float Rotation
        {
            get => _rotation;

            set => SetRotation(value);
        }

        public virtual void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public virtual void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public virtual void SetX(float val) => _position.X = val;
        public virtual void SetY(float val) => _position.Y = val;
        public virtual void SetZ(float val) => _position.Z = val;

        public virtual void SetRotation(float val)
        {
            _rotation = val;
            _rotChange = true;
        }

        public Transform(Vector3 position = new Vector3(), float rotation = 0, bool update = true)
        {

            Position = position;
            Rotation = rotation;

            if(update)
                Update(true);

        }

        public Transform(Vector2 position, float rotation = 0, bool update = true)
        {

            Position = new Vector3(position, 0);
            Rotation = rotation;

            if (update)
                Update(true);

        }

        public Transform(Transform transform)
        {
            //copy

            Copy(transform);
        }

        public virtual void Update(bool forceUpdate = false)
        {
            if(forceUpdate)
            {
                UpdatePositionMatrix();
                UpdateRotationMatrix();
                UpdateMatrix();

                return;
            }

            if (_rotChange)
                UpdateRotationMatrix();

            UpdatePositionMatrix();
            UpdateMatrix();
        }

        protected void UpdatePositionMatrix()
        {
            PositionMatrix = Matrix.CreateTranslation(_position);
        }

        protected void UpdateRotationMatrix()
        {
            RotationMatrix = Matrix.CreateRotationZ(_rotation);
            _rotChange = false;
        }

        protected void UpdateMatrix()
        {
            Matrix = RotationMatrix * PositionMatrix;
        }

        public void Copy(Transform transform)
        {
            _position = transform._position;
            _rotation = transform._rotation;
            Matrix = transform.Matrix;
            RotationMatrix = transform.RotationMatrix;
            _rotChange = transform._rotChange;
        }

        public static Transform operator+(Transform A, Transform B)
        {
            return new Transform(A._position + B._position, A._rotation + B._rotation);
        }

        public bool Equals(Transform other)
        {
            if (Position == other.Position && Rotation == other.Rotation)
                return true;
            else return false;
        }

        public override string ToString()
        {
            return "{"+ Position + " " + Rotation +"}";
        }
    }
}
