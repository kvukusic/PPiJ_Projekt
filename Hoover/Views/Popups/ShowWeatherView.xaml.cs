﻿#region

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
            if (currentWeather == null)
            {
                RaiseNoData();
                return;
            }
            var forecastWeather = await service.GetForecastWeatherAsync();


            // Autoclose after 15 seconds
            await Task.Delay(TimeSpan.FromSeconds(10));
            RaiseCloseRequested();
        }

        private string _CurrentTemperature;
        public string CurrentTemperature
        {
            get { return _CurrentTemperature; }
            set
            {
                if (value != _CurrentTemperature)
                {
                    _CurrentTemperature = value;
                    OnPropertyChanged("CurrentTemperature");
                }
            }
        }

        private string _CurrentCity;
        public string CurrentCity
        {
            get { return _CurrentCity; }
            set
            {
                if (value != _CurrentCity)
                {
                    _CurrentCity = value;
                    OnPropertyChanged("CurrentCity");
                }
            }
        }

        private string _CurrentMessage;
        public string CurrentMessage
        {
            get { return _CurrentMessage; }
            set
            {
                if (value != _CurrentMessage)
                {
                    _CurrentMessage = value;
                    OnPropertyChanged("CurrentMessage");
                }
            }
        }

        #region Events

        public event EventHandler<EventArgs> CloseRequested;
        private void RaiseCloseRequested()
        {
            if (CloseRequested != null)
            {
                CloseRequested(this, new EventArgs());
            }
        }

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
