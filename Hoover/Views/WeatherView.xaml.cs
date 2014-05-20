#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Hoover.Helpers;
using Hoover.Settings;

using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Model.Weather;
using Hoover.Services;
using Hoover.Views.WeatherItems;
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
                    var filteredToday = forecast.ForecastItems.Where(item => CalendarHelper.FromUnixTimeToDateTime(item.Dt).Date == DateTime.Today.Date);
                    var filteredTommorow = forecast.ForecastItems.Where(item => CalendarHelper.FromUnixTimeToDateTime(item.Dt).Date == DateTime.Today.AddDays(1).Date);

                    var resultForecast = new ObservableCollection<WeatherItem>();
                    resultForecast.Add(new WeatherItem("TODAY"));
                    foreach (var item in filteredToday)
                    {
                        resultForecast.Add(new WeatherItem(item));
                    }

                    resultForecast.Add(new WeatherItem("TOMORROW"));
                    foreach (var item in filteredTommorow)
                    {
                        resultForecast.Add(new WeatherItem(item));
                    }

                    Forecast = resultForecast;
                }
            }

            IsLoaded = true;
        }

        private bool _IsLoaded;
        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set
            {
                if (value != _IsLoaded)
                {
                    _IsLoaded = value;
                    OnPropertyChanged("IsLoaded");
                }
            }
        }

        private ObservableCollection<WeatherItem> _Forecast;
        public ObservableCollection<WeatherItem> Forecast
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
