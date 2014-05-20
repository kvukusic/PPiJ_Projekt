#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Views.HistoryItems;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Hoover.Helpers;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Windows.Media;

#endregion

namespace Hoover.Views
{
    public partial class HistoryDetailsPage : PhoneApplicationPage, INotifyPropertyChanged
	{

		#region Fields and Properties

		/// <summary>
        /// The item for this details view.
        /// </summary>
        private HistoryViewItem _historyItem;
		private MapRoute _mapRoute;
		private Visibility _mapVisibility;

		public string DateOfRoute
		{
			get
			{
				return _historyItem.StartDate;
			}
		}

		public string AverageSpeed
		{
			get
			{
				return _historyItem.AverageSpeed;
			}
		}

		public string RouteLength
		{
			get
			{
				return _historyItem.Distance;
			}
		}

		public string RuningTime
		{
			get
			{
				return _historyItem.TotalTime;
			}
		}

		public Visibility ShowMap
		{
			get
			{
				return _mapVisibility;
			}
		}

		#endregion

		/// <summary>
        /// Constructor.
        /// </summary>
        public HistoryDetailsPage()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
            this.DataContext = this;
			_mapVisibility = System.Windows.Visibility.Collapsed;
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			try
			{
				var parameter = Services.NavigationService.Instance.GetLastNavigationParameter<HistoryViewItem>();

				if (parameter != null)
				{
					_historyItem = parameter;
				}

				if (_historyItem != null)
				{

				}
			}
			catch (Exception)
			{
				return;
			}

		}

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
			if (_historyItem.Waypoints.Count > 1)
			{
				GenerateMapRoute();
				_mapVisibility = System.Windows.Visibility.Visible;
                OnPropertyChanged("ShowMap");
			}
        }

		private void GenerateMapRoute()
		{
			if (_historyItem.Waypoints.Count < 2)
			{
				_mapRoute = null;
			}
			else
			{
				RouteQuery query = new RouteQuery()
				{
					TravelMode = TravelMode.Walking,
					Waypoints = _historyItem.Waypoints
				};

				query.QueryCompleted += RouteQuery_QueryCompleted;
				query.QueryAsync();
			}
		}

		private void RouteQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Microsoft.Phone.Maps.Services.Route> e)
		{
			_mapRoute = new MapRoute(e.Result);
			_mapRoute.Color = Colors.Gray;
			_mapRoute.RouteViewKind = RouteViewKind.UserDefined;

			mapControl.AddRoute(_mapRoute);
			mapControl.SetView(_mapRoute.Route.BoundingBox);
			//Map.AddRoute(_mapRoute);
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