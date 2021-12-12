using Choros.Source.Resource;
using Engine.EngineMath;
using Engine.Graphics.Light;
using Engine.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.Creation
{
    class Bullet01 : PhysicalObject
    {
        float LifeTime = 0;

        ResourcePackManager ResourcePackManager { get; set; }

        float _speed;
        float _rotation;

        Vector2 _initPos;
        public Bullet01(ResourcePackManager resourcePackManager, float rotation, float speed, Vector2 initPos)
        {
            ResourcePackManager = resourcePackManager;
            _speed = speed;
            _rotation = rotation;
            _initPos = initPos;
        }

        public override void Start()
        {
            Info = new ObjectInfo();
            Info.SetTags("Bullet");

            Transform = new Transform(_initPos, _rotation);

            ObjectApperanceInfo = new ObjectApperanceInfo();
            ObjectApperanceInfo.SetDefault();

            ObjectApperanceInfo.IsObjectsCollisionOn = true;
            ObjectApperanceInfo.IsLandCollisionOn = true;
            Hitbox = new Hitbox(this);
            Hitbox.Radius = 8;

            Model = new Model2D();

            Rigidbody = new Rigidbody(this);

            Rigidbody.IgnoreTags.Add("Player");
            Rigidbody.IgnoreTags.Add("Bullet");

            SetModel();
            Random random = new Random((int)(GameManager.Time.TotalTime * 1000000));
            Transform.Rotation = 2 * 3.14f * (float)random.NextDouble();


            Rigidbody.AddForce(new Force(Vector2.Zero, 3000 * new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation)), Force.ForceType.Impulse));
        }



        public override void Update()
        {
            LifeTime += GameManager.Time.DeltaTime;


            WorldManager.LightManager.Add(new CircleLatern(Transform, 10, Color.White, false, true));

            if (LifeTime > 5)
                WorldManager.ObjectsPooler.AddToRemoveList(this);
        }

        private void SetModel()
        {
            CellModelComponent component = new CellModelComponent(ResourcePackManager.TexturesPack.Get("bullet1"), new Point(5, 2), new Vector2(2, 1), Vector2.Zero);
            component.Transform = Transform.Zero;
            component.DrawFrame = false;
            component.FrameColor = Color.Black;
            Model.AddModelComponent(component);

            Hitbox.AddPolygon(new Polygon(

                new Vector2(-2, -1) * 2,
                new Vector2(0, 2) * 2,
                new Vector2(2, -1) * 2


            ));


            Rigidbody.Params.Mass = 10f;
            CalculateInertia();
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


            float inertia = Rigidbody.Params.Mass * Tools.CalculateGeometricInertia(vertices.ToArray(), 200);


            Rigidbody.Params.Inertia = inertia;

        }
    }
}
