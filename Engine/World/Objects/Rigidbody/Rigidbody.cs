using Engine.EngineMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class Rigidbody
    {
        public enum ForceType
        {
            Impulse,
            Continous,
        }
        public Transform Transform { get; set; }

        public RigidBodyParameters Params { get; set; }

        public Vector2 AccelerationToAdd { get;  set; }
        public Vector2 VelocityToAdd { get; set; }

        public float EpsilonToAdd { get; set; }
        public float OmegaToAdd { get; set; }

        public Vector2 PositionToAdd { get; set; } 
        public float RotationToAdd { get; set; }

        public List<string> IgnoreTags = new List<string>();

        ///copy

        public Rigidbody(PhysicalObject obj)
        {
            IgnoreTags = new List<string>();
            Transform = obj.Transform;
            Params = new RigidBodyParameters();
            Params.Default();
        }

        internal void ClearToAdd()
        {
            AccelerationToAdd = Vector2.Zero;
            VelocityToAdd = Vector2.Zero;
            EpsilonToAdd = 0;
            OmegaToAdd = 0;

            PositionToAdd = Vector2.Zero;
            RotationToAdd = 0;
        }

        public void AddForce(Force force)
        {
            //force.Point is r
  
            if (force.Value == Vector2.Zero)
                return;

            if(force.Type == Force.ForceType.Continous)
            {
                if(force.Point == Vector2.Zero)
                {
                    AccelerationToAdd += force.Value * Params.InvMass;
                   
                }
                else
                {
                    float cos = Tools.CosAngleBetween(force.Point, force.Value);
                   // float sin = (float)Math.Sqrt(1 - cos * cos);

                    AccelerationToAdd += force.Value * Math.Abs(cos) * Params.InvMass;

                    EpsilonToAdd += Tools.MulZ(force.Point, force.Value) * Params.InvInertia;

                    //Debug.WriteLine(cos + " " + force.Point + " " + force.Value);
                }


               
            }
            else
            {
                if (force.Point == Vector2.Zero)
                {
                    VelocityToAdd += force.Value * Params.InvMass;

                }
                else
                {
                    VelocityToAdd += force.Value * Math.Abs(Tools.CosAngleBetween(force.Point, force.Value)) * Params.InvMass;

                    OmegaToAdd += Tools.MulZ(force.Point, force.Value) * Params.InvInertia;
                }


                
            }
        }

        public static void CalculateCollision(Rigidbody A, Rigidbody B, Vector2 r1, Vector2 r2, Vector2 n)
        {
            Vector2 v1 = A.Params.Velocity;
            Vector2 v2 = B.Params.Velocity;

            Vector2 vp1 = v1 + Tools.ToVector2(Tools.Mul(A.Params.Omega3, new Vector3(r1, 0)));
            Vector2 vp2 = v2 + Tools.ToVector2(Tools.Mul(B.Params.Omega3, new Vector3(r2, 0)));

            Vector2 vr = vp2 - vp1;


            float e = A.Params.Elasticity * B.Params.Elasticity;

            //calculate jr

            // r1.Normalize();
            //r2.Normalize();
            e = 0.5f;
            float jr_up = -Tools.Dot((1 + e) * vr, n);

            float jr_down = A.Params.InvMass + B.Params.InvMass + 0.01f * Tools.Dot(Tools.Mul(A.Params.InvInertia * Tools.Mul(r1, n), r1) + Tools.Mul(B.Params.InvInertia * Tools.Mul(r2, n), r2), n);

            float jr =  jr_up / jr_down;

            
            //Calculate post velocities
            Vector2 vAToAdd = -jr * A.Params.InvMass * n;
            Vector2 vBToAdd = jr * B.Params.InvMass * n;
            
            A.VelocityToAdd += vAToAdd * 1;
            B.VelocityToAdd +=  vBToAdd * 1;

            A.OmegaToAdd += -jr * A.Params.InvInertia * Tools.MulZ(r1, n) * 0.5f;
            B.OmegaToAdd += jr * B.Params.InvInertia * Tools.MulZ(r2, n) * 0.5f;
            

        }



        public static Tuple<Vector2, Vector2, float, float> GetCollisionMovementDiffs(RigidBodyParameters A, RigidBodyParameters B, Vector2 R1, Vector2 R2, Vector2 N)
        {
            //Vector2 v1 = A.Velocity;
            //Vector2 v2 = B.Velocity;

            //Vector2 vp1 = v1 + Tools.ToVector2(Tools.Mul(A.Omega3, new Vector3(r1, 0)));
            //Vector2 vp2 = v2 + Tools.ToVector2(Tools.Mul(B.Omega3, new Vector3(r2, 0)));

            //Vector2 vr = vp2 - vp1;


            float e = A.Elasticity * B.Elasticity;

            Vector3 v1 = new Vector3(A.Velocity, 0);
            Vector3 v2 = new Vector3(B.Velocity, 0);

            Vector3 r1 = new Vector3(R1, 0);
            Vector3 r2 = new Vector3(R2, 0);

            Vector3 n = new Vector3(N, 0);

            Vector3 vp1 = v1 + Vector3.Cross(A.Omega3, r1);
            Vector3 vp2 = v2 + Vector3.Cross(B.Omega3, r2);

            Vector3 vr = vp2 - vp1;
            //calculate jr

            /*
            float jr_up = Tools.Dot(-(1 + e) * vr, n);

            //float jr_down = A.InvMass + B.InvMass + Tools.Dot(Tools.Mul(A.Params.InvInertia * Tools.Mul(r1, n), r1) + Tools.Mul(B.Params.InvInertia * Tools.Mul(r2, n), r2), n);
            float jr_down = A.InvMass + B.InvMass;// + Vector2.Dot(Tools.Mul(0.3f * A.InvInertia * Tools.Mul(r1, n), r1) + Tools.Mul(0.3f * B.InvInertia * Tools.Mul(r2, n), r2), n);


            float jr = jr_up / jr_down;
            */

            float impulseForce = Vector3.Dot(vr, n);

            Vector3 inertiaA = Vector3.Cross(A.InvInertia * Vector3.Cross(r1, n), r1);
            Vector3 inertiaB = Vector3.Cross(B.InvInertia * Vector3.Cross(r2, n), r2);

            float totalMass = A.InvMass + B.InvMass;

            float angularEffect = Vector3.Dot(inertiaA + inertiaB, n);


            float jr = (-(1 + e) * impulseForce) / (totalMass + angularEffect);


            //Debug.WriteLine("jr: " + jr);
            //Debug.WriteLine("iF: " + impulseForce);
            //Debug.WriteLine("inertiaA: " + inertiaA);
            //Debug.WriteLine("inertiaB: " + inertiaB);
            //Debug.WriteLine("totalMass: " + totalMass);
            //Debug.WriteLine("angularEffect: " + angularEffect);



            //Calculate post velocities

            Vector2 velocityToAddA = Tools.ToVector2(-jr * A.InvMass * n);
            Vector2 velocityToAddB = Tools.ToVector2(jr * B.InvMass * n);


            float omegaToAddA = (-jr * A.InvInertia * Vector3.Cross(r1, n)).Z;
            float omegaToAddB = (jr * B.InvInertia * Vector3.Cross(r2, n)).Z ;

            return new Tuple<Vector2, Vector2, float, float>(velocityToAddA, velocityToAddB, omegaToAddA, omegaToAddB);
        }
    }
}
