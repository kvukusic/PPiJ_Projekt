#region

using System;


#endregion

namespace Hoover.Helpers
{
	/// <summary>
	/// This class contains methods to manipulate with DateTime, and converting dates and time to strings.
	/// </summary>
	public static class CalendarHelper
	{

		/// <summary>
		/// This method converts the given <see cref="DateTime"/> to a string representation of the time.
		/// </summary>
		/// <param name="dateTime">The <see cref="DateTime"/> which will be converted to a time string.</param>
		/// <returns>
		/// The string of the given <see cref="DateTime"/> in culture format.
		/// <para></para>
		/// Example:
		/// <para></para>
		/// <c>19:27:58</c>
		/// <para></para>
		/// <c>06:35:21 PM</c>
		/// </returns>
		public static string FromDateTimeToTimeString(DateTime dateTime)
		{
			return dateTime.ToString("t");
		}

		/// <summary>
		/// This method converts the given <see cref="DateTime"/> to a string representation of the date.
		/// </summary>
		/// <param name="dateTime">The <see cref="DateTime"/> which will be converted to a date string</param>
		/// <returns>
		/// The string of the given <see cref="DateTime"/> in culture format.<para></para>
		/// Example: 
		/// <para></para>
		/// <c>20.10.2013</c>
		/// <para></para>
		/// <c>2013/10/20</c>
		/// </returns>
		public static string FromDateTimeToDateString(DateTime dateTime)
		{
			return dateTime.ToString("d");
		}

		/// <summary>
		/// This method returns a DateTime for the given timestamp in the local timezone.
		/// </summary>
		/// <param name="unixTime">The timestamp which will be converted to a <see cref="DateTime"/>.</param>
		/// <returns>A <see cref="DateTime"/> object which represents the date of the given timestamp.</returns>
		public static DateTime FromUnixTimeToDateTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime).ToLocalTime();
		}

		/// <summary>
		/// This method returns a DateTime for the given timestamp in the utc timezone.
		/// </summary>
		/// <param name="unixTime">The timestamp which will be converted to a <see cref="DateTime"/>.</param>
		/// <returns>A <see cref="DateTime"/> object which represents the date of the given timestamp.</returns>
		public static DateTime FromUnixTimeToUtcDateTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime);
		}

		/// <summary>
		/// This method converts the given DateTime to timestamp.
		/// </summary>
		/// <param name="date">A <see cref="DateTime"/> which will be converted to its timestamp (unix format).</param>
		/// <returns>The timestamp (unix format) of the given <see cref="DateTime"/></returns>
		public static long FromDateTimeToUnixTime(DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan diff = date.ToUniversalTime() - origin;
			return (long)diff.TotalSeconds;
		}

		/// <summary>
		/// This method convert DateTime to a “Time Ago” string
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string TimeAgo(DateTime dt)
		{
			TimeSpan span = DateTime.Now - dt;
			if (span.Days > 365)
			{
				int years = (span.Days / 365);
				if (span.Days % 365 != 0)
					years += 1;
				return String.Format("about {0} {1} ago",
				years, years == 1 ? "year" : "years");
			}
			if (span.Days > 30)
			{
				int months = (span.Days / 30);
				if (span.Days % 31 != 0)
					months += 1;
				return String.Format("about {0} {1} ago",
				months, months == 1 ? "month" : "months");
			}
			if (span.Days > 0)
				return String.Format("about {0} {1} ago",
				span.Days, span.Days == 1 ? "day" : "days");
			if (span.Hours > 0)
				return String.Format("about {0} {1} ago",
				span.Hours, span.Hours == 1 ? "hour" : "hours");
			if (span.Minutes > 0)
				return String.Format("about {0} {1} ago",
				span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
			if (span.Seconds > 5)
				return String.Format("about {0} seconds ago", span.Seconds);
			if (span.Seconds <= 5)
				return "just now";
			return string.Empty;
		}

	}

}