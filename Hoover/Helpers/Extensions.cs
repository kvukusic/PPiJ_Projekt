using Hoover.Settings;
using Microsoft.Phone.Maps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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

		public static string Length(this double lengthInMeters)
		{
			if (ApplicationSettings.Instance.UseMetricSystem)
			{
				return Math.Round(lengthInMeters).ToString() + " m";
			}
			else
			{
				return Convert.ToInt32(lengthInMeters * 0.9144) + " yd";
			}
		}

		/// <summary>
		/// Check what unit system is used and returns a route average speed in selected unit system.
		/// </summary>
		/// <param name="speedInMetersPerSecond"></param>
		/// <returns></returns>
		public static string Speed(this double speedInMetersPerSecond)
		{
			if (ApplicationSettings.Instance.UseMetricSystem)
			{
				return Math.Round(speedInMetersPerSecond * 3.6, 1) + " km/h";
			}
			else
			{
				return Math.Round(speedInMetersPerSecond * 2.237136, 1) + " mph";
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

        /// <summary>
        /// Extension method for <see cref="JObject"/> when getting the value. Handles the situations when key is not found which returns the default value of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(this JObject jObject, string key, T defaultValue = default(T))
        {
            T retval = defaultValue;

            if (jObject != null)
            {
                var val = jObject[key];
                if (val != null) return val.Value<T>();
            }

            return retval;
        }

        /// <summary>
        /// Returns a timespan formatted string.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
	    public static string TimeSpanFormatString(this TimeSpan ts)
        {
            return Convert.ToInt32(ts.TotalHours) + "h " + ts.Minutes + "m " + ts.Seconds + "s";
        }
	}
}
