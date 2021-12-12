using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control
{
    public partial class UserSettings
    {
        public General GeneralSettings { get; set; }
        public Video VideoSettings { get; set; }
        public Controls ControlsSettings { get; set; }

        public Debug DebugSettings { get; set; }

        public UserSettings()
        {
            GeneralSettings = new General();
            VideoSettings = new Video();
            ControlsSettings = new Controls();
            DebugSettings = new Debug();
        }

        public void SetToDefault()
        {
            GeneralSettings.SetToDefault();
            VideoSettings.SetToDefault();
            ControlsSettings.SetToDefault();
            DebugSettings.SetToDefault();
        }
    }
}
