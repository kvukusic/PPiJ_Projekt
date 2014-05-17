#region

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GART.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Shell;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.Recognition;
using Microsoft.Phone.Net.NetworkInformation;
using System.Net.NetworkInformation;
using Windows.Phone.Speech.VoiceCommands;
using System.Threading.Tasks;

#endregion

namespace Hoover.Views
{
	public partial class MainPage : PhoneApplicationPage
	{
		private GeoCoordinate _myLocation;
		private GeoCoordinate _destination;
		private MapRoute _mapRoute;

		private GeoCoordinateCollection _currentWaypoints;

		public MainPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Called when a page becomes the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}

		/// <summary>
		/// Called when a page is no longer the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
		}

		/// <summary>
		/// Called just before a page is no longer the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			base.OnNavigatingFrom(e);
		}

		//private void OverheadMap_OnTap(object sender, GestureEventArgs e)
		//{
		//	Map mapControl = this.OverheadMap.Map;
		//	if (mapControl != null)
		//	{
		//		_destination = mapControl.ConvertViewportPointToGeoCoordinate(e.GetPosition(mapControl));
		//		// Add the current destination to the waypoints list

		//		//mapControl.MapElements.Add(new MapPolyline()
		//		//{
		//		//    StrokeThickness = 3,
		//		//    StrokeColor = Colors.Gray,
		//		//    Path = _currentWaypoints
		//		//});

		//		mapControl.Layers.Add(new MapLayer()
		//		{
		//			new MapOverlay()
		//			{
		//				GeoCoordinate = _destination,
		//				PositionOrigin = new Point(0.5, 0.5),
		//				Content = CreateIndicator(),
		//			}
		//		});
		//		_currentWaypoints.Add(_destination);

		//		RouteQuery query = new RouteQuery()
		//		{
		//			TravelMode = TravelMode.Walking,
		//			Waypoints = _currentWaypoints.ToList()
		//		};

		//		query.QueryCompleted += routeQuery_QueryCompleted;
		//		query.QueryAsync();
		//	}
		//}

		/// <summary>
		/// Creates the indicator on a map.
		/// </summary>
		/// <returns></returns>
		//private UIElement CreateIndicator()
		//{
		//	return new Ellipse()
		//	{
		//		Fill = new SolidColorBrush(Colors.Red),
		//		Width = 15,
		//		Height = 15
		//	};
		//}

		//private void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
		//{
		//	try
		//	{
		//		Map mapControl = this.OverheadMap.Map;
		//		if (mapControl != null)
		//		{
		//			if (_mapRoute != null) mapControl.RemoveRoute(_mapRoute);
		//			_mapRoute = new MapRoute(e.Result);
		//			mapControl.AddRoute(_mapRoute);
		//			//routeInfo.Text = "Distance: " + e.Result.LengthInMeters + "\nTime: " + e.Result.EstimatedDuration;
		//			//routeInfo.Visibility = System.Windows.Visibility.Visible;
		//		}
		//	}
		//	catch (Exception)
		//	{
		//	}
		//}

		private void startGART_Click(object sender, RoutedEventArgs e)
		{
			Services.NavigationService.Instance.Navigate(Services.PageNames.TestPageViewName);
		}

        private async void SpeechButton_Click(object sender, EventArgs e)
        {
           string s=await App.SpeechRecognitionService.RecognizeSpeech();
           
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            Services.NavigationService.Instance.Navigate(Services.PageNames.SettingsPageViewName);
        }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {

        }

	}
}
