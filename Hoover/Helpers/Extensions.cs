using Hoover.Settings;
using Microsoft.Phone.Maps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoover.Helpers
{
	public static class Extensions
	{
		/// <summary>
		/// Check what unit system is used and returns a route length in selected unit system.
		/// </summary>
		/// <param name="route"></param>
		/// <returns></returns>
		public static string Length(this Route route) {
			if(ApplicationSettings.Instance.UseMetricSystem) 
			{
				return route.LengthInMeters + " m";
			}
			else
			{
				return Convert.ToInt32(route.LengthInMeters * 0.9144) + " yd";
			}
		}

		/// <summary>
		/// Converts walking duration time of route to runing duration.
		/// Factor for time scaling is 0.4.
		/// </summary>
		/// <param name="route"></param>
		/// <returns>Time converted to string in format HH:mm:ss</returns>
		public static string RuningTime(this Route route)
		{
			return TimeSpan.FromSeconds(Convert.ToInt32(route.EstimatedDuration.TotalSeconds * 0.4)).ToString();
		}
	}
}
