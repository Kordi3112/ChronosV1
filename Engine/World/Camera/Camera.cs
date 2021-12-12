using Engine.Control.Input;
using Engine.Core;
using Engine.EngineMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    public class Camera
    {
        public Transform Transform { get; set; }

        public float Zoom { get; set; }

        public Vector2 DefaultViewSize { get; set; }

        public Vector2 ViewSize => DefaultViewSize / Zoom;

        public Matrix CameraMatrix { get; private set; }

        public Matrix CameraInvMatrix { get; private set; }

        /// <summary>
        /// View bounds in map coordinates
        /// </summary>
        public RectangleF ViewBounds { get; private set; }

        public Camera()
        {
            Transform = new Transform();
            Zoom = 1;
        }

        public Camera(float zoom)
        {
            Transform = new Transform();

            Zoom = zoom;
        }

        /// <summary>
        /// recalculate camera matrix
        /// </summary>
        public void Update()
        {
            Matrix translation = Matrix.CreateTranslation(-Transform.Position);

            Matrix rotation = Matrix.CreateRotationZ(-Transform.Rotation);

            Matrix scale = Matrix.CreateScale(1 / ViewSize.X * 2.0f, -1 / ViewSize.Y * 2.0f, 1);

            CameraMatrix = translation * rotation * scale;
            //CameraMatrix = scale * rotation * translation;

            CameraInvMatrix = Matrix.Invert(CameraMatrix);

            ViewBounds = RectangleF.CreateFromCenter(Transform.Position2, ViewSize);

            Transform.Update();

        }

        public Vector3 ScenePositionToWorld(Vector2 position, Vector2 sceneSize)
        {

            //                (position - halfSceneSize) / halfSceneSize;
            Vector2 partPos = position / sceneSize * 2.0f - Vector2.One;
            //correct y 
            partPos.Y *= -1;



            return Tools.Mul(new Vector3(partPos, 0), CameraInvMatrix);
        }

        public Vector2 MousePositionToWorld(Vector2 sceneSize, InputState inputState)
        {
            return Tools.ToVector2(ScenePositionToWorld(new Vector2(inputState.Mouse.Position.X, inputState.Mouse.Position.Y), sceneSize));

        }
        /// <summary> Takes screen pos in pixels </summary>
        public Matrix CreateDefaultViewMatrix()
        {
            return Matrix.CreateScale(1 / ViewSize.X * 2, -1 / ViewSize.Y * 2, 1) * Matrix.CreateTranslation(-1, 1, 0);
        }

        /// <summary> Takes screen pos in pixels </summary>
        public static Matrix CreateDefaultViewMatrix(Vector2 viewSize)
        {
            return Matrix.CreateScale(1 / viewSize.X * 2, -1 / viewSize.Y * 2, 1) * Matrix.CreateTranslation(-1, 1, 0);
        }

        /// <summary> Takes screen pos in pixels </summary>
        public static Matrix CreateDefaultViewMatrix(VideoManager videoManager)
        {
            return Matrix.CreateScale(1 / videoManager.ViewSize.X * 2, -1 / videoManager.ViewSize.Y * 2, 1) * Matrix.CreateTranslation(-1 + videoManager.ViewShift.X, 1 + videoManager.ViewShift.Y, 0);
        }

        /// <summary>
        /// effect.Parameters["WorldViewProjection"].SetValue(CameraMatrix);
        /// </summary>
        public void SetEffectMatrix(Effect effect)
        {
            effect.Parameters["WorldViewProjection"].SetValue(CameraMatrix);
        }
    }
}
