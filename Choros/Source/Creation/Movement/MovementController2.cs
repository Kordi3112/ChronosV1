using Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Creation
{
    class MovementController2
    {
        List<Engine2> _enginesBackward;
        List<Engine2> _enginesForward;

        Rigidbody Rigidbody { get; set; }

        [Flags]
        public enum MovementType
        {
            None = 0,
            Forward = 0x00000001,
            Backward = 0x00000010,
            RotateLeft = 0x00000100,
            RotateRight = 0x00001000,
        }

        public MovementController2(Rigidbody rigidbody)
        {
            Rigidbody = rigidbody;

            _enginesBackward = new List<Engine2>();
            _enginesForward = new List<Engine2>();
        }

        public void Add(Engine2 engine)
        {
            if (engine.Position.X < 0)
                _enginesBackward.Add(engine);
            else _enginesForward.Add(engine);
        }

        public void Update(MovementType movementType, float deltaTime)
        {
            if(movementType.HasFlag(MovementType.Forward))
            {
                if(movementType.HasFlag(MovementType.RotateLeft))
                {

                }
            }


            ApplyForces();
        }

        private void ApplyForces()
        {
            foreach(var engine in _enginesBackward)
            {
                engine.RealRotation = (float)Math.Atan2(engine.F.Y, engine.F.X);
                engine.Rotation = engine.RealRotation - Rigidbody.Transform.Rotation;
                Rigidbody.AddForce(new Force(engine.RealPosition, engine.F, Force.ForceType.Continous));
            }

            foreach (var engine in _enginesForward)
            {
                engine.RealRotation = (float)Math.Atan2(engine.F.Y, engine.F.X);
                engine.Rotation = engine.RealRotation - Rigidbody.Transform.Rotation;
                Rigidbody.AddForce(new Force(engine.RealPosition, engine.F, Force.ForceType.Continous));
            }

        }

        private void CalculateRotation()
        {

        }
    }
}
