using Choros.Source.Resource;
using Engine.EngineMath;
using Engine.Graphics.Light;
using Engine.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Creation
{
   

    class TestObject1 : PhysicalObject
    {
        MovementController MovementController { get; set; }
        ResourcePackManager ResourcePackManager { get; set; }

        Vector2 laternPos = new Vector2(20, 0);

        float cooldownCount = 0;

        float currentEngineRot = 0;

        public TestObject1(ResourcePackManager resourcePackManager)
        {
            ResourcePackManager = resourcePackManager;
        }

        public override void Start()
        {
            Info = new ObjectInfo();
            Info.SetTags("Player");

            Transform = new Transform(new Vector2(0, -300), (float)Math.PI / 2);

            ObjectApperanceInfo = new ObjectApperanceInfo();
            ObjectApperanceInfo.SetDefault();
            ObjectApperanceInfo.IsObjectsCollisionOn = true;
            Hitbox = new Hitbox(this);
          
            Model = new Model2D();

            Rigidbody = new Rigidbody(this);
            //Rigidbody.Params.Inertia = 1000;
            Rigidbody.Params.Mass = 10;
            //SetHitbox();
            SetModel();

            CalculateInertia();

            MovementController = new MovementController(Rigidbody);

            MovementController.Add(new Engine(new Vector2(-30, -30)));
            MovementController.Add(new Engine(new Vector2(-30, 30))); 
            MovementController.Add(new Engine(new Vector2(30, -30)));
            MovementController.Add(new Engine(new Vector2(30, 30)));   
        }

        private void CalculateInertia()
        {
            List<Vector2> vertices = new List<Vector2>();

            foreach (var polygon in Hitbox.Polygons)
            {
                foreach (var vertex in polygon.Vertices)
                {
                    if (!vertices.Contains(vertex))
                        vertices.Add(vertex);
                }
            }

            Debug.WriteLine(vertices.Count);

            float inertia = Rigidbody.Params.Mass * Tools.CalculateGeometricInertia(vertices.ToArray(), 200);

            Debug.WriteLine(inertia);

            Rigidbody.Params.Inertia = inertia;
            Rigidbody.Params.LinearResistance = 0.5f;
            Rigidbody.Params.LinearHorizontalResistance = 0.0f;
            Rigidbody.Params.LinearVerticalResistance = 0.8f;
            Rigidbody.Params.AngularResistance = 1;
        }

        private void SetModel()
        {
            SetObjectFromXML(ResourcePackManager.ObjectsXMLPack.Get("object1"), ResourcePackManager.ModelComponentsPack);


            MeshModelComponent latern = new MeshModelComponent(null, null);

            latern.Transform = new Transform(laternPos);

            float size = 15;

            latern.AddShadowPolygon(new Polygon(
                new Vector2(-size, 0),
                new Vector2(0.5f * size, 0.5f * size),
                new Vector2(-0.8f * size, 0),
                new Vector2(0.5f * size, -0.5f * size)
            ));

            Model.AddModelComponent(latern);


            //add engines


            CellModelComponent engine1 = new CellModelComponent(ResourcePackManager.TexturesPack.Get("engine1"), new Point(5, 5), new Vector2(2, 2), Vector2.Zero)
            {
                Transform = new Transform(new Vector2(-30, -30)),
                Tag = "engine1"
            };
            CellModelComponent engine2 = new CellModelComponent(ResourcePackManager.TexturesPack.Get("engine1"), new Point(5, 5), new Vector2(2, 2), Vector2.Zero)
            {
                Transform = new Transform(new Vector2(-30, 30)),
                Tag = "engine2"
            };
            CellModelComponent engine3 = new CellModelComponent(ResourcePackManager.TexturesPack.Get("engine1"), new Point(5, 5), new Vector2(2, 2), Vector2.Zero)
            {
                Transform = new Transform(new Vector2(30, -30)),
                Tag = "engine3"
            };
            CellModelComponent engine4 = new CellModelComponent(ResourcePackManager.TexturesPack.Get("engine1"), new Point(5, 5), new Vector2(2, 2), Vector2.Zero)
            {
                Transform = new Transform(new Vector2(30, 30)),
                Tag = "engine4"
            };

            CellModelComponent engine0 = new CellModelComponent(ResourcePackManager.TexturesPack.Get("engine1"), new Point(4, 4), new Vector2(2, 2), Vector2.Zero)
            {
                Transform = new Transform(new Vector2(-30, 0)),
                Tag = "engine0"
            };

            Model.AddModelComponent(engine0);
           // Model.AddModelComponent(engine2);
           // Model.AddModelComponent(engine3);
           // Model.AddModelComponent(engine4);
        }


        public override void Update()
        {
           // Transform.Z += 1;        
            MovementUpdate();

            Vector2 offset = new Vector2((float)Math.Sin(GameManager.Time.TotalTime) * 0.5f, (float)Math.Cos(GameManager.Time.TotalTime) * 0.25f);
          //  offset = Vector2.Zero;2

            Vector2 realLaternPos = Tools.Mul(laternPos + new Vector2(-1,0) + offset, Transform.Matrix);

            float range = GameManager.Input.Keys.IsKeyDown(Keys.LeftControl) ? 1200 : 600;

            WorldManager.LightManager.Add(new CircleLatern(new Transform(realLaternPos), range, Color.IndianRed, true, true));

            Vector2 additionalLatern1Pos = new Vector2(0, -20);
            Vector2 additionalLatern2Pos = new Vector2(0, 20);

            additionalLatern1Pos = Tools.Mul(additionalLatern1Pos, Transform.Matrix);
            additionalLatern2Pos = Tools.Mul(additionalLatern2Pos, Transform.Matrix);

            WorldManager.LightManager.Add(new CircleLatern(new Transform(additionalLatern1Pos), 150, Color.White, false, true));
            WorldManager.LightManager.Add(new CircleLatern(new Transform(additionalLatern2Pos), 150, Color.White, false, true));

            if(GameManager.Input.Keys.IsKeyDown(Keys.H))
            {
                ModelComponent cellModelComponent = Model.GetModelComponent("laserDown");
                cellModelComponent.Transform.Rotation += 0.8f * GameManager.Time.DeltaTime;
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.G))
            {
                ModelComponent cellModelComponent = Model.GetModelComponent("laserDown");
                cellModelComponent.Transform.Rotation -= 0.8f * GameManager.Time.DeltaTime;
            }

            cooldownCount += GameManager.Time.DeltaTime;

            if (GameManager.Input.Keys.IsKeyDown(Keys.RightShift))
            {
                if(cooldownCount > 0.06f)
                {
                    cooldownCount = 0;
                    WorldManager.ObjectsPooler.AddToList(new Bullet01(ResourcePackManager, Transform.Rotation, 20, Transform.Position2));
                }

               
            }
            if(GameManager.Input.Keys.IsKeyDown(Keys.Enter))
                Rigidbody.AddForce(new Force(new Vector2(0, 10), new Vector2(0, 1000), Force.ForceType.Continous));

            if (GameManager.Input.Keys.IsKeyDown(Keys.Back))
                Rigidbody.AddForce(new Force(new Vector2(10, 0), new Vector2(1000, 0), Force.ForceType.Continous));

            WorldManager.Camera.Transform.Position2 = Transform.Position2;
        }
        private void MovementUpdate2()
        {
            Vector2 engine1Pos = new Vector2(-30, -30);
            Vector2 engine2Pos = new Vector2(-30, 30);

            Model.GetModelComponent("laserUp").Transform.Position2 = engine1Pos;
            Model.GetModelComponent("laserDown").Transform.Position2 = engine2Pos;
            //CorrectEnginePos
            engine1Pos = Tools.Mul(engine1Pos, Transform.RotationMatrix);
            engine2Pos = Tools.Mul(engine2Pos, Transform.RotationMatrix);

            float rotSpeed = 1;
            if (GameManager.Input.Keys.IsKeyDown(Keys.U)) //Rot Left
            {
                Rigidbody.RotationToAdd += -rotSpeed * GameManager.Time.DeltaTime;
            }
            if (GameManager.Input.Keys.IsKeyDown(Keys.O)) //Rot Right
            {
                Rigidbody.RotationToAdd += rotSpeed * GameManager.Time.DeltaTime;
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.I)) //Up
            {
                Rigidbody.AddForce(new Force(engine1Pos, 500  * Tools.AngleToNormal(Transform.Rotation), Force.ForceType.Continous));
                Rigidbody.AddForce(new Force(engine2Pos, 500  * Tools.AngleToNormal(Transform.Rotation), Force.ForceType.Continous));
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.K)) //Down
            {
                Rigidbody.AddForce(new Force(engine1Pos, -500 * Tools.AngleToNormal(Transform.Rotation), Force.ForceType.Continous));
                Rigidbody.AddForce(new Force(engine2Pos, -500 * Tools.AngleToNormal(Transform.Rotation), Force.ForceType.Continous));
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.J)) //Left
            {
                float alphaOffstet = MathHelper.Pi / 2;
                Rigidbody.AddForce(new Force(engine1Pos, 200 * Tools.AngleToNormal(Transform.Rotation + alphaOffstet), Force.ForceType.Continous));
                Rigidbody.AddForce(new Force(engine2Pos, 200 * Tools.AngleToNormal(Transform.Rotation + alphaOffstet), Force.ForceType.Continous));
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.L)) //Right
            {
                float alphaOffstet = MathHelper.Pi / 2;
                Rigidbody.AddForce(new Force(engine1Pos, 200 * Tools.AngleToNormal(Transform.Rotation - alphaOffstet), Force.ForceType.Continous));
                Rigidbody.AddForce(new Force(engine2Pos, 200 * Tools.AngleToNormal(Transform.Rotation - alphaOffstet), Force.ForceType.Continous));
            }


            if (GameManager.Input.Keys.IsKeyDown(Keys.Space))
            {
                Rigidbody.OmegaToAdd += -1f * Rigidbody.Params.Omega * GameManager.Time.DeltaTime;
                Rigidbody.VelocityToAdd += -1f * Rigidbody.Params.Velocity * GameManager.Time.DeltaTime;
            }
        }
        private void MovementUpdate()
        {
            bool none = false;
            float value = 3000;
            float targetRotation = 0;

            if (GameManager.Input.Keys.IsKeyDown(Keys.I) || GameManager.Input.Keys.IsKeyDown(Keys.K))
            {
                if (GameManager.Input.Keys.IsKeyDown(Keys.J))
                {
                    targetRotation = MathHelper.Pi / 6;
                    value *= 0.9f;
                }
                    
                else if (GameManager.Input.Keys.IsKeyDown(Keys.L))
                {
                    targetRotation = -MathHelper.Pi / 6;
                    value *= 0.9f;
                }
                    
                else targetRotation = 0;

                if (GameManager.Input.Keys.IsKeyDown(Keys.K))
                {
                    targetRotation *= -1;
                    targetRotation += MathHelper.Pi;
                }
                    
            }
            else if (GameManager.Input.Keys.IsKeyDown(Keys.J))
            {
                targetRotation = MathHelper.PiOver2;
                value *= 0.5f;
            }
                
            else if (GameManager.Input.Keys.IsKeyDown(Keys.L))
            {
                targetRotation = -MathHelper.PiOver2;
                value *= 0.5f;
            }
                
            else
            {
                none = true;
                value = 0;
            }


            //Update Rotation
            float omegaSpeed = 40;

            float omegaDiff = omegaSpeed * GameManager.Time.DeltaTime;

            if(omegaDiff >= Math.Abs(currentEngineRot - targetRotation))
            {
                currentEngineRot = targetRotation;
            }
            else
            {
                if(currentEngineRot - targetRotation < 0)
                {
                    currentEngineRot += omegaDiff;
                }
                else currentEngineRot -= omegaDiff;
            }

            if (Math.Abs(currentEngineRot - targetRotation) > 0.5f)
                value = 0;

            Model.GetModelComponent("engine0").Transform.Rotation = currentEngineRot;


            Vector2 enginePos = new Vector2(-30, 0);

            Vector2 realPos = Tools.Mul(enginePos, Transform.RotationMatrix);



            //resistance
            //value *= 1 - Rigidbody.Params.Velocity.LengthSquared() / 25000;

            Vector2 F = Tools.Rotate(-Vector2.Normalize(realPos), currentEngineRot) * value;

            

            Rigidbody.AddForce(new Force(realPos, F, Force.ForceType.Continous));

            



        }

        private void MovementUpdate3()
        {

            bool up = false;
            bool left = false;
            bool right = false;

            if (GameManager.Input.Keys.IsKeyDown(Keys.J)) //Rot Left
            {
                left = true;
            }
            if (GameManager.Input.Keys.IsKeyDown(Keys.L)) //Rot Right
            {
                right = true;
            }

            if (GameManager.Input.Keys.IsKeyDown(Keys.I)) //Up
            {
                up = true;
            }

            MovementController.MovementType type = MovementController.MovementType.None;

            if(up)
            {
                if (!left && !right)
                    type = MovementController.MovementType.FullForward;
                if (left)
                    type = MovementController.MovementType.ForwardRotateLeft;
                if (right)
                    type = MovementController.MovementType.ForwardRotateRight;
            }
            else
            {
                if (left)
                    type = MovementController.MovementType.FullRotateLeft;
                if (right)
                    type = MovementController.MovementType.FullRotateRight;
            }

            MovementController.Update(type);

            //Set Models
            Model.GetModelComponent("engine1").Transform.Rotation = MovementController.GetEngine(0).Rotation;
            Model.GetModelComponent("engine2").Transform.Rotation = MovementController.GetEngine(1).Rotation;
            Model.GetModelComponent("engine3").Transform.Rotation = MovementController.GetEngine(2).Rotation;
            Model.GetModelComponent("engine4").Transform.Rotation = MovementController.GetEngine(3).Rotation;
        }
    }
}
