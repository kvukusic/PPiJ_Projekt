using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.VoiceCommands;

namespace Hoover.Services
{
	public class SpeechRecognitionService
	{
		SpeechSynthesizer _synthesizer;
		SpeechRecognizer _recognizer;
		SpeechRecognizerUI speechRecognizerUI = new SpeechRecognizerUI();

		private async Task RecognizeSpeech()
		{
			speechRecognizerUI.Settings.ListenText = "Say your command...";
			speechRecognizerUI.Settings.ExampleText = "Start Stop";
			speechRecognizerUI.Settings.ReadoutEnabled = true;
			speechRecognizerUI.Settings.ShowConfirmation = false;
			SpeechRecognitionUIResult recognitionResult = await speechRecognizerUI.RecognizeWithUIAsync();
		}

		public async void TestSpeech()
		{
			if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
			{
				_synthesizer = new SpeechSynthesizer();
				_recognizer = new SpeechRecognizer();
				await _synthesizer.SpeakTextAsync("Command now!");
				var recoResult = await _recognizer.RecognizeAsync();
				if (recoResult.TextConfidence < SpeechRecognitionConfidence.Medium)
				{
					// If the confidence level of the speech recognition attempt is low, 
					// ask the user to try again.
					MessageBox.Show("Command isn't recognised.", "Error", MessageBoxButton.OK);
					await _synthesizer.SpeakTextAsync("Not sure what you said, please try again.");
				}
				else
				{
					// Output that the color of the rectangle is changing by updating
					// the TextBox control and by using text-to-speech (TTS). 
					MessageBox.Show(recoResult.Text, "Success", MessageBoxButton.OK);
					await _synthesizer.SpeakTextAsync(recoResult.Text);
				}
			}
			else
			{
				MessageBox.Show("Please connect to internet", "No network", MessageBoxButton.OKCancel);
			}
		}

	}
}
