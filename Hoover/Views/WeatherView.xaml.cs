using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Hoover.Helpers;
using Hoover.Settings;

#region

using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Model.Weather;
using Hoover.Services;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
    public partial class WeatherView : UserControl, INotifyPropertyChanged
    {
        public WeatherView()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            WeatherService service = new WeatherService();
            var forecast = await service.GetForecastWeatherAsync();
            if (forecast != null)
            {
                if (forecast.ForecastItems != null)
                {
                    var filteredToday = forecast.ForecastItems.Where(item => CalendarHelper.FromUnixTimeToDateTime(item.Dt).Date == DateTime.Today.Date.AddDays(1));
                    Forecast = new ObservableCollection<object>(filteredToday.Select(item => new
                    {
                        TemperatureString = item.Temp + (ApplicationSettings.Instance.UseMetricSystem ? " C" : " F"),
                        TimeString = CalendarHelper.FromDateTimeToTimeString(CalendarHelper.FromUnixTimeToDateTime(item.Dt)),
                        IconUrl = "http://openweathermap.org/img/w/" + item.Icon + ".png",
                        item.Message
                    }));
                }
            }
        }

        private ObservableCollection<object> _Forecast;
        public ObservableCollection<object> Forecast
        {
            get { return _Forecast; }
            set
            {
                if (value != _Forecast)
                {
                    _Forecast = value;
                    OnPropertyChanged("Forecast");
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
}
