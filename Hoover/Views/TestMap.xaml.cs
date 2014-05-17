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

		    this._previewBoxWidth = (double)this.Resources["PreviewBoxWidth"];
		    this._previewBoxHeight = (double)this.Resources["PreviewBoxHeight"];
		    this.DataContext = this;
			this._isMapActive = false;
			this._waypoints = new ObservableCollection<GeoCoordinate>();

			InitARDisplay();
		}

		private void InitARDisplay()
		{
			ARDisplay.StartServices();
			this.OverheadMap.Tap += OverheadMapRoute_OnTap;
			this.OverheadMap.Loaded += Map_Loaded;
			this._waypoints.Add(ARDisplay.Location);
            //this.ToggleView();
		}

		void Map_Loaded(object sender, RoutedEventArgs e)
		{
			this.OverheadMap.map.Pitch = 70;
			//this.OverheadMap.map.CartographicMode = MapCartographicMode.Aerial;
		}

		private void OverheadMapRoute_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			GeoCoordinate location = this.OverheadMap.Map.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.OverheadMap.Map));
			this._waypoints.Add(location);

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
			if (this._mapRoute != null)
			{
				this.OverheadMap.Map.RemoveRoute(this._mapRoute);
			}
			this._mapRoute = new MapRoute(e.Result);
			this._mapRoute.Color = Colors.Gray;
			this._mapRoute.RouteViewKind = RouteViewKind.UserDefined;
			this.OverheadMap.Map.AddRoute(this._mapRoute);
			this._waypoints[this._waypoints.Count - 1] = this._mapRoute.Route.Geometry[_mapRoute.Route.Geometry.Count - 1];

			Pushpin destination = new Pushpin()
			{
				Content = this._waypoints.Count.ToString()
			};

			this.OverheadMap.Map.Layers.Add(new MapLayer() {
				new MapOverlay() {
					Content = destination,
					GeoCoordinate = this._waypoints[this._waypoints.Count-1],
					PositionOrigin = new Point(0,1)
				}
			});

			//this.OverheadMap.Map.SetView(LocationRectangle.CreateBoundingRectangle(_waypoints));
			MessageBox.Show("Distance: " + e.Result.LengthInMeters + "\nTime: " + e.Result.EstimatedDuration);
		}

		private void ClearPointsButton_Click(object sender, RoutedEventArgs e)
		{
			this.OverheadMap.Map.Layers.Clear();
			this._waypoints.Clear();
			this._waypoints.Add(ARDisplay.Location);
			if (this._mapRoute != null)
			{
				this.OverheadMap.Map.RemoveRoute(this._mapRoute);
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

	            this._isMapActive = true;
	        }
	    }
        
        private void PreviewBox_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.ToggleView();
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

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			this.ToggleView();
		}

	}
}