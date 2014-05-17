#region

using System.ComponentModel;
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

#endregion

namespace Hoover.Views
{
	public partial class TestMap : PhoneApplicationPage, INotifyPropertyChanged
	{
		private double _previewBoxWidth;
		private double _previewBoxHeight;
		private bool _isMapActive;
		private ObservableCollection<GeoCoordinate> _waypoints;
		private MapRoute _mapRoute;

		public TestMap()
		{
			InitializeComponent();

			_previewBoxWidth = (double)this.Resources["PreviewBoxWidth"];
			_previewBoxHeight = (double)this.Resources["PreviewBoxHeight"];
			this.DataContext = this;
			_isMapActive = false;
			_waypoints = new ObservableCollection<GeoCoordinate>();

			InitARDisplay();

			this.DataContext = this;
		}

		private void InitARDisplay()
		{
			ARDisplay.StartServices();
			this.OverheadMap.Tap += OverheadMapRoute_OnTap;
			_waypoints.Add(ARDisplay.Location);
			this.OverheadMap.Loaded += OverheadMap_Loaded;
		}

		void OverheadMap_Loaded(object sender, RoutedEventArgs e)
		{
			OverheadMap.Map.Layers.RemoveAt(1);
			OverheadMap.Map.Layers.Add(new MapLayer() {
				new MapOverlay() {
					Content = new UserLocationMarker(),
					GeoCoordinate = ARDisplay.Location
				}
			});
		}

		private void OverheadMapRoute_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
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

				this.informationBoard.Text = "Total route length: " + _mapRoute.Route.LengthInMeters + "m\nTotal time: " + _mapRoute.Route.EstimatedDuration; 
			}
			else
			{
				_waypoints.RemoveAt(_waypoints.Count - 1);
			}

			//MessageBox.Show("Distance: " + e.Result.LengthInMeters + "\nTime: " + e.Result.EstimatedDuration);
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			this.ToggleView();
			OverheadMap.Map.Pitch = 75;
			OverheadMap.Map.ZoomLevel = 20;
			OverheadMap.Tap -= OverheadMapRoute_OnTap;
			this.routeMapControls.Visibility = System.Windows.Visibility.Collapsed;
			this.PreviewBox.Visibility = System.Windows.Visibility.Visible;
		}

		private void ClearPointsButton_Click(object sender, RoutedEventArgs e)
		{
			this.OverheadMap.Map.Layers.Clear();
			_waypoints.Clear();
			_waypoints.Add(ARDisplay.Location);
			if (_mapRoute != null)
			{
				this.OverheadMap.Map.RemoveRoute(_mapRoute);
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}

		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			base.OnOrientationChanged(e);
			switch (e.Orientation)
			{
				case PageOrientation.Landscape:
				case PageOrientation.LandscapeLeft:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise270Degrees;
					break;
				case PageOrientation.LandscapeRight:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise90Degrees;
					break;
				case PageOrientation.Portrait:
				case PageOrientation.PortraitUp:
					this.
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Default;
					break;
				case PageOrientation.PortraitDown:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise180Degrees;
					break;
			}
		}

		private void PreviewBox_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			this.ToggleView();
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

				this._isMapActive = false;
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

				_isMapActive = true;
			}
		}

		private void AddPushpinToMap(GeoCoordinate location, string content)
		{
			Pushpin pushpin = new Pushpin()
			{
				Content = content
			};

			this.OverheadMap.Map.Layers.Add(new MapLayer() {
				new MapOverlay() {
						Content = pushpin,
						GeoCoordinate = location,
						PositionOrigin = new Point(0,1)
					}
				});
		}

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