﻿#region

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Hoover.Annotations;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using GART;
using GART.Controls;
using System.Windows.Media;
using Microsoft.Devices;
using System.Device.Location;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Services;
using System.Windows.Media.Imaging;
using Hoover.Helpers;
using Hoover.Controls;
using Hoover.Common;
using System.Windows.Threading;

#endregion

namespace Hoover.Views
{
	public partial class TrackingPage : PhoneApplicationPage, INotifyPropertyChanged
	{

		#region Fields

		private double _previewBoxWidth;
		private double _previewBoxHeight;
		private bool _isMapActive;
		private ObservableCollection<GeoCoordinate> _waypoints;
		private ObservableCollection<GART.Data.ARItem> _checkpoints;
		private MapRoute _mapRoute;
		private MapOverlay _userPushpin;
		private MapLayer _currentLocation;
		private DispatcherTimer _timer;
		private long _startTime;
		private GeoCoordinateWatcher _watcher;
		private MapPolyline _line;
		private double _kilometres;
		private long _previousPositionChangeTick;

		#endregion

		#region Properties

		public Settings.ApplicationSettings ApplicationSettings
		{
			get;
			set;
		}



		#endregion

		#region Constructor

		public TrackingPage()
		{
			InitializeComponent();

			this.DataContext = this;
			this.ApplicationSettings = Settings.ApplicationSettings.Instance;

			_previewBoxWidth = (double)this.Resources["PreviewBoxWidth"];
			_previewBoxHeight = (double)this.Resources["PreviewBoxHeight"];
			_isMapActive = true;
		}

		#endregion

		#region Event handlers

		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

			_waypoints = new ObservableCollection<GeoCoordinate>();
			_checkpoints = new ObservableCollection<GART.Data.ARItem>();
			_timer = new DispatcherTimer();
			_watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

			InitARDisplay();

			_line = new MapPolyline();
			_line.StrokeColor = Colors.Red;
			_line.StrokeThickness = 20;

			_watcher.PositionChanged += Watcher_PositionChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
			ClearARItems();
			ApplicationSettings = null;
        }

		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			VideoPreview.Margin = new Thickness(-60, 0, -60, 0);
			base.OnOrientationChanged(e);
			switch (e.Orientation)
			{
				case PageOrientation.Landscape:
				case PageOrientation.LandscapeLeft:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise270Degrees;
					if (!_isMapActive)
						VideoPreview.Margin = new Thickness(0, -60, 0, -60);
					else
						VideoPreview.Margin = new Thickness(0, 0, 0, 0);
					break;
				case PageOrientation.LandscapeRight:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise90Degrees;
					if (!_isMapActive)
						VideoPreview.Margin = new Thickness(0, -60, 0, -60);
					else
						VideoPreview.Margin = new Thickness(0, 0, 0, 0);
					break;
				case PageOrientation.Portrait:
				case PageOrientation.PortraitUp:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Default;
					if (!_isMapActive)
						VideoPreview.Margin = new Thickness(-60, 0, -60, 0);
					else
						VideoPreview.Margin = new Thickness(0, 0, 0, 0);
					break;
			}
		}

		void OverheadMap_Loaded(object sender, RoutedEventArgs e)
		{
			OverheadMap.Tap += OverheadMapRoute_Tap;
			OverheadMap.Map.Layers.RemoveAt(1);
			_currentLocation = new MapLayer() {
				new MapOverlay() {
				Content = new UserLocationMarker(),
				GeoCoordinate = ARDisplay.Location
				}
			};

			OverheadMap.Map.Layers.Add(_currentLocation);
			OverheadMap.Map.MapElements.Add(_line);
		}

		private void OverheadMapRoute_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			GeoCoordinate location = this.OverheadMap.Map.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.OverheadMap.Map));
			_waypoints.Add(location);

			RouteQuery query = new RouteQuery()
			{
				TravelMode = TravelMode.Walking,
				Waypoints = _waypoints
			};

			query.QueryCompleted += routeQuery_QueryCompleted;
			query.QueryAsync();
		}

		private void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
		{
			if (e.Error == null)
			{
				// RouteQuery returned valid Route as result
				if (this._mapRoute != null)
				{
					this.OverheadMap.Map.RemoveRoute(_mapRoute);
				}
				_mapRoute = new MapRoute(e.Result);
				_mapRoute.Color = Colors.Gray;
				_mapRoute.RouteViewKind = RouteViewKind.UserDefined;
				this.OverheadMap.Map.AddRoute(_mapRoute);
				_waypoints[_waypoints.Count - 1] = _mapRoute.Route.Geometry[_mapRoute.Route.Geometry.Count - 1];

				AddPushpinToMap(_waypoints.Last(), (_waypoints.Count - 1).ToString());
				AddItemToARItems("checkpoint " + (_waypoints.Count - 1), _waypoints.Last(), "distance");

				this.totalDistance.Text = _mapRoute.Route.Length();
				this.durationTime.Text = _mapRoute.Route.RuningTime();
			}
			else
			{
				_waypoints.RemoveAt(_waypoints.Count - 1);
			}
		}

		private void StartButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ARDisplay.ARItems = _checkpoints;
			this.ToggleView();
			OverheadMap.Map.Pitch = 75;
			OverheadMap.Map.ZoomLevel = 20;
			OverheadMap.Tap -= OverheadMapRoute_Tap;
			this.routeMapControls.Visibility = System.Windows.Visibility.Collapsed;
			this.PreviewBox.Visibility = System.Windows.Visibility.Visible;
			this.RouteInformationBox.Visibility = System.Windows.Visibility.Collapsed;
			OverheadMap.Map.Layers.Remove(_currentLocation);

			// Add heading indicator to map
			UserPushpin pushpin = new Controls.UserPushpin();
			pushpin.DataContext = ARDisplay;
			_userPushpin = new MapOverlay()
			{
				Content = pushpin,
				GeoCoordinate = ARDisplay.Location
			};

			OverheadMap.Map.Layers.Add(new MapLayer() { _userPushpin });

			StartRoute();
			_watcher.Start();
		}

		private void ClearPointsButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ClearARItems();
		}

		private void PreviewBox_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			this.ToggleView();
		}

		private void ARDisplay_LocationChanged(object sender, EventArgs e)
		{
			if (_userPushpin != null)
			{
				_userPushpin.GeoCoordinate = ARDisplay.Location;
			}
		}

		private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
		{
			var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

			if (_line.Path.Count > 0)
			{
				var previousPoint = _line.Path.Last();
				var distance = coord.GetDistanceTo(previousPoint);
				var millisPerKilometer = (1000.0 / distance) * (System.Environment.TickCount - _previousPositionChangeTick);
				_kilometres += distance / 1000.0;

				//paceLabel.Text = TimeSpan.FromMilliseconds(millisPerKilometer).ToString(@"mm\:ss");
				//distanceLabel.Text = string.Format("{0:f2} km", _kilometres);
				//caloriesLabel.Text = string.Format("{0:f0}", _kilometres * 65);

				//PositionHandler handler = new PositionHandler();
				//var heading = handler.CalculateBearing(new Position(previousPoint), new Position(coord));
				//Map.SetView(coord, OverheadMap.Map.ZoomLevel, heading, MapAnimationKind.Parabolic);

				//ShellTile.ActiveTiles.First().Update(new IconicTileData()
				//{
				//	Title = "WP8Runner",
				//	WideContent1 = string.Format("{0:f2} km", _kilometres),
				//	WideContent2 = string.Format("{0:f0} calories", _kilometres * 65),
				//});
			}
			else
			{
				OverheadMap.Map.Center = coord;
			}
			_line.Path.Add(coord);
			_previousPositionChangeTick = System.Environment.TickCount;
		}

		#endregion

		#region Helper Methods

		private void InitARDisplay()
		{
			ARDisplay.StartServices();
			OverheadMap.Loaded += OverheadMap_Loaded;
			_waypoints.Add(ARDisplay.Location);
		}

		private void ToggleView()
		{
			if (_isMapActive)
			{
				this.OverheadMap.VerticalAlignment = VerticalAlignment.Top;
				this.OverheadMap.HorizontalAlignment = HorizontalAlignment.Right;
				this.OverheadMap.Width = _previewBoxWidth;
				this.OverheadMap.Height = _previewBoxHeight;
				Canvas.SetZIndex(this.OverheadMap, 1);

				this.VideoPreview.VerticalAlignment = VerticalAlignment.Stretch;
				this.VideoPreview.HorizontalAlignment = HorizontalAlignment.Stretch;
				this.VideoPreview.Width = Double.NaN;
				this.VideoPreview.Height = Double.NaN;
				Canvas.SetZIndex(this.VideoPreview, 0);

				this.WorldView.Visibility = System.Windows.Visibility.Visible;

				this._isMapActive = false;

				if (!ApplicationSettings.ShowPreviewBox)
				{
					this.PreviewBox.Visibility = System.Windows.Visibility.Collapsed;
					this.OverheadMap.Visibility = System.Windows.Visibility.Collapsed;
					this.OverheadMap.Visibility = System.Windows.Visibility.Visible;
				}
			}
			else
			{
				this.VideoPreview.VerticalAlignment = VerticalAlignment.Top;
				this.VideoPreview.HorizontalAlignment = HorizontalAlignment.Right;
				this.VideoPreview.Width = _previewBoxWidth;
				this.VideoPreview.Height = _previewBoxHeight;
				Canvas.SetZIndex(this.VideoPreview, 1);

				this.OverheadMap.VerticalAlignment = VerticalAlignment.Stretch;
				this.OverheadMap.HorizontalAlignment = HorizontalAlignment.Stretch;
				this.OverheadMap.Width = Double.NaN;
				this.OverheadMap.Height = Double.NaN;
				Canvas.SetZIndex(this.OverheadMap, 0);

				this.WorldView.Visibility = System.Windows.Visibility.Collapsed;

				_isMapActive = true;

				if (!ApplicationSettings.ShowPreviewBox)
				{
					this.PreviewBox.Visibility = System.Windows.Visibility.Visible;
					this.OverheadMap.Visibility = System.Windows.Visibility.Visible;
					this.OverheadMap.Visibility = System.Windows.Visibility.Collapsed;
				}
			}
		}

		private void AddPushpinToMap(GeoCoordinate location, string content)
		{
			this.OverheadMap.Map.Layers.Add(new MapLayer() {
				new MapOverlay() {
						Content = new Pushpin() {
							Content = content
						},
						GeoCoordinate = location,
						PositionOrigin = new Point(0,1)
					}
				});
		}

		private void StartRoute()
		{
			_timer.Interval = TimeSpan.FromSeconds(1);
			_timer.Tick += Timer_Tick;
			_timer.Start();
			_startTime = System.Environment.TickCount;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			TimeSpan runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
			TotalRunningTime.Text = runTime.ToString(@"hh\:mm\:ss");
		}


		private void AddItemToARItems(string content, GeoCoordinate location, string description)
		{
			_checkpoints.Add(new CheckpointItem()
			{
				Content = content,
				GeoLocation = location,
				Description = description
			});
		}

		private void ClearARItems()
		{
			ARDisplay.ARItems.Clear();
			WorldView.ARItems.Clear();
			OverheadMap.ARItems.Clear();
			_waypoints.Clear();
			_waypoints.Add(ARDisplay.Location);
			if (_mapRoute != null)
			{
				this.OverheadMap.Map.RemoveRoute(_mapRoute);
			}
		}

		#endregion

		#region INotifyPropertyChanged

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