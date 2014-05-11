using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps;
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

namespace Hoover.Views
{
	public partial class TestMap : PhoneApplicationPage
	{
		public TestMap()
		{
			InitializeComponent();
			InitARDisplay();
		}

		private void InitARDisplay()
		{
			ARDisplay.StartServices();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ScaleTransform scale = new ScaleTransform();
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
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Default;
					break;
				case PageOrientation.PortraitDown:
					ARDisplay.Orientation = GART.BaseControls.ControlOrientation.Clockwise180Degrees;
					break;
			}
		}

		private void VideoPreview_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var test = this.OverheadMap.Width;
			this.VideoPreview.Width = double.NaN;
			this.VideoPreview.Height = double.NaN;

		}

	}
}