#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Model.Weather
{
    public class ForecastItem
    {
        /// <summary>
        /// The timestamp of this forecast item.
        /// </summary>
        public long Dt { get; set; }

        /// <summary>
        /// The temperature in metric system.
        /// </summary>
        public double Temp { get; set; }

        /// <summary>
        /// The weather message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The icon id.
        /// </summary>
        public string Icon { get; set; }

    }
}