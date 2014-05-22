#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hoover.Helpers;
using Hoover.Model.Weather;
using Hoover.Settings;

#endregion

namespace Hoover.Views.WeatherItems
{
    public class WeatherItem
    {
        private ForecastItem _model;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WeatherItem(ForecastItem model)
        {
            this._model = model;

            TemperatureString = Convert.ToInt32(Math.Round(model.Temp)) + (ApplicationSettings.Instance.UseMetricSystem ? " °C" : " °F");
            TimeString = CalendarHelper.FromDateTimeToTimeString(CalendarHelper.FromUnixTimeToDateTime(model.Dt));
            IconUri = "/Assets/WeatherIcons/" + model.Icon + ".png";
            Message = model.Message;
        }

        /// <summary>
        /// Header constructor.
        /// </summary>
        public WeatherItem(string title)
        {
            this.IsHeader = true;
            this.Title = title;
        }

        public string TemperatureString { get; set; }
        public string TimeString { get; set; }
        public string IconUri { get; set; }
        public string Message { get; set; }

        public bool IsHeader { get; set; }
        public string Title { get; set; }
    }
}