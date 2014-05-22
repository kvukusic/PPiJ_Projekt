#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Services;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views.Popups
{
    public partial class ShowWeatherView : UserControl, INotifyPropertyChanged
    {
        public ShowWeatherView()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var service = new WeatherService();
            var currentWeather = await service.GetCurrentWeatherAsync();
            await Task.Delay(5000);
            RaiseNoData();
            if (currentWeather == null)
            {
                RaiseNoData();
                return;
            }
            var forecastWeather = await service.GetForecastWeatherAsync();
        }

        #region Events

        public event EventHandler<EventArgs> NoData;
        private void RaiseNoData()
        {
            if (NoData != null)
            {
                NoData(this, new EventArgs());
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
}
