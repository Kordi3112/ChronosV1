using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class RigidBodyParameters
    {
        //LINEAR
        public Vector2 Velocity { get; set; }

        internal Vector2 PrevVelocity { get; set; }

        public Vector2 Acceleration { get; set; }

        internal Vector2 PrevAcceleration { get; set; }

        //ANGULAR

        public float Omega { get; set; }

        internal float PrevOmega { get; set; }
        public Vector3 Omega3 => new Vector3(0, 0, Omega);

        public float Epsilon { get; set; }

        internal float PrevEpsilon { get; set; }

        public Vector3 Epsilon3 => new Vector3(0, 0, Epsilon);

        //OTHER

        public float Mass { get; set;}
        public float InvMass => 1.0f / Mass;
        public float Inertia { get; set; }
        public float InvInertia => 1.0f / Inertia;

        public float Elasticity { get; set; }

        public float LinearResistance { get; set; }

        ///<summary>Resistance in forward direction</summary>
        public float LinearHorizontalResistance { get; set; }

        ///<summary>Resistance in side direction</summary>
        public float LinearVerticalResistance { get; set; }
        public float AngularResistance { get; set; }

        public RigidBodyParameters(RigidBodyParameters rigidBodyParameters)
        {
            Velocity = rigidBodyParameters.Velocity;
            Acceleration = rigidBodyParameters.Acceleration;
            Omega = rigidBodyParameters.Omega;
            Epsilon = rigidBodyParameters.Epsilon;

            Mass = rigidBodyParameters.Mass;
            Inertia = rigidBodyParameters.Inertia;
            Elasticity = rigidBodyParameters.Elasticity;

            LinearResistance = rigidBodyParameters.LinearResistance;
            LinearHorizontalResistance = rigidBodyParameters.LinearHorizontalResistance;
            LinearVerticalResistance = rigidBodyParameters.LinearVerticalResistance;
            AngularResistance = rigidBodyParameters.AngularResistance;
        }

        public static RigidBodyParameters CreateFromPrevParams(RigidBodyParameters rigidBodyParameters)
        {
            return new RigidBodyParameters() {
                Velocity = rigidBodyParameters.PrevVelocity,
                Acceleration = rigidBodyParameters.PrevAcceleration,
                Omega = rigidBodyParameters.PrevOmega,
                Epsilon = rigidBodyParameters.PrevEpsilon,

                Mass = rigidBodyParameters.Mass,
                Inertia = rigidBodyParameters.Inertia,
                Elasticity = rigidBodyParameters.Elasticity,

                LinearResistance = rigidBodyParameters.LinearResistance,
                LinearHorizontalResistance = rigidBodyParameters.LinearHorizontalResistance,
                LinearVerticalResistance = rigidBodyParameters.LinearVerticalResistance,
                AngularResistance = rigidBodyParameters.AngularResistance,

            };
        }

        public RigidBodyParameters()
        {
            Default();
        }

        public void Default()
        {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Omega = 0;
            Epsilon = 0;
            Mass = 1;
            Inertia = 1;
            Elasticity = 1;

            LinearResistance = 0;
            AngularResistance = 0;

            LinearHorizontalResistance = 0;
            LinearVerticalResistance = 0;
        }

        internal void SavePrevParams()
        {
            PrevVelocity = Velocity;
            PrevAcceleration = Acceleration;
            PrevOmega = Omega;
            PrevEpsilon = Epsilon;
        }
    }
}
