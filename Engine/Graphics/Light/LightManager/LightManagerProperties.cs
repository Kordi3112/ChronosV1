using Engine.Control;
using Engine.Core;
using Engine.EngineMath;
using Engine.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics.Light
{
    //PROPERTIES
    public partial class LightManager
    {

        WorldManager WorldManager { get; set; }
        VideoManager VideoManager => WorldManager.GameManager.VideoManager;
        UserSettings UserSettings => WorldManager.GameManager.UserSettings;
        Camera Camera => WorldManager.Camera;
        ///SETTINGS
        ///
        public bool IsLightingActive { get; set; }
        ///
        List<Latern> ShadowLaterns { get; set; }
        List<Latern> CasualLaterns { get; set; }

        public int ShadowLaternsCount => ShadowLaterns.Count;
        public int CasualLaternsCount => CasualLaterns.Count;
        ///
        RenderTarget2D LightMapTexture { get; set; }
        List<RenderTarget2D> ShadowsMapTextures { get; set; }
        List<RenderTarget2D> LaternMapTextures { get; set; }
        Point SceneSize { get; set; }
        List<Polygon> Polygons { get; set; }

        ///
        float DefaultCircleLaternRadius { get; set; }
        Texture2D DefaultCircleLaternTexture { get; set; }
        public List<RectangleF> LightBounds { get; private set; }

        ///
        internal List<List<RayLine>> _rays;
        Queue<Latern> laternsQueue;
        bool directAddingLight;

        ///

        float _raysAngleOffset = 0.000005f;
        float _raysAngleSin = 0;
        float _raysAngleCos = 0;
    }
}
