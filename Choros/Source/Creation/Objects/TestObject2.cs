using Choros.Source.Resource;
using Engine.EngineMath;
using Engine.Graphics.Light;
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
    class TestObject2 : PhysicalObject
    {
        ResourcePackManager ResourcePackManager { get; set; }

        public TestObject2(ResourcePackManager resourcePackManager, Transform transform)
        {
            ResourcePackManager = resourcePackManager;

            Transform = transform;
        }

        public override void Start()
        {
            Info = new ObjectInfo();

            

            ObjectApperanceInfo = new ObjectApperanceInfo();
            ObjectApperanceInfo.SetDefault();

            Hitbox = new Hitbox(this);

            Model = new Model2D();

            Rigidbody = new Rigidbody(this);

            SetModel();

            CalculateInertia();

            //Rigidbody.Params.Elasticity = 0.7f;

            Rigidbody.Params.LinearResistance = 0.15f;
            Rigidbody.Params.LinearHorizontalResistance = 0.0f;
            Rigidbody.Params.LinearVerticalResistance = 0.3f;
            Rigidbody.Params.AngularResistance = 0.5f;
        }

        public override void Update()
        {
           // Transform.Rotation += 0.3f * GameManager.Time.DeltaTime;

            WorldManager.LightManager.Add(new CircleLatern(Transform, 800, Color.Green, true, true));

        }


        private void SetModel()
        {
            CellModelComponent component = new CellModelComponent(ResourcePackManager.TexturesPack.Get("model3"), new Point(7, 10), new Vector2(5, 5), Vector2.Zero);

            component.Transform = Transform.Zero;
            component.DrawFrame = false;
            component.FrameColor = Color.Black;
            Model.AddModelComponent(component);

            Hitbox.Radius = 100;
            Rigidbody.Params.Mass = 2;


            MeshModelComponent obstacle = new MeshModelComponent(null, null);


            obstacle.Transform = new Transform(new Vector2(-12, -15));

            float size = 6;

            obstacle.AddShadowPolygon(new Polygon(
                new Vector2(-size, -size),
                new Vector2(size, -size),
                new Vector2(size, size),
                new Vector2(-size, size)
            ));

            Model.AddModelComponent(obstacle);

            MeshModelComponent obstacle2 = new MeshModelComponent(null, null);


            obstacle2.Transform = new Transform(new Vector2(-12, 15));


            obstacle2.AddShadowPolygon(new Polygon(
                new Vector2(-size, -size),
                new Vector2(size, -size),
                new Vector2(size, size),
                new Vector2(-size, size)
            ));

            Model.AddModelComponent(obstacle2);

            Hitbox.AddPolygon(new Polygon(new Vector3(5, 5, 0),
                new Vector2(-7.5f, -1.5f),
                new Vector2(-2.5f, -10.5f),
                new Vector2(1.5f, -10.5f),
                new Vector2(4.5f, -9.5f),
                new Vector2(4.5f, 9.5f),
                new Vector2(1.5f, 10.5f),
                new Vector2(-2.5f, 10.5f),
                new Vector2(-7.5f, 1.5f)
            ));

            Hitbox.AddPolygon(new Polygon(new Vector3(5, 5, 0),
                new Vector2(4.5f, -2.5f),
                new Vector2(6.5f, 0),
                new Vector2(4.5f, 2.5f)
             
            ));

            Hitbox.Radius = 13 * 5;
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

        }
    }
}
