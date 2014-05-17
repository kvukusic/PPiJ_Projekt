#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Model.Weather
{
    public class Coordinate
    {
        /// <summary>
        /// The longitude component.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// The latitude component.
        /// </summary>
        public double Latitude { get; set; }
    }
}