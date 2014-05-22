#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
	public partial class StatisticsView : UserControl, INotifyPropertyChanged
	{
		public StatisticsView()
		{
			InitializeComponent();

		    this.DataContext = this;
            this.Loaded += OnLoaded;
		}

	    private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	    {
	        var historyItems = App.DataAccess.GetAllHistoryItems();
	        if (historyItems != null && historyItems.Count > 0)
	        {
	            int count = historyItems.Count;
	            double totalSpeed = 0.0;
	            double totalDistance = 0.0;
	            double totalTimeSecs = 0;
	            foreach (var item in historyItems)
	            {
                    totalSpeed += item.AverageSpeed;
	                totalDistance += item.RouteLength;
	                totalTimeSecs += (item.EndTime - item.StartTime).TotalSeconds;
	            }

	            this.AverageSpeed = (totalSpeed/count).Speed();
	            this.AverageDistance = (totalDistance/count).Length();
	            this.AverageTime = TimeSpan.FromSeconds(totalTimeSecs/count).TimeSpanFormatString();

	            this.TotalDistance = totalDistance.Length();
	            this.TotalTime = TimeSpan.FromSeconds(totalTimeSecs).TimeSpanFormatString();
	        }
	        else
	        {
	            // Set defaults
	            this.AverageSpeed = 0.0.Speed();
	            this.AverageDistance = 0.0.Length();
	            this.AverageTime = new TimeSpan(0, 0, 0).TimeSpanFormatString();
	            this.TotalDistance = 0.0.Length();
	            this.TotalTime = new TimeSpan(0, 0, 0).TimeSpanFormatString();
	        }
	    }

        #region Properties

        private string _AverageSpeed;
        public string AverageSpeed
        {
            get { return _AverageSpeed; }
            set
            {
                if (value != _AverageSpeed)
                {
                    _AverageSpeed = value;
                    OnPropertyChanged("AverageSpeed");
                }
            }
        }

        private string _AverageDistance;
        public string AverageDistance
        {
            get { return _AverageDistance; }
            set
            {
                if (value != _AverageDistance)
                {
                    _AverageDistance = value;
                    OnPropertyChanged("AverageDistance");
                }
            }
        }

        private string _AverageTime;
        public string AverageTime
        {
            get { return _AverageTime; }
            set
            {
                if (value != _AverageTime)
                {
                    _AverageTime = value;
                    OnPropertyChanged("AverageTime");
                }
            }
        }

        private string _TotalDistance;
        public string TotalDistance
        {
            get { return _TotalDistance; }
            set
            {
                if (value != _TotalDistance)
                {
                    _TotalDistance = value;
                    OnPropertyChanged("TotalDistance");
                }
            }
        }

        private string _TotalTime;
        public string TotalTime
        {
            get { return _TotalTime; }
            set
            {
                if (value != _TotalTime)
                {
                    _TotalTime = value;
                    OnPropertyChanged("TotalTime");
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
}
