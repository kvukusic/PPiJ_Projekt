#region

using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Model
{
    public class HistoryItem
    {
        /// <summary>
        /// The ID of this history item.
        /// </summary>
        public int ID { get; set; }

		/// <summary>
		/// Collection of all checkpoint on the route.
		/// </summary>
		public GeoCoordinateCollection CurrentWayPoints;

		/// <summary>
		/// Date and time of session start.
		/// </summary>
		public DateTime StartTime;

		/// <summary>
		/// Date and time of session end.
		/// </summary>
		public DateTime EndTime;

		/// <summary>
		/// Length of route in meters.
		/// </summary>
		public double RouteLength;

    }
}