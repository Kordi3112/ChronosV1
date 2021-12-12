using Engine.Control;
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
    partial class ObjectsController1 : ObjectsController
    {
        public Vector2 closestOnA;
        public Vector2 closestOnB;
        public Vector2 Normal;

        bool stop = false;
        public ObjectsController1(WorldManager worldManager) : base(worldManager)
        {

        }

        public override void UpdateObjects(ObjectsPooler objects, Map map)
        {
            //Recalculate Pos

            UpdateRigidBody(objects);

            //
            objects.ActionForeachActive(element => element.Hitbox.Update());


            objects.ActionForeachActive(element => element.Update());

            WorldManager.ObjectsPooler.RemoveFromList();
            WorldManager.ObjectsPooler.AddFromList();

            UpdateApperanceInfo(objects);
            

            //Collision detection

            ObjectsCollisionDetection(objects);

            //Map Coolision detection
            MapCollisionDetection(objects, map);
                
        }

        private void MapCollisionDetection(ObjectsPooler objects, Map map)
        {
            objects.ActionForeachActive(element => {

                if (!element.ObjectApperanceInfo.IsLandCollisionOn)
                    return;

                BlockInfo blockInfo =  map.CurrentDimension.GetBlockInfo(element.Transform.Position2);

                GJK_2 gjk = new GJK_2();
                Tuple<float, Block, int, Vector2> earliestCollision = null;

                map.CurrentDimension.ActionForEachSurroundingChunk(blockInfo, 1, chunk => {

                    chunk.UpperBlocksPooler.ActionForeachActive(block => {

                        if (!block.Bounds.Intersect(element.Hitbox.RealBounds))
                            return;
                        //check collision
                        Polygon blockPolygon = new Polygon(new Vector2[] { block.Bounds.LeftTop, block.Bounds.RightTop, block.Bounds.RightBot, block.Bounds.LeftBot });

                        int polCount = -1;
                        foreach (var polygon in element.Hitbox.RealPolygons)
                        {
                            polCount++;

                            gjk.CheckCollision(polygon, blockPolygon, false);

                            if (!gjk.IsCollision)
                                continue;
                            if (stop)
                                return;
                            float collisionTime = FindCollisionTime(element, blockPolygon, polCount);


                            //if (collisionTime < 0)
                            //{
                            //    gjk.CalculatePenetrationVector();

                            //    if (earliestCollision == null)
                            //        earliestCollision = new Tuple<float, Block, int, Vector2>(collisionTime, block, polCount, gjk.PenetrationVector);
                            //    else
                            //    {
                            //        if (earliestCollision.Item4.LengthSquared() > gjk.PenetrationVector.LengthSquared())
                            //            earliestCollision = new Tuple<float, Block, int, Vector2>(collisionTime, block, polCount, gjk.PenetrationVector);
                            //    }
                            //}
                                

                            if (earliestCollision == null)
                                earliestCollision = new Tuple<float, Block, int, Vector2>(collisionTime, block, polCount, Vector2.Zero);
                            else
                            {
                                if (earliestCollision.Item1 > collisionTime)
                                    earliestCollision = new Tuple<float, Block, int, Vector2>(collisionTime, block, polCount, Vector2.Zero);
                            }
                        }

                    });


                });



               //TODO: check all chunks
                //Chunk chunk = map.CurrentDimension.GetQuarter(Board.Quarter.QuarterAlign.RightBot).GetChunk(new Point(0,0));

               // if (!chunk.Bounds.Contains(element.Hitbox.RealBounds))
                 //   return;

               

                if(earliestCollision != null)
                {
                    //stop = true;
                    
                    //solve collision
                    SolveCollision(element, earliestCollision.Item2, earliestCollision.Item3, earliestCollision.Item1, earliestCollision.Item4);
                }


            });
        }



        private void UpdateApperanceInfo(ObjectsPooler objects)
        {
            objects.ActionForeachActive(element =>
            {
                ObjectApperanceInfo info = element.ObjectApperanceInfo;
                info.IsInCameraView = false;

                if (WorldManager.Camera.ViewBounds.Intersect(element.Hitbox.RealBounds))
                    info.IsInCameraView = true;

            });
        }

        private void ObjectsCollisionDetection(ObjectsPooler objects)
        {
            objects.ActionForeachTwoActives((element1, element2) => {

                if (!element1.ObjectApperanceInfo.IsObjectsCollisionOn || !element2.ObjectApperanceInfo.IsObjectsCollisionOn)
                    return;

                if (element1.Info.HasOneOfTags(element2.Rigidbody.IgnoreTags) || element2.Info.HasOneOfTags(element1.Rigidbody.IgnoreTags))
                    return;

                if (!element1.Hitbox.RealBounds.Intersect(element2.Hitbox.RealBounds))
                    return;

                GJK_2 gjk = new GJK_2();

                //         time  id1   id2
                List<Tuple<float, int, int>> collisionsToSolve = new List<Tuple<float, int, int>>();

                for (int id1 = 0; id1 < element1.Hitbox.RealPolygons.Count; id1++)
                {
                    Polygon polygon1 = element1.Hitbox.RealPolygons[id1];

                    for (int id2 = 0; id2 < element2.Hitbox.RealPolygons.Count; id2++)
                    {
                        Polygon polygon2 = element2.Hitbox.RealPolygons[id2];

                        //Check collision

                        gjk.CheckCollision(polygon1, polygon2);
                      
                        if (!gjk.IsCollision)
                            continue;
                        if (stop)
                            return;
                        //Debug.WriteLine("Start find");
                        //findt collision time
                        float collisionTime = FindCollisionTime(element1, element2, id1, id2);
                        //Debug.WriteLine(collisionTime / GameManager.Time.DeltaTime * 100);
                       
                        //Debug.WriteLine("End find");
                        //add to solve
                        collisionsToSolve.Add(new Tuple<float, int, int>(collisionTime, id1, id2));
                        
                    }
                }

                //no collsions, nothing to do
                if (collisionsToSolve.Count == 0)
                    return;

                //find the earliest collsion
                int closestId = 0;
                float closestVal = collisionsToSolve[0].Item1;

                for (int i = 1; i < collisionsToSolve.Count; i++)
                {
                    if(collisionsToSolve[i].Item1 < closestVal)
                    {
                        closestId = i;
                        closestVal = collisionsToSolve[i].Item1;
                    }
                }
                //stop = true;
                SolveCollison(element1, element2, collisionsToSolve[closestId].Item2, collisionsToSolve[closestId].Item3, collisionsToSolve[closestId].Item1);

            });

        }


        private void UpdateRigidBody(ObjectsPooler objects)
        {
            objects.ActionForeachActive(element => {

                if (!element.ObjectApperanceInfo.IsPhysicsUpdate || element.Rigidbody == null)
                    return;

                Rigidbody rigidbody = element.Rigidbody;

                float t = WorldManager.GameManager.Time.DeltaTime;
                float t2 = t * t;

                if (element.ObjectApperanceInfo.ApplyMotionResistance)
                    ApplyMotionResistance(element.Rigidbody);

                //Save prev params
                rigidbody.Params.SavePrevParams();

                ///Update linear
                //add velocity from impulses
                rigidbody.Params.Velocity += rigidbody.VelocityToAdd;
                
                
                //set acceleration from forces
                rigidbody.Params.Acceleration = rigidbody.AccelerationToAdd;
                //update position
                rigidbody.Transform.Position2 += rigidbody.PositionToAdd;
                rigidbody.Transform.Position2 += rigidbody.Params.Velocity * t + 0.5f * rigidbody.Params.Acceleration * t2;

                rigidbody.Params.Velocity += rigidbody.Params.Acceleration * t;

                ///UpdateAngular
                //add omega from impulses
                rigidbody.Params.Omega += rigidbody.OmegaToAdd;
                //set epsilon from forces
                rigidbody.Params.Epsilon = rigidbody.EpsilonToAdd;
                //update rotation
                rigidbody.Transform.Rotation += rigidbody.RotationToAdd;
                rigidbody.Transform.Rotation += rigidbody.Params.Omega * t + 0.5f * rigidbody.Params.Epsilon * t2;

                rigidbody.Params.Omega += rigidbody.Params.Epsilon * t;

                rigidbody.ClearToAdd();

            });

        }

        private void ApplyMotionResistance(Rigidbody rigidbody)
        {
            if (rigidbody.Params.Velocity != Vector2.Zero)
            {
                ///Linear
                float p = -WorldManager.GameManager.UserSettings.GeneralSettings.GlobalLinearResistanceRatio * rigidbody.Params.LinearResistance;

                //if (none)
                //   p = -0.9f;

                Vector2 R = p * rigidbody.Params.Velocity.LengthSquared() * Vector2.Normalize(rigidbody.Params.Velocity);

                rigidbody.AddForce(new Force(Vector2.Zero, R, Force.ForceType.Continous));

                ///LINEAR HORIZONTAL & VERTICAL
                ///

                Vector2 vRot1 = Tools.Rotate(rigidbody.Params.Velocity, -rigidbody.Transform.Rotation);
                vRot1.Normalize();

                Vector2 vVeritical = Tools.Mul(new Vector2(0, vRot1.Y), rigidbody.Transform.RotationMatrix);
                Vector2 vHorizontal = Tools.Mul(new Vector2(vRot1.X, 0), rigidbody.Transform.RotationMatrix);

                Vector2 R2 = -WorldManager.GameManager.UserSettings.GeneralSettings.GlobalLinearResistanceRatio * (rigidbody.Params.LinearVerticalResistance * vVeritical + rigidbody.Params.LinearHorizontalResistance * vHorizontal);

                R2 *= rigidbody.Params.Velocity.LengthSquared();

                //horizontal force
                rigidbody.AddForce(new Force(Vector2.Zero, R2, Force.ForceType.Continous));
            }

            if (rigidbody.Params.Omega != 0)
            {

                float p = WorldManager.GameManager.UserSettings.GeneralSettings.GlobalAngularResistanceRatio * rigidbody.Params.AngularResistance;

                if (rigidbody.Params.Omega < 0)
                    p *= -1;

                float val = p * rigidbody.Params.Omega * rigidbody.Params.Omega;

                rigidbody.AddForce(new Force(new Vector2(-30, 0), new Vector2(0, 1) * val, Force.ForceType.Continous));

            }
        }

        private Vector2 GetPositionDiffFromPast(PhysicalObject physicalObject, float timeShift)
        {
            return -(physicalObject.Rigidbody.Params.Velocity * timeShift - 0.5f * physicalObject.Rigidbody.Params.Acceleration * timeShift * timeShift);
        }
        private float GetRotationDiffFromPast(PhysicalObject physicalObject, float timeShift)
        {
            return -(physicalObject.Rigidbody.Params.Omega * timeShift - 0.5f * physicalObject.Rigidbody.Params.Epsilon * timeShift * timeShift);
        }

        private Polygon GetPolygonFromPast(PhysicalObject physicalObject, Polygon basic, float timeShift)
        {
            //calculate transform from past
            Transform transform = new Transform()
            {
                Position2 = physicalObject.Transform.Position2 + GetPositionDiffFromPast(physicalObject, timeShift),
                Rotation = physicalObject.Transform.Rotation + GetRotationDiffFromPast(physicalObject, timeShift)
            };

            transform.Update();

            return new Polygon(basic, transform.Matrix);
        }
    }
}
