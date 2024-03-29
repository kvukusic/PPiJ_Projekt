﻿#region

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
using Windows.Devices.Geolocation;
using Hoover.Annotations;
using Hoover.Helpers;
using Hoover.Model.Weather;
using Hoover.Services;
using Hoover.Settings;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Shell;
using Hoover.Model;

#endregion

namespace Hoover.Views
{
    public partial class HomeView : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Used to disable loading the city name more than once.
        /// </summary>
        private bool _isCityNameKnown = false;

		private HistoryItem _LastRun;

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

			_CurrentTime = DateTime.Now.ToString("dddd dd.MM.yyyy") + " | " + DateTime.Now.ToString("t");

            // Load Current Weather
            var forecastItem = await new WeatherService().GetCurrentWeatherAsync();
            var currentWeather = new CurrentWeather();
            currentWeather.WeatherMessage = forecastItem.Message;
            currentWeather.Temperature = Convert.ToInt32(Math.Round(forecastItem.Temp)).ToString(CultureInfo.InvariantCulture);
            currentWeather.TemperatureUnit = (ApplicationSettings.Instance.UseMetricSystem ? " °C" : " °F");
            currentWeather.IconUrl = "/Assets/WeatherIcons/" + forecastItem.Icon + ".png";
            CurrentWeather = currentWeather;
            IsWeatherLoaded = true;

			// Load Last Run data
            var items = App.DataAccess.GetAllHistoryItems().OrderBy(item => item.StartTime).ToList();
			if (items.Count > 0)
			{
				_LastRun = items.Last();
				LastRun = "You ran " + _LastRun.RouteLength.Length() + " in " + (_LastRun.EndTime - _LastRun.StartTime).TimeSpanFormatString() +
							" with average speed " + _LastRun.AverageSpeed.Speed() + "\nIt was " + Helpers.CalendarHelper.TimeAgo(_LastRun.EndTime) + "...";
			}
			else
			{
				LastRun = "...you have not ran yet";
			}
        }

        /// <summary>
        /// Executed when the <see cref="StartButton"/> has been tapped.
        /// </summary>
        private void StartButton_Tap(object sender, RoutedEventArgs e)
		{
            Geolocator locator = new Geolocator();
            if (locator.LocationStatus == PositionStatus.Disabled)
            {
                var res = MessageBox.Show("Location services are turned off on your phone. To use this feature, location must be turned on.\n" +
	                                      "Do you want to turn on location services?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (res == MessageBoxResult.OK)
                {
                    Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                }
                else
                {
                    return;
                }
            }

			Services.NavigationService.Instance.Navigate(Services.PageNames.TrackingPageName);
        }

        #region Weather

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

		private string _CurrentTime;
		/// <summary>
		/// The city name used for displaying along the weather.
		/// </summary>
		public string DateAndTime
		{
			get { return _CurrentTime; }
			set
			{
				if (value != _CurrentTime)
				{
					_CurrentTime = value;
					OnPropertyChanged("DateAndTime");
				}
			}
		}

		private string _lastRun;

		/// <summary>
		/// The city name used for displaying along the weather.
		/// </summary>
		public string LastRun
		{
			get
			{
				return _lastRun;
			}
			set
			{
				if (value != _lastRun)
				{
					_lastRun = value;
					OnPropertyChanged("LastRun");
				}
			}
		}

        #endregion

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
