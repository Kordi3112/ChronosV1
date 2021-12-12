using Engine.EngineMath;
using Engine.EngineMath.GJK2;
using Engine.World.Board;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    partial class ObjectsController1
    {
        private void SolveCollison(PhysicalObject element1, PhysicalObject element2, int id1, int id2, float collsionTime)
        {
            GJK_2 gjk = new GJK_2();

            float t0 = GameManager.Time.DeltaTime - collsionTime;

            //Get Transforms from moment of collision
            Transform transformFromCollision1 = new Transform(element1.Transform.Position2 + GetPositionDiffFromPast(element1, t0), element1.Transform.Rotation + GetRotationDiffFromPast(element1, t0));
            Transform transformFromCollision2 = new Transform(element2.Transform.Position2 + GetPositionDiffFromPast(element2, t0), element2.Transform.Rotation + GetRotationDiffFromPast(element2, t0));

            //Polygons from collision moment
            Polygon polygon1 = GetPolygonFromPast(element1, element1.Hitbox.Polygons[id1], t0);
            Polygon polygon2 = GetPolygonFromPast(element2, element2.Hitbox.Polygons[id2], t0);
            //check collsiion to get closest points
            gjk.CheckCollision(polygon1, polygon2);

            Vector2 r1 = gjk.ClosestOnA - transformFromCollision1.Position2;
            Vector2 r2 = gjk.ClosestOnB - transformFromCollision2.Position2;

            Vector2 normal = gjk.ClosestOnA - gjk.ClosestOnB;
            normal.Normalize();

            //Get RBParams just before collision moment
            RigidBodyParameters rigidBodyParams1 = RigidBodyParameters.CreateFromPrevParams(element1.Rigidbody.Params);
            RigidBodyParameters rigidBodyParams2 = RigidBodyParameters.CreateFromPrevParams(element2.Rigidbody.Params);

            //calculate collision diffs
            var tuple = Rigidbody.GetCollisionMovementDiffs(rigidBodyParams1, rigidBodyParams2, r1, r2, normal);
            //apply collision diffs
            rigidBodyParams1.Velocity += tuple.Item1;
            rigidBodyParams2.Velocity += tuple.Item2;
            rigidBodyParams1.Omega += tuple.Item3;
            rigidBodyParams2.Omega += tuple.Item4;

            //Calculate Post-Collision correct position 
            Transform correctTransform1 = new Transform()
            {
                Position2 = transformFromCollision1.Position2 + rigidBodyParams1.Velocity * t0 + 0.5f * rigidBodyParams1.Acceleration * t0 * t0,
                Rotation = transformFromCollision1.Rotation + rigidBodyParams1.Omega * t0 + 0.5f * rigidBodyParams1.Epsilon * t0 * t0,
            };

            Transform correctTransform2 = new Transform()
            {
                Position2 = transformFromCollision2.Position2 + rigidBodyParams2.Velocity * t0 + 0.5f * rigidBodyParams2.Acceleration * t0 * t0,
                Rotation = transformFromCollision2.Rotation + rigidBodyParams2.Omega * t0 + 0.5f * rigidBodyParams2.Epsilon * t0 * t0,
            };

            //correct pos
            element1.Rigidbody.PositionToAdd += correctTransform1.Position2 - element1.Transform.Position2;
            element2.Rigidbody.PositionToAdd += correctTransform2.Position2 - element2.Transform.Position2;
            //correct rot
            element1.Rigidbody.RotationToAdd += correctTransform1.Rotation - element1.Transform.Rotation;
            element2.Rigidbody.RotationToAdd += correctTransform2.Rotation - element2.Transform.Rotation;

            //set correct velocity
            element1.Rigidbody.VelocityToAdd += rigidBodyParams1.Velocity - element1.Rigidbody.Params.Velocity;
            element2.Rigidbody.VelocityToAdd += rigidBodyParams2.Velocity - element2.Rigidbody.Params.Velocity;
            //set correct omega
            element1.Rigidbody.OmegaToAdd += rigidBodyParams1.Omega - element1.Rigidbody.Params.Omega;
            element2.Rigidbody.OmegaToAdd += rigidBodyParams2.Omega - element2.Rigidbody.Params.Omega;

            //
            element1.OnCollision(element2.Info);
            element2.OnCollision(element1.Info);

        }

        private void SolveCollision(PhysicalObject element1, Block block, int id1, float collsionTime, Vector2 penetrationVector)
        {
            GJK_2 gjk = new GJK_2();

            Polygon polygon1;
            //block
            Polygon polygon2 = new Polygon(new Vector2[] { block.Bounds.LeftTop, block.Bounds.RightTop, block.Bounds.RightBot, block.Bounds.LeftBot });

            if (collsionTime < 0)
            {               

                if (gjk.IsCollision == false || float.IsNaN(penetrationVector.X))
                    return;

                
                penetrationVector.Normalize();

                if (Vector2.Dot(element1.Transform.Position2 - block.Bounds.Center, penetrationVector) < 0)
                    penetrationVector *= -1;

                //Debug.WriteLine("Before: " + element1.Rigidbody.Params.Velocity);
                //Debug.WriteLine("Pv: " + penetrationVector);
                // Debug.WriteLine("Collision: " + gjk.IsCollision);




                //check if element.Velocity has opposite sign

                if (Vector2.Dot(element1.Rigidbody.Params.Velocity, penetrationVector) <= 0)
                {

                    float cosAlpha = penetrationVector.X;
                    float sinAlpha = penetrationVector.Y;


                    Vector2 v = -element1.Rigidbody.Params.Velocity;

                    //rotate Alpha
                    v = Tools.Rotate(v, -sinAlpha, cosAlpha);
                    v.Y = -v.Y;
                    //rotate -Alpha
                    v = Tools.Rotate(v, sinAlpha, cosAlpha);

                    element1.Rigidbody.VelocityToAdd += v - element1.Rigidbody.Params.Velocity;
                    element1.Rigidbody.OmegaToAdd += -0.5f *element1.Rigidbody.Params.Omega - element1.Rigidbody.Params.Omega;
                }
                else
                {
                    Vector2 v = element1.Rigidbody.Params.Velocity + penetrationVector;

                    v.Normalize();
                    element1.Rigidbody.VelocityToAdd += element1.Rigidbody.Params.Velocity.Length() * v - element1.Rigidbody.Params.Velocity;
                    element1.Rigidbody.OmegaToAdd += -element1.Rigidbody.Params.Omega * 0.1f;
                }
                //Mirror reflection



               // element1.Rigidbody.Params.Velocity =  v;
               // element1.Rigidbody.Params.Omega =  0;
                
               // Debug.WriteLine("After: " + element1.Rigidbody.Params.Velocity);
                return;
            }



            float t0 = GameManager.Time.DeltaTime - collsionTime;

            //Get Transforms from moment of collision
            Transform transformFromCollision1 = new Transform(element1.Transform.Position2 + GetPositionDiffFromPast(element1, t0), element1.Transform.Rotation + GetRotationDiffFromPast(element1, t0));

            //Polygons from collision moment
            polygon1 = GetPolygonFromPast(element1, element1.Hitbox.Polygons[id1], t0);
            polygon2 = new Polygon(new Vector2[] { block.Bounds.LeftTop, block.Bounds.RightTop, block.Bounds.RightBot, block.Bounds.LeftBot });

            gjk.CheckCollision(polygon1, polygon2);

            Vector2 r1 = gjk.ClosestOnA - transformFromCollision1.Position2;
            Vector2 r2 = gjk.ClosestOnB - block.Bounds.Center;

            Vector2 normal = gjk.ClosestOnA - gjk.ClosestOnB;
            normal.Normalize();

            closestOnA = gjk.ClosestOnA;
            closestOnB = gjk.ClosestOnB;
            Normal = normal;

            RigidBodyParameters rigidBodyParams1 = RigidBodyParameters.CreateFromPrevParams(element1.Rigidbody.Params);
            RigidBodyParameters rigidBodyParams2 = new RigidBodyParameters()
            {
                Mass = float.MaxValue,
                Inertia = float.MaxValue,
                Elasticity = 0.4f,
            };

            //calculate collision diffs
            var tuple = Rigidbody.GetCollisionMovementDiffs(rigidBodyParams1, rigidBodyParams2, r1, r2, normal);

            //apply collision diffs
            rigidBodyParams1.Velocity += tuple.Item1;
            rigidBodyParams1.Omega += tuple.Item3;

            //Calculate Post-Collision correct position 
            Transform correctTransform1 = new Transform()
            {
                Position2 = transformFromCollision1.Position2 + rigidBodyParams1.Velocity * t0 + 0.5f * rigidBodyParams1.Acceleration * t0 * t0,
                Rotation = transformFromCollision1.Rotation + rigidBodyParams1.Omega * t0 + 0.5f * rigidBodyParams1.Epsilon * t0 * t0,
            };

            //correct pos
            element1.Rigidbody.PositionToAdd += correctTransform1.Position2 - element1.Transform.Position2;
            //correct rot
            element1.Rigidbody.RotationToAdd += correctTransform1.Rotation - element1.Transform.Rotation;

            //set correct velocity
            element1.Rigidbody.VelocityToAdd += rigidBodyParams1.Velocity - element1.Rigidbody.Params.Velocity;
            //set correct omega
            element1.Rigidbody.OmegaToAdd += rigidBodyParams1.Omega - element1.Rigidbody.Params.Omega;

        }

        private float FindCollisionTime(PhysicalObject element1, PhysicalObject element2, int id1, int id2)
        {
            GJK_2 gjk = new GJK_2();

            //Find t0 in [left, right]
            float left = 0;
            float right = GameManager.Time.DeltaTime;

            float distanceTolerance2 = 0.01f * 0.01f;

            Polygon polygon1;
            Polygon polygon2;

            //Debug_DrawDiagram(element1, id1, element2, id2);
            //
            int count = 0;
            while (true)
            {
                count++;

                if (count > 200)
                    break;

                float t = (left + right) * 0.5f;

                //sample polygons from time = t
                polygon1 = GetPolygonFromPast(element1, element1.Hitbox.Polygons[id1], GameManager.Time.DeltaTime - t);
                polygon2 = GetPolygonFromPast(element2, element2.Hitbox.Polygons[id2], GameManager.Time.DeltaTime - t);
                //
                gjk.CheckCollision(polygon1, polygon2, false);

                if (gjk.IsCollision || gjk.ClosestOnA == gjk.ClosestOnB)
                {
                    //change interval to [left, t]
                    right = t;
                    continue;
                }
                //calculate distance 
                float d2 = (gjk.ClosestOnA - gjk.ClosestOnB).LengthSquared();

                //check if distance is in tolerance
                if (d2 <= distanceTolerance2)
                {
                    //Debug.WriteLine("Count " + count + " D: " + d2);
                    return t;
                }


                //change interval to [t, right]
                left = t;
            }

            //Debug_DrawDiagram(element1, id1, element2, id2);
            return 0;
        }

        private float FindCollisionTime(PhysicalObject element, Polygon blockPolygon, int id1)
        {
            GJK_2 gjk = new GJK_2();

            //Find t0 in [left, right]
            float left = 0;
            float right = GameManager.Time.DeltaTime;

            float distanceTolerance2 = 0.01f * 0.01f;

            //

            Polygon polygon1 = GetPolygonFromPast(element, element.Hitbox.Polygons[id1], GameManager.Time.DeltaTime);

            gjk.CheckCollision(polygon1, blockPolygon, false);

            if (gjk.IsCollision)
            {
                //Debug.WriteLine("Collision");

                //Debug_DrawDiagram(element, id1, blockPolygon);
                return -1;
                //return 0.5f * GameManager.Time.DeltaTime;

            }
           // else Debug_DrawDiagram(element, id1, blockPolygon);

            while (true)
            {
                float t = (left + right) * 0.5f;

                polygon1 = GetPolygonFromPast(element, element.Hitbox.Polygons[id1], GameManager.Time.DeltaTime - t);

                gjk.CheckCollision(polygon1, blockPolygon, false);

                if (gjk.IsCollision || gjk.ClosestOnA == gjk.ClosestOnB)
                {
                    //change interval to [left, t]
                    right = t;
                    continue;
                }

                //calculate distance 
                float d2 = (gjk.ClosestOnA - gjk.ClosestOnB).LengthSquared();

                //check if distance is in tolerance
                if (d2 <= distanceTolerance2)
                {
                    return t;
                }

                //change interval to [t, right]
                left = t;
            }
        }

        private void Debug_DrawDiagram(PhysicalObject element, int id1, Polygon blockPolygon)
        {
            GJK_2 gjk = new GJK_2();
            Debug.WriteLine("--------");
            float t = 0;
            for (int i = 0; i < 100; i++)
            {
                t = (float)i / 100 * GameManager.Time.DeltaTime;

                Polygon polygon1 = GetPolygonFromPast(element, element.Hitbox.Polygons[id1], GameManager.Time.DeltaTime - t);

                gjk.CheckCollision(polygon1, blockPolygon, true);

                float d2 = (gjk.ClosestOnA - gjk.ClosestOnB).LengthSquared();

                Debug.WriteLine(i + ": " + d2 + " Pen: " + gjk.PenetrationVector);
            }

            Debug.WriteLine("--------");
        }

        private void Debug_DrawDiagram(PhysicalObject element1, int id1, PhysicalObject element2, int id2)
        {
            GJK_2 gjk = new GJK_2();
            Debug.WriteLine("--------");
            float t = 0;
            for (int i = 0; i < 100; i++)
            {
                t = (float)i / 100 * GameManager.Time.DeltaTime;

                Polygon polygon1 = GetPolygonFromPast(element1, element1.Hitbox.Polygons[id1], GameManager.Time.DeltaTime - t);
                Polygon polygon2 = GetPolygonFromPast(element2, element2.Hitbox.Polygons[id2], GameManager.Time.DeltaTime - t);

                gjk.CheckCollision(polygon1, polygon2, true);

                float d2 = (gjk.ClosestOnA - gjk.ClosestOnB).LengthSquared();

                Debug.WriteLine(i + ": " + d2 + " Pen: " + gjk.PenetrationVector + " Pen L: " + gjk.PenetrationVector.LengthSquared());
            }

            Debug.WriteLine("--------");
        }
    }
}
