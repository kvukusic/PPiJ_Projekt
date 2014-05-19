#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Helpers;
using Hoover.Model.Weather;
using Hoover.Services;
using Hoover.Settings;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
    public partial class HomeView : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Used to disable loading the city name more than once.
        /// </summary>
        private bool _isCityNameKnown = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HomeView()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Called when the <see cref="HomeView"/> control has loaded.
        /// </summary>
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Load city name
            if (!_isCityNameKnown)
            {
                CityName = await LocationService.Instance.GetCurrentCityName();
                _isCityNameKnown = true;
            }

            // Load Current Weather
            var forecastItem = await new WeatherService().GetCurrentWeatherAsync();
            var currentWeather = new CurrentWeather();
            currentWeather.WeatherMessage = forecastItem.Message;
            currentWeather.Temperature = Convert.ToInt32(Math.Round(forecastItem.Temp)).ToString(CultureInfo.InvariantCulture);
            currentWeather.TemperatureUnit = "o" + (ApplicationSettings.Instance.UseMetricSystem ? " C" : " F");
            currentWeather.IconUrl = "http://openweathermap.org/img/w/" + forecastItem.Icon + ".png";
            CurrentWeather = currentWeather;
            IsWeatherLoaded = true;
        }

        /// <summary>
        /// Executed when the <see cref="StartButton"/> has been tapped.
        /// </summary>
        private void StartButton_Tap(object sender, RoutedEventArgs e)
		{
			Services.NavigationService.Instance.Navigate(Services.PageNames.TrackingPageName);
        }

        private CurrentWeather _CurrentWeather;
        /// <summary>
        /// The current weather.
        /// </summary>
        public CurrentWeather CurrentWeather
        {
            get { return _CurrentWeather; }
            set
            {
                if (value != _CurrentWeather)
                {
                    _CurrentWeather = value;
                    OnPropertyChanged("CurrentWeather");
                }
            }
        }

        private bool _IsWeatherLoaded;
        public bool IsWeatherLoaded
        {
            get { return _IsWeatherLoaded; }
            set
            {
                if (value != _IsWeatherLoaded)
                {
                    _IsWeatherLoaded = value;
                    OnPropertyChanged("IsWeatherLoaded");
                }
            }
        }

        private string _CityName;
        /// <summary>
        /// The city name used for displaying along the weather.
        /// </summary>
        public string CityName
        {
            get { return _CityName; }
            set
            {
                if (value != _CityName)
                {
                    _CityName = value;
                    OnPropertyChanged("CityName");
                }
            }
        }

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    
        #endregion

    }

    /// <summary>
    /// This is the model for the current weather in <see cref="HomeView"/>.
    /// </summary>
    public class CurrentWeather
    {
        public string IconUrl { get; set; }
        public string WeatherMessage { get; set; }
        public string Temperature { get; set; }
        public string TemperatureUnit { get; set; }
    }

}
