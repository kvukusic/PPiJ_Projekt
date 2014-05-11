#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Hoover.Annotations;
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

#endregion

namespace Hoover.Views
{
	public partial class TestMap : PhoneApplicationPage, INotifyPropertyChanged
	{
	    private double _previewBoxWidth;
	    private double _previewBoxHeight;

		public TestMap()
		{
			InitializeComponent();

		    this._previewBoxWidth = (double)this.Resources["PreviewBoxWidth"];
		    this._previewBoxHeight = (double)this.Resources["PreviewBoxHeight"];
		    this.DataContext = this;

			InitARDisplay();
		}

		private void InitARDisplay()
		{
			ARDisplay.StartServices();

            this.ToggleView();
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
                    this.
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

	    private bool _isMapActive = false;

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
        
        private void PreviewBox_OnTap(object sender, GestureEventArgs e)
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

	}
}