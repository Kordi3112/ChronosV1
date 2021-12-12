using Choros.Source.App.Scenes;
using Choros.Source.Resource;
using Engine.Core;
using Engine.Resource;
using EngineXML;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choros.Source.App
{
    public class GameManager1 : GameManager
    {
        public ResourcePackManager ResourcePackManager { get; set; }
        protected override void LoadContent(ContentManager content)
        {
            EffectsPack.ColorEffect = content.Load<Effect>("Effects/Basic/ColorEffect");
            EffectsPack.TextureEffect = content.Load<Effect>("Effects/Basic/TextureEffect");
            EffectsPack.Combine1 = content.Load<Effect>("Effects/Basic/Combine1");
            EffectsPack.Combine2 = content.Load<Effect>("Effects/Basic/Combine2");

            EffectsPack.FogWave = content.Load<Effect>("Effects/FogWave");

            UserSettings.SetToDefault();

            XMLData1 xMLData1 = content.Load<XMLData1>("XML/xml1");
            Debug.WriteLine(xMLData1.A);
            Debug.WriteLine(xMLData1.B);
            Debug.WriteLine(xMLData1.C);

            float[] tab = xMLData1.D;

            foreach (float val in tab)
                Debug.WriteLine(val);

            Debug.WriteLine("------------");

            string[] str = xMLData1.Text;

            foreach (string val in str)
                Debug.WriteLine(val);

            ResourcePackManager = new ResourcePackManager(content);


            ResourcePackManager.Load(content);

            CommandPanel.SpriteFont = ResourcePackManager.FontsPack.Get("Default");

        }

        protected override void LoadScenes()
        {
            VideoManager.IsFullscreen = true;
            VideoManager.ApplyChanges();



            SceneManager.AddScene(new Scene1(ResourcePackManager)); //1
            SceneManager.AddScene(new Scene2(ResourcePackManager)); //2
            SceneManager.SetHeadScene(new HeadScene1(ResourcePackManager));

            SceneManager.SetCurrentSceneById(1);

        }
    }
}
