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
        SpeechSynthesizer _synthesizer;                             // The speech synthesizer (text-to-speech, TTS) object
        SpeechRecognizer _recognizer;                               // The speech recognition object
        SpeechRecognizerUI speechRecognizerUI = new SpeechRecognizerUI();
        private GeoCoordinate _myLocation;
        private GeoCoordinate _destination;
        private MapRoute _mapRoute;

        private GeoCoordinateCollection _currentWaypoints;

        public MainPage()
        {
            InitializeComponent();
        }

        private async Task RecognizeSpeech()
        {
            try
            {
                speechRecognizerUI.Settings.ListenText = "Say your command...";
                speechRecognizerUI.Settings.ExampleText = "Start Stop";
                speechRecognizerUI.Settings.ReadoutEnabled = true;
                speechRecognizerUI.Settings.ShowConfirmation = false;
                SpeechRecognitionUIResult recognitionResult = await speechRecognizerUI.RecognizeWithUIAsync();
                MessageBox.Show(recognitionResult.RecognitionResult.Text, "Result", MessageBoxButton.OK);
            }
           // Dispatcher.BeginInvoke{          }
            catch (Exception ex) // Catch exception and call function to handle it.
            {
               HandleSpeechSynthesisError(ex);
            }
        }
       
        private static void HandleSpeechSynthesisError(Exception ex)
        {
            // convert the HResult code to an int for comparison to returned
            const uint AbortedCallHResult = 0x80045508; // SPERR_SYSTEM_CALL_INTERRUPTED
            const uint GenericInternalHResult = 0x800455A0; // SPERR_WINRT_INTERNAL_ERROR
            const uint AlreadyInLexiconHResult = 0x800455A1; // SPERR_WINRT_ALREADY_IN_LEX
            const uint NotInLexiconHResult = 0x800455A2; // SPERR_WINRT_NOT_IN_LEX
            const uint UnsupportedPhonemeHResult = 0x800455B5; // SPERR_WINRT_UNSUPPORTED_PHONEME
            const uint PhonemeConversionHResult = 0x800455B6; // SPERR_WINRT_PHONEME_CONVERSION
            const uint InvalidLexiconHResult = 0x800455B8; // SPERR_WINRT_LEX_INVALID_DATA
            const uint UnsupportedLanguageHResult = 0x800455BC; // SPERR_WINRT_UNSUPPORTED_LANG
            const uint StringTooLongHResult = 0x800455BD; // SPERR_WINRT_STRING_TOO_LONG
            const uint StringEmptyHResult = 0x800455BE; // SPERR_WINRT_STRING_EMPTY
            const uint NoMoreItemsHResult = 0x800455BF; // SPERR_WINRT_NO_MORE_ITEMS


            if ((uint)ex.HResult == AbortedCallHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_SYSTEM_CALL_INTERRUPTED", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == GenericInternalHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_INTERNAL_ERROR", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == AlreadyInLexiconHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_ALREADY_IN_LEX", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == NotInLexiconHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_NOT_IN_LEX", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == UnsupportedPhonemeHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_UNSUPPORTED_PHONEME", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == PhonemeConversionHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_PHONEME_CONVERSION", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == InvalidLexiconHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_LEX_INVALID_DATA", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == UnsupportedLanguageHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_UNSUPPORTED_LANG", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == StringTooLongHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_STRING_TOO_LONG", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == StringEmptyHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_STRING_EMPTY", MessageBoxButton.OK);
            }
            else if ((uint)ex.HResult == NoMoreItemsHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_NO_MORE_ITEMS", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK);
            }
        }

        private async void InitMainPage()
        {
           
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ARDisplay.StartServices();

            this._currentWaypoints = new GeoCoordinateCollection();

            this._myLocation = ARDisplay.Location;
            this._currentWaypoints.Add(_myLocation);
            this.InitMainPage();

        }

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
			//ARDisplay.StopServices();
            base.OnNavigatedFrom(e);

            ARDisplay.StopServices();
        }

        private void OverheadMap_OnTap(object sender, GestureEventArgs e)
        {
            Map mapControl = this.OverheadMap.Map;
            if (mapControl != null)
            {
                _destination = mapControl.ConvertViewportPointToGeoCoordinate(e.GetPosition(mapControl));
                // Add the current destination to the waypoints list

                //mapControl.MapElements.Add(new MapPolyline()
                //{
                //    StrokeThickness = 3,
                //    StrokeColor = Colors.Gray,
                //    Path = _currentWaypoints
                //});

                mapControl.Layers.Add(new MapLayer()
                {
                    new MapOverlay()
                    {
                        GeoCoordinate = _destination,
                        PositionOrigin = new Point(0.5, 0.5),
                        Content = CreateIndicator(),
                    }
                });
                _currentWaypoints.Add(_destination);

                RouteQuery query = new RouteQuery()
                {
                    TravelMode = TravelMode.Walking,
                    Waypoints = _currentWaypoints.ToList()
                };

                query.QueryCompleted += routeQuery_QueryCompleted;
                query.QueryAsync();
            }
        }

        /// <summary>
        /// Creates the indicator on a map.
        /// </summary>
        /// <returns></returns>
        private UIElement CreateIndicator()
        {
            return new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.Red),
                Width = 15,
                Height = 15
            };
        }

        private void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            try
            {
                Map mapControl = this.OverheadMap.Map;
                if (mapControl != null)
                {
                    if (_mapRoute != null) mapControl.RemoveRoute(_mapRoute);
                    _mapRoute = new MapRoute(e.Result);
                    mapControl.AddRoute(_mapRoute);
                    //routeInfo.Text = "Distance: " + e.Result.LengthInMeters + "\nTime: " + e.Result.EstimatedDuration;
                    //routeInfo.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception)
            {
            }
        }

		private void startGART_Click(object sender, RoutedEventArgs e)
		{
			Services.NavigationService.Instance.Navigate(Services.PageNames.TestPageViewName);
		}

        private async void TestSpeech_OnClick(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                await RecognizeSpeech();
                /*
                _synthesizer = new SpeechSynthesizer();
                _recognizer = new SpeechRecognizer();
                await _synthesizer.SpeakTextAsync("Command now!");
                var recoResult = await _recognizer.RecognizeAsync();
                if (recoResult.TextConfidence < SpeechRecognitionConfidence.Medium)
                {
                    // If the confidence level of the speech recognition attempt is low, 
                    // ask the user to try again.
                    MessageBox.Show("Nije prepoznato", "Error", MessageBoxButton.OK);
                    await _synthesizer.SpeakTextAsync("Not sure what you said, please try again");
                    InitMainPage();
                }
                else
                {
                    // Output that the color of the rectangle is changing by updating
                    // the TextBox control and by using text-to-speech (TTS). 
                    MessageBox.Show(recoResult.Text, "Excellent", MessageBoxButton.OK);
                    await _synthesizer.SpeakTextAsync(recoResult.Text);
                }*/
            }
            else
                MessageBox.Show("Please connect to internet to use speech recognition", "No network", MessageBoxButton.OKCancel);
            
        }
    }
}