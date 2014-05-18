#region

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Hoover.Helpers;
using Hoover.Model.Weather;
using Hoover.Settings;
using Newtonsoft.Json.Linq;

#endregion

namespace Hoover.Services
{
    /// <summary>
    /// This class contains methods for accessig the google weather API.
    /// </summary>
    public class WeatherService
    {
        /// <summary>
        /// This is the base url of the weather service.
        /// </summary>
        private const string BaseUrl = "http://api.openweathermap.org/data/2.5/";

        /// <summary>
        /// This method asynchronously downloads the forecast of the current location from the 
        /// <code>OpenWeather</code> service.
        /// <example>
        /// http://api.openweathermap.org/data/2.5/forecast?units=metric&lat=35&lon=139
        /// </example>
        /// </summary>
        /// <returns></returns>
        public async Task<Forecast> GetForecastWeatherAsync()
        {
            HttpClient client = new HttpClient();
            var unit = ApplicationSettings.Instance.UseMetricSystem ? "metric" : "imperial";
            GeoCoordinateWatcher geoCoordinate = new GeoCoordinateWatcher();
            var position = geoCoordinate.Position;
            var lat = position.Location.Latitude.ToString(CultureInfo.InvariantCulture);
            var lon = position.Location.Longitude.ToString(CultureInfo.InvariantCulture);
            // Create url
            var url = new Uri(BaseUrl + "forecast?units=" + unit + "&lat=" + lat + "&lon=" + lon);

            string json = await client.GetStringAsync(url);
            return ParseForecastJson(json);
        }
        
        /// <summary>
        /// Parses the given json string into a <see cref="ForecastItem"/>. Returns <code>null</code> if
        /// not possible.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private ForecastItem ParseForecastItem(string json)
        {
            try
            {
                JToken root = JToken.Parse(json);

                // Validate the rootObject
                if (!ValidatePayload(root)) return null;

                JObject rootJObject = (JObject) root;

                var dt = rootJObject.GetValueOrDefault<long>("dt");
                double temp = 0.0;
                JObject mainJObject = rootJObject.GetValueOrDefault<JObject>("main");
                if (mainJObject != null)
                {
                    temp = mainJObject.GetValueOrDefault<double>("temp");
                }
                string message = null;
                string icon = null;
                JArray weatherJArray = rootJObject.GetValueOrDefault<JArray>("weather");
                if (weatherJArray != null)
                {
                    if (weatherJArray.Count > 0)
                    {
                        foreach (JObject weatherJObject in weatherJArray)
                        {
                            message = weatherJObject.GetValueOrDefault<string>("main");
                            icon = weatherJObject.GetValueOrDefault<string>("icon");
                            break;
                        }
                    }
                }
                var name = rootJObject.GetValueOrDefault<string>("name");
                return new ForecastItem()
                {
                    Dt = dt,
                    Temp = temp,
                    Message = message,
                    Icon = icon,
                    Name = name
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Parses the json string retreived from the weather service and returns the <see cref="Forecast"/> object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private Forecast ParseForecastJson(string json)
        {
            try
            {
                JToken root = JToken.Parse(json);

                // Validate the rootObject
                if (!ValidatePayload(root)) return null;

                JObject rootJObject = (JObject) root;
                if (rootJObject != null)
                {
                    City city = null;
                    JObject cityJObject = rootJObject.GetValueOrDefault<JObject>("city");
                    if (cityJObject != null)
                    {
                        var name = cityJObject.GetValueOrDefault<string>("name");
                        var country = cityJObject.GetValueOrDefault<string>("country");
                        city = new City()
                        {
                            Name = name,
                            Country = country
                        };
                    }
                    List<ForecastItem> items = new List<ForecastItem>();
                    JArray forecastJArray = rootJObject.GetValueOrDefault<JArray>("list");
                    foreach (JObject forecastItemJObject in forecastJArray)
                    {
                        items.Add(ParseForecastItem(forecastItemJObject.ToString()));
                    }
                    return new Forecast()
                    {
                        City = city,
                        ForecastItems = items
                    };
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// This method checks if the given payload isn't null. Used before every json parsing to 
        /// cancel if the payload is null.
        /// </summary>
        /// <param name="root">The root object of a json tree hierarchy.</param>
        /// <returns>True if valid, otherwise false.</returns>
        public bool ValidatePayload(JToken root)
        {
            // If null
            if (root == null) return false;

            // If payload is empty
            if (root is JArray && !root.HasValues) return false;
            if (root is JObject && !root.HasValues) return false;
            if (root is JValue && !root.HasValues) return false;

            return true;
        }

    }
}