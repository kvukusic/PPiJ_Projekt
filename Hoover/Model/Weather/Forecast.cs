#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Model.Weather
{
    public class Forecast
    {
        /// <summary>
        /// The city for which the forecast is requested.
        /// </summary>
        public City City { get; set; }

        /// <summary>
        /// The forecast items.
        /// </summary>
        public List<ForecastItem> ForecastItems { get; set; }
    }
}