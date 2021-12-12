using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EngineMath
{
    class RaysAngleComparer : IComparer<RayLine>
    {
        public int Compare(RayLine x, RayLine y)
        {
            if (x.Alpha < y.Alpha)
                return -1;
            if (x.Alpha > y.Alpha)
                return 1;          
            return 0;             
        }
    }
}
