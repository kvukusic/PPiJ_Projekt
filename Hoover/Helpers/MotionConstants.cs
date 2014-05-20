#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Devices.Sensors;

#endregion

namespace Hoover.Helpers
{
    public class MotionConstants
    {
        /// <summary>
        /// Will be true if Motion API is enabled on this device.
        /// </summary>
        public bool IsMotionEnabled
        {
            get
            {
                return Motion.IsSupported;
            }
        }
    }
}