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
			catch (Exception ex)
			{
				HandleSpeechSynthesisError(ex);
			}
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
