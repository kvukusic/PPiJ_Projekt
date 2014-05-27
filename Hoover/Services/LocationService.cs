#region

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Services;

#endregion

namespace Hoover.Services
{
    /// <summary>
    /// Contains helper classes for location services.
    /// </summary>
    public class LocationService
    {
        #region Singleton

        /// <summary>
        /// Private constructor.
        /// </summary>
        private LocationService()
        {
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static LocationService()
        {
        }

        private static readonly LocationService _instance = new LocationService();
        public static LocationService Instance { get { return _instance; } }

        #endregion

        /// <summary>
        /// Retreives the current <see cref="GeoCoordinate"/> position.
        /// </summary>
        /// <returns></returns>
        public GeoCoordinate GetCurrentPosition()
        {
            GeoCoordinateWatcher geoCoordinateWatcher = new GeoCoordinateWatcher();
            geoCoordinateWatcher.Start();
            var position = geoCoordinateWatcher.Position;
            geoCoordinateWatcher.Dispose();
            return position.Location;
        }

        /// <summary>
        /// Retreives the city name of the current location.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentCityName()
        {
            TaskCompletionSource<string> waitForCompletion = new TaskCompletionSource<string>(null);

            ReverseGeocodeQuery query = new ReverseGeocodeQuery();
            query.GeoCoordinate = GetCurrentPosition();
            query.QueryCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    waitForCompletion.TrySetResult(null);
                }

                if (e.Result.Count > 0)
                {

                    MapAddress address = e.Result[0].Information.Address;
                    string city = address.City;
                    waitForCompletion.TrySetResult(city);
                }
                else
                {
                    waitForCompletion.TrySetResult(null);
                }
            };

            query.QueryAsync();

            return await waitForCompletion.Task;
        }
    }
}