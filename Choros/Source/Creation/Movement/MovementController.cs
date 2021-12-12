using Engine.EngineMath;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Creation
{
    /// <summary>
    /// Help to set Force from engines
    /// </summary>
    class MovementController
    {

        List<Engine> _engines;
        Rigidbody Rigidbody { get; set; }

        /// <summary>
        /// Movement type based on Input
        /// </summary>
        public enum MovementType
        {
            FullForward,
            FullBackward,
            FullRotateLeft,
            FullRotateRight,
            None,
            ForwardRotateLeft,
            ForwardRotateRight,
        }

        public MovementController(Rigidbody rigidbody)
        {
            _engines = new List<Engine>();

            Rigidbody = rigidbody;
        }

        public void Add(Engine engine)
        {
            _engines.Add(engine);
        }

        public int EngineCount => _engines.Count;
        public Engine GetEngine(int id) => _engines[id];

        public void Update3(MovementType type)
        {
            UpdateEnginesPos();

            if (type == MovementType.None)
            {
                CalculateNone();
                return;
            }
                
        }
        public void Update(MovementType type)
        {
            if (type == MovementType.None)
            {
                CalculateNone();
                return;
            }

            UpdateEnginesPos();

            float rotationVelocityTolerance = 10.05f;
            float rotationToApply = 0;

            if (type == MovementType.FullForward)
            {
                CalculateFullForward();
                return;
            }
            else if (type == MovementType.FullRotateLeft)
            {
                rotationToApply = MathHelper.Pi / 2f;
            }
            else if (type == MovementType.FullRotateRight)
            {
                rotationToApply = -MathHelper.Pi / 2f;
            }
            else if (type == MovementType.ForwardRotateLeft)
            {
                rotationToApply = MathHelper.Pi / 6f;
            }
            else if (type == MovementType.ForwardRotateRight)
            {
                rotationToApply = -MathHelper.Pi / 6f;
            }

            float value = 500;

            foreach (Engine engine in _engines)
            {
                
               

                Vector2 F = Tools.Rotate(-Vector2.Normalize(engine.RealPosition), rotationToApply) * value;

                
                
                engine.RealRotation = (float)Math.Atan2(F.Y, F.X);
                engine.Rotation = engine.RealRotation - Rigidbody.Transform.Rotation;

                Rigidbody.AddForce(new Force(engine.RealPosition, F, Force.ForceType.Continous));
            }
        }

        public void Update2(MovementType type)
        {
            if (type == MovementType.None)
                return;

            UpdateEnginesPos();


            float rotationVelocityTolerance = 10.05f;

            if(type == MovementType.FullForward)
            {
                //negate rotation
                if(Math.Abs(Rigidbody.Params.Omega) > rotationVelocityTolerance)
                {

                    foreach (var item in _engines)
                    {
                        item.Rotation =  Math.Sign(Rigidbody.Params.Omega) * MathHelper.Pi / 6f;
                    }
                }
                else
                {
                    foreach (var item in _engines)
                    {
                        item.Rotation = 0;
                    }
                }
            }
            else if(type == MovementType.FullRotateLeft)
            {
                foreach (var item in _engines)
                {
                    item.Rotation = MathHelper.Pi / 4f;
                }
            }
            else if (type == MovementType.FullRotateRight)
            {
                foreach (var item in _engines)
                {
                    item.Rotation = -MathHelper.Pi / 4f;
                }
            }
            else if(type == MovementType.ForwardRotateLeft)
            {
                foreach (var item in _engines)
                {
                    item.Rotation = MathHelper.Pi / 6f;
                }
            }
            else if (type == MovementType.ForwardRotateRight)
            {
                foreach (var item in _engines)
                {
                    item.Rotation = -MathHelper.Pi / 6f;
                }
            }

            float value = 500;

            foreach(Engine engine in _engines)
            {
                engine.RealRotation = engine.Rotation + Rigidbody.Transform.Rotation;
                Rigidbody.AddForce(new Force(engine.RealPosition, value * Tools.AngleToNormal(engine.RealRotation), Force.ForceType.Continous));
            }

        }

        private void UpdateEnginesPos()
        {
            foreach (var item in _engines)
            {
                item.RealPosition = Tools.Mul(item.Position, Rigidbody.Transform.RotationMatrix);
            }
        }

        private void CalculateNone()
        {
            float rotationVelocityTolerance = 0.02f;

            if (Math.Abs(Rigidbody.Params.Omega) < rotationVelocityTolerance)
            {
                foreach (Engine engine in _engines)
                    engine.Rotation = 0;

                return;
            }

            float rotationToApply = 0;

            //Negate rotation

            if(Rigidbody.Params.Omega > 0 )
                rotationToApply = MathHelper.Pi / 2f;
            else rotationToApply = -MathHelper.Pi / 2f;

            float value = 500 / 2;

            foreach (Engine engine in _engines)
            {
                Vector2 F = Tools.Rotate(-Vector2.Normalize(engine.RealPosition), rotationToApply) * value;

                engine.RealRotation = (float)Math.Atan2(F.Y, F.X);
                engine.Rotation = engine.RealRotation - Rigidbody.Transform.Rotation;

                Rigidbody.AddForce(new Force(engine.RealPosition, F, Force.ForceType.Continous));
            }
        }

        private void CalculateFullForward()
        {
            float rotationVelocityTolerance = 0.05f;

            foreach (Engine engine in _engines)
            {
                //store rotation to apply in:
                engine.Rotation = Rigidbody.Transform.Rotation - (float)Math.Atan2(-engine.RealPosition.Y, -engine.RealPosition.X);



            }

            float value = 500;

            foreach (Engine engine in _engines)
            {



                Vector2 F = Tools.Rotate(-Vector2.Normalize(engine.RealPosition), engine.Rotation) * value;

                

                engine.RealRotation = (float)Math.Atan2(F.Y, F.X);
                engine.Rotation = engine.RealRotation - Rigidbody.Transform.Rotation;

                Rigidbody.AddForce(new Force(engine.RealPosition, F, Force.ForceType.Continous));
            }
        }

        private void CalculateFullRotate()
        {

        }
    }
}
