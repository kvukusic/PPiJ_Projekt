#region

using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;

#endregion

namespace Hoover.Model
{
    public class HistoryItem
    {
        /// <summary>
        /// The ID of this history item.
        /// </summary>
        public long ID { get; set; }

		/// <summary>
		/// Collections of all checkpoints on route.
		/// </summary>
		public List<GeoCoordinate> Checkpoints;

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

		/// <summary>
		/// Average speed on route.
		/// </summary>
		public double AverageSpeed;

    }
}