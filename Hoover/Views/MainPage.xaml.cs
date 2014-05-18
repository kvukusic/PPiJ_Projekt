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
		public MainPage()
		{
			InitializeComponent();
		}

        private async void SpeechButton_Click(object sender, EventArgs e)
        {
           string s=await App.SpeechRecognitionService.RecognizeSpeech();
           
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            Services.NavigationService.Instance.Navigate(Services.PageNames.SettingsPageName);
        }

	}
}
