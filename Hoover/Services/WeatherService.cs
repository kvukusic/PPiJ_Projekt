#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hoover.Model.Weather;

#endregion

namespace Hoover.Services
{
    /// <summary>
    /// This class contains methods for accessig the google weather API.
    /// </summary>
    public class WeatherService
    {
        /// <summary>
        /// This method asynchronously downloads the forecast of the current location from the 
        /// <code>OpenWeather</code> service.
        /// <example>
        /// http://api.openweathermap.org/data/2.5/forecast?units=metric&lat=35&lon=139
        /// </example>
        /// </summary>
        /// <returns></returns>
        public Task<Forecast> GetForecastWeatherAsync()
        {
            return null;
        }

        private Forecast ParseForecastJson(string json)
        {
            return null;
        }
    }
}