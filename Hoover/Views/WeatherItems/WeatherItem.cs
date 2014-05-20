#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Views.WeatherItems
{
    public class WeatherItem
    {
        public string TemperatureString { get; set; }
        public string TimeString { get; set; }
        public string IconUri { get; set; }
        public string Message { get; set; }

        public bool IsHeader { get; set; }
        public string Title { get; set; }
    }
}