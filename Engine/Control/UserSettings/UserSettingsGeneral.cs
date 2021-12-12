using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Control
{
    public partial class UserSettings
    {
        public class General
        {
            public float GlobalLinearResistanceRatio { get; set; }
            public float GlobalAngularResistanceRatio { get; set; }

            public void SetToDefault()
            {
                GlobalLinearResistanceRatio = 0.05f;
                GlobalAngularResistanceRatio = 100f;
            }
        }
    }
}
