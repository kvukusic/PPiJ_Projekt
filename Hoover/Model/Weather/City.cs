#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Model.Weather
{
    public class City
    {
        /// <summary>
        /// City name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City country ba Two-Letter code.
        /// </summary>
        public string Country { get; set; }
    }
}