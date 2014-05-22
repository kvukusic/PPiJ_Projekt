#region

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Hoover.Annotations;
using Hoover.Settings;
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
using Microsoft.Devices.Sensors;
using Hoover.Services;
using Microsoft.Xna.Framework;

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
		private DispatcherTimer _timer;
		private MapPolyline _line;
		private MapRoute _mapRoute;
		private MapOverlay _userPushpin;
		private MapLayer _currentLocation;
		private DateTime _startTime;
		private GeoCoordinateWatcher _watcher;
		private double _distance;
		private long _previousPositionChangeTick;
		private int _activeCheckpoint;
		private Model.HistoryItem _currentRoute;
		private bool _firstInit = true;
		private Motion _motion;
		private Gyroscope _gyroscope;
		private bool _motionFlag = true;
		private SpeechRecognitionService _speech;

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
			
		}

		#endregion

		#region Event handlers

		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
			this.ApplicationSettings = Settings.ApplicationSettings.Instance;

			if (_firstInit)
			{
				_isMapActive = !ApplicationSettings.ShowMapSystem;
				_activeCheckpoint = 0;
				_distance = 0;

				_waypoints = new ObservableCollection<GeoCoordinate>();
				_checkpoints = new ObservableCollection<GART.Data.ARItem>();
				_timer = new DispatcherTimer();
				_timer.Interval = TimeSpan.FromSeconds(1);
				_watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
				_speech = new SpeechRecognitionService();

				_previewBoxWidth = (double)this.Resources["PreviewBoxWidth"];
				_previewBoxHeight = (double)this.Resources["PreviewBoxHeight"];

				InitARDisplay();

				// Line is used for tracking position
				_line = new MapPolyline();
				_line.StrokeColor = Colors.Red;
				_line.StrokeThickness = 20;

				OverheadMap.Loaded += OverheadMap_Loaded;
				_watcher.PositionChanged += Watcher_PositionChanged;
				_firstInit = false;
			}
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
			//Exception where are no motion on WP
			//ARDisplay.StopServices();
            _motion.CurrentValueChanged -= Motion_CurrentValueChanged;
			ApplicationSettings = null;
        }

		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			VideoPreview.Margin = new Thickness(-60, 0, -60, 0);
			base.OnOrientationChanged(e);
			ChangeOrientation(e.Orientation);
		}

		private void OverheadMap_Loaded(object sender, RoutedEventArgs e)
		{
			OverheadMap.Tap += OverheadMapRoute_Tap;
			OverheadMap.Map.Layers.RemoveAt(1);

			_currentLocation = new MapLayer() {
				new MapOverlay() {
				Content = new UserLocationMarker(),
				GeoCoordinate = ARDisplay.Location
				}
			};

			if (!ApplicationSettings.ShowPreviewBox)
				PreviewBox.Tap -= PreviewBox_Tap;

			UpdateCurrentLocationPushpin();
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

			query.QueryCompleted += RouteQuery_QueryCompleted;
			query.QueryAsync();
		}

		private void StartButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			OverheadMap.Tap -= OverheadMapRoute_Tap;
			StopButton.Visibility = System.Windows.Visibility.Visible;

			if (_checkpoints.Count > 0)
			{
				SetNewCheckpoint();
			}

			this.ToggleView();
			OverheadMap.Map.Pitch = 75;
			OverheadMap.Map.ZoomLevel = 20;
			OverheadMap.Map.SetView(ARDisplay.Location, 20);
			this.routeMapControls.Visibility = System.Windows.Visibility.Collapsed;
			OverheadMap.Map.Layers.Remove(_currentLocation);

			// Set new description to Information box
			this.description1.Text = "Average speed: ";	//totatlDistance
			this.totalDistance.Text = Helpers.Extensions.Speed(0);
			this.description2.Text = "Time running: ";
			this.description3.Visibility = System.Windows.Visibility.Visible;
			this.totalDistanceRun.Visibility = System.Windows.Visibility.Visible;
			this.totalDistanceRun.Text = Helpers.Extensions.Length(0);

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

			if (ApplicationSettings.EnableSpeechHelper && Motion.IsSupported)
			{
				_motion = new Motion();
			    _motion.TimeBetweenUpdates = TimeSpan.FromSeconds(1);
				_motion.CurrentValueChanged += Motion_CurrentValueChanged;
				_motion.Start();

				_gyroscope = new Gyroscope();
				_gyroscope.TimeBetweenUpdates = TimeSpan.FromSeconds(1);
				_gyroscope.CurrentValueChanged += Gyroscope_CurrentValueChanged;
				_gyroscope.Start();
			}

		}

		private void Gyroscope_CurrentValueChanged(object sender, SensorReadingEventArgs<GyroscopeReading> e)
		{
			var X = e.SensorReading.RotationRate.X;
			var Y = e.SensorReading.RotationRate.Y;
			var Z = e.SensorReading.RotationRate.Z;

			Debug.WriteLine("X " + X + " Y " + Y + " Z " + Z);

		}

		private void Motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
		{
            double pitchValue = Math.Abs(MathHelper.ToDegrees(e.SensorReading.Attitude.Pitch));

		    if (_motionFlag)
		    {
                Dispatcher.BeginInvoke(delegate
                {
					if (this.Orientation == PageOrientation.PortraitUp)
					{
						if (pitchValue > 125)
						{
							ShowWeatherTooltip();
						}
						else if (pitchValue < 25)
						{
							this._isMapActive = false;
							this.ToggleView();
						}
						else
						{
							this._isMapActive = true;
							this.ToggleView();
						}
					}
                });
		    }
            else if (pitchValue > 135 || pitchValue < 5)
            {
                _motionFlag = true;
            }
		}

		private void StopButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			FinishRoute();
		}

		private void ClearPointsButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ClearARItems();
		}

		private void PreviewBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
		    _motionFlag = false;
			this.ToggleView();
		}

		private void RouteQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
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

				OverheadMap.Map.AddRoute(_mapRoute);

				// Align tapped position to calculated point (on road/walk line)
				_waypoints[_waypoints.Count - 1] = _mapRoute.Route.Geometry.Last();

				AddPushpinToMap(_waypoints.Last(), (_waypoints.Count - 1).ToString());
				AddItemToARItems("checkpoint " + (_waypoints.Count - 1), _waypoints.Last(), String.Empty, "/Assets/mapMarkerGray.png");

				this.totalDistance.Text = _mapRoute.Route.Length();
				this.durationTime.Text = _mapRoute.Route.RuningTime();
			}
			else
			{
				_waypoints.RemoveAt(_waypoints.Count - 1);
			}
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
				_distance += distance;
				this.totalDistanceRun.Text = Helpers.Extensions.Length(_distance);
				this.totalDistance.Text = Helpers.Extensions.Speed(_distance / (DateTime.Now - _startTime).TotalSeconds);

				UpdateCheckpointDistance();

				if (_waypoints.Count > _activeCheckpoint && coord.GetDistanceTo(_waypoints[_activeCheckpoint]) < 30)
				{
					SetNewCheckpoint();
				}

				//OverheadMap.Map.SetView(coord, OverheadMap.Map.ZoomLevel, heading, MapAnimationKind.Parabolic);

				//ShellTile.ActiveTiles.First().Update(new IconicTileData()
				//{
				//	Title = "WP8Runner",
				//	WideContent1 = string.Format("{0:f2} km", _kilometres),
				//	WideContent2 = string.Format("{0:f0} calories", _kilometres * 65),
				//});
			}

			_line.Path.Add(coord);
			_previousPositionChangeTick = System.Environment.TickCount;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			TimeSpan runTime = DateTime.Now - _startTime;
			durationTime.Text = runTime.ToString(@"hh\:mm\:ss");
		}

		#endregion

		#region Helper Methods

		private void InitARDisplay()
		{
			ARDisplay.StartServices();
			_waypoints.Add(ARDisplay.Location);
		}

		private void ToggleView()
		{
		    if (VideoPreview == null || OverheadMap == null
                || PreviewBox == null || WorldView == null || ApplicationSettings == null)
		    {
		        return;
		    }

			PreviewBox.Visibility = System.Windows.Visibility.Visible;
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
					this.VideoPreview.Visibility = System.Windows.Visibility.Visible;
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
					this.PreviewBox.Visibility = System.Windows.Visibility.Collapsed;
					this.OverheadMap.Visibility = System.Windows.Visibility.Visible;
					this.VideoPreview.Visibility = System.Windows.Visibility.Collapsed;
				}
			}

			ChangeOrientation(this.Orientation);
		}

		private void AddPushpinToMap(GeoCoordinate location, string content)
		{
			OverheadMap.Map.Layers.Add(new MapLayer() {
				new MapOverlay() {
					Content = new Pushpin() { Content = content },
					GeoCoordinate = location,
					PositionOrigin = new System.Windows.Point(0,0)
				}
			});
		}

		private void StartRoute()
		{
			_timer.Tick += Timer_Tick;
			_startTime = DateTime.Now;

			_timer.Start();
		}

		private void UpdateCurrentLocationPushpin()
		{
			_currentLocation = new MapLayer() {
				new MapOverlay() {
					Content = new UserLocationMarker(),
					GeoCoordinate = ARDisplay.Location
				}
			};

			OverheadMap.Map.Layers.Add(_currentLocation);
		}

		private void AddItemToARItems(string content, GeoCoordinate location, string description, string imageSource)
		{
			_checkpoints.Add(new CheckpointItem()
			{
				Content = content,
				GeoLocation = location,
				Description = description,
				ImageSource = imageSource
			});
		}

		private void ClearARItems()
		{
			ARDisplay.ARItems.Clear();
			WorldView.ARItems.Clear();
			OverheadMap.ARItems.Clear();
			OverheadMap.Map.Layers.Clear();
			_waypoints.Clear();
			_waypoints.Add(ARDisplay.Location);
			_checkpoints.Clear();
			if (_mapRoute != null)
			{
				this.OverheadMap.Map.RemoveRoute(_mapRoute);
			}

			UpdateCurrentLocationPushpin();
		}

		private void SetNewCheckpoint()
		{
			CheckpointItem item;

			// Set previous checkpoint to default value
			if (_activeCheckpoint > 0 && _activeCheckpoint <= _checkpoints.Count)
			{
				item = _checkpoints[_activeCheckpoint-1] as CheckpointItem;
				item.ImageSource = "/Assets/mapFlagMarker.png";
				item.Description = (DateTime.Now - _startTime).ToString();
				_checkpoints[_activeCheckpoint-1] = item;
			}

			if (_checkpoints.Count > 0)
			{
				item = _checkpoints[_activeCheckpoint] as CheckpointItem;
				item.ImageSource = "/Assets/mapMarkerGreen.png";
				int distanceToNextCheckpoint = (int) ARDisplay.Location.GetDistanceTo(item.GeoLocation);
				item.Description = "distance: " + Helpers.Extensions.Length(distanceToNextCheckpoint);

				if (ApplicationSettings.EnableSpeechHelper)
				{
					_speech.SpeakText("Distance to next checkpoint is " + distanceToNextCheckpoint + ((ApplicationSettings.UseMetricSystem) ? " meters" : " yards"));
				}

				_checkpoints[_activeCheckpoint] = item;
			}

			_activeCheckpoint++;

			if (_activeCheckpoint >= _waypoints.Count && _waypoints.Count > 1)
			{
				FinishRoute();
			}

			ARDisplay.ARItems = _checkpoints;
		}

		private void UpdateCheckpointDistance()
		{
			CheckpointItem checkpoint;

			// Get current checkpoint
			if (_activeCheckpoint > 0 && _activeCheckpoint <= _checkpoints.Count)
			{
				checkpoint = _checkpoints[_activeCheckpoint - 1] as CheckpointItem;
				checkpoint.Description = "distance: " + Helpers.Extensions.Length(ARDisplay.Location.GetDistanceTo(checkpoint.GeoLocation));
				_checkpoints[_activeCheckpoint-1] = checkpoint;
			}
		}

		private void ChangeOrientation(PageOrientation orientation)
		{
			VideoPreview.Margin = new Thickness(0, 0, 0, 0);
			VideoPreview.Height = (_isMapActive) ? _previewBoxHeight : Double.NaN;

			switch (orientation)
			{
				case PageOrientation.Landscape:
				case PageOrientation.LandscapeLeft:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise270Degrees;
					if (!_isMapActive)
						VideoPreview.Margin = new Thickness(0, -60, 0, -60);
					break;
				case PageOrientation.LandscapeRight:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise90Degrees;
					if (!_isMapActive)
						VideoPreview.Margin = new Thickness(0, -60, 0, -60);
					break;
				case PageOrientation.Portrait:
				case PageOrientation.PortraitUp:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Default;
					if (!_isMapActive)
						VideoPreview.Margin = new Thickness(-60, 0, -60, 0);
					else
					{
						VideoPreview.Height = _previewBoxWidth * 4 / 3;
						VideoPreview.Margin = new Thickness(0, -(VideoPreview.Height - _previewBoxHeight), 0, 0);
					}
					break;
			}
		}

        /// <summary>
        /// Adds the current route to the history database.
        /// </summary>
        /// <returns></returns>
		private Model.HistoryItem AddRouteToHistory()
		{
			_currentRoute = new Model.HistoryItem()
			{
				AverageSpeed = _distance / (DateTime.Now - _startTime).TotalSeconds,
				EndTime = DateTime.Now,
				StartTime = _startTime,
				RouteLength = (_mapRoute != null) ? _mapRoute.Route.LengthInMeters : _distance,
				Checkpoints = _waypoints.ToList(),
				ID = Helpers.CalendarHelper.FromDateTimeToUnixTime(DateTime.Now)
			};

			App.DataAccess.AddHistoryItem(_currentRoute);

            return _currentRoute;
		}

        /// <summary>
        /// Executed when the user finishes a route.
        /// </summary>
		private async void FinishRoute()
		{
			var item = AddRouteToHistory();
			// Show session completed popup
            if (item != null)
		{
                await PopupService.Instance.ShowSessionCompletedPopup(item);
            }
		}

        /// <summary>
        /// Executed when the mobile phone back is pointed at the sky.
        /// </summary>
		private async void ShowWeatherTooltip()
		{
            await PopupService.Instance.ShowWeatherPopup();
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
		{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

	}
}