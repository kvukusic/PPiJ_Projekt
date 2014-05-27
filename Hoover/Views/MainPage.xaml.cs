#region

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Geolocation;
using GART.Controls;
using Hoover.Services;
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
            // Check if internet is turned on
	        if (!DeviceNetworkInformation.IsNetworkAvailable)
	        {
	            MessageBox.Show("This application will not work without an active internet connection. " +
	                            "The application will now terminate.", "No internet", MessageBoxButton.OK);
                Application.Current.Terminate();
	        }
            // Check if location is turned on
            Geolocator locator = new Geolocator();
	        if (locator.LocationStatus == PositionStatus.Disabled)
	        {
	            var res = MessageBox.Show("Location services are turned off on your phone. App functionality will be limited.\n" +
	                                      "Do you want to turn on location services?", 
                    "Location", 
                    MessageBoxButton.OKCancel);

	            if (res == MessageBoxResult.OK)
	            {
	                Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
	            }
	        }

            AnalyticsService.Instance.TrackPageView("MainPage");
	    }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            Services.NavigationService.Instance.Navigate(Services.PageNames.SettingsPageName);
        }

	    private void AboutMenuItem_Click(object sender, EventArgs e)
	    {
	        Services.NavigationService.Instance.Navigate(Services.PageNames.AboutPageName);
	    }
	}
}
