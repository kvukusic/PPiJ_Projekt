#region

using System.Threading.Tasks;
using GoogleAnalytics;
using GoogleAnalytics.Core;

#endregion

namespace Hoover.Services
{
    /// <summary>
    /// Sends data to the platform specific analytics account.
    /// </summary>
    public sealed class AnalyticsService
    {

        #region Singleton

        /// <summary>
        /// Private constructor.
        /// </summary>
        private AnalyticsService()
        {
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AnalyticsService()
        {
        }

        private static readonly AnalyticsService _instance = new AnalyticsService();
        public static AnalyticsService Instance { get { return _instance; } }

        #endregion

        /// <summary>
        /// Tracks a screen view for the given screen name.
        /// </summary>
        /// <param name="screenName">This screen name will be shown in google analytics</param>
        public void TrackPageView(string screenName)
        {
            EasyTracker.GetTracker().SendView(screenName);
        }

        /// <summary>
        /// Tracks a custom event with the given values. This event needs to be predefined in the GA account.
        /// </summary>
        /// <param name="category">Event category.</param>
        /// <param name="action">Event action.</param>
        /// <param name="label">Event label.</param>
        public void TrackCustomEvent(string category, string action, string label)
        {
            int value = 1;

            EasyTracker.GetTracker().SendEvent(category, action, label, value);
        }

        /// <summary>
        /// Tracks an exception with the description. This exception can be seen in GA dashboard.
        /// </summary>
        /// <param name="description">Exception description.</param>
        /// <param name="isFatal">Is this exception causing a crash.</param>
        public void TrackException(string description, bool isFatal)
        {
            EasyTracker.GetTracker().SendException(description, isFatal);
        }

        /// <summary>
        /// Immediately send all queued operations.
        /// </summary>
        public async Task Dispatch()
        {
            await GAServiceManager.Current.Dispatch();
        }

    }
}