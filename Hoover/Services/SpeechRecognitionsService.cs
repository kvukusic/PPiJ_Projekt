#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.VoiceCommands;

#endregion

namespace Hoover.Services
{
	public class SpeechRecognitionService
	{
		private SpeechSynthesizer _synthesizer;
		private SpeechRecognizer _recognizer;
	    private SpeechRecognizerUI _speechRecognizerUI;

        /// <summary>
        /// Constructor.
        /// </summary>
	    public SpeechRecognitionService()
	    {
	        _synthesizer = new SpeechSynthesizer();
            _recognizer = new SpeechRecognizer();
            _speechRecognizerUI = new SpeechRecognizerUI();
	    }

        /// <summary>
        /// Plays the given string as audio.
        /// </summary>
        /// <param name="textToSpeech"></param>
		public async void SpeakText(string textToSpeech)
		{
		    if (textToSpeech != null)
		    {
                await _synthesizer.SpeakTextAsync(textToSpeech);
		    }
		}

        /// <summary>
        /// Listens to the ambient sound and returns the recognized string.
        /// </summary>
        /// <returns></returns>
		public async Task<string> RecognizeSpeech()
		{
		    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
		    {
                return await Task.Run(async () =>
                {
                    try
                    {
                        _speechRecognizerUI.Settings.ListenText = "Say your command...";
                        _speechRecognizerUI.Settings.ExampleText = "Example: Start/Stop";
                        _speechRecognizerUI.Settings.ReadoutEnabled = false;
                        _speechRecognizerUI.Settings.ShowConfirmation = false;
                        SpeechRecognitionUIResult recognitionResult;
                        do
                        {
                            recognitionResult = await _speechRecognizerUI.RecognizeWithUIAsync();
                        }
                        while (recognitionResult.RecognitionResult.TextConfidence < SpeechRecognitionConfidence.Medium);

                        return recognitionResult.RecognitionResult.Text;
                    }
                    catch (Exception ex)
                    {
                        HandleSpeechSynthesisError(ex);
                        return null;
                    }

                });
		    }

		    return null;
		}

        #region Exceptions

        private void HandleSpeechSynthesisError(Exception ex)
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


            if ((uint) ex.HResult == AbortedCallHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_SYSTEM_CALL_INTERRUPTED", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == GenericInternalHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_INTERNAL_ERROR", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == AlreadyInLexiconHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_ALREADY_IN_LEX", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == NotInLexiconHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_NOT_IN_LEX", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == UnsupportedPhonemeHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_UNSUPPORTED_PHONEME", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == PhonemeConversionHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_PHONEME_CONVERSION", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == InvalidLexiconHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_LEX_INVALID_DATA", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == UnsupportedLanguageHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_UNSUPPORTED_LANG", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == StringTooLongHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_STRING_TOO_LONG", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == StringEmptyHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_STRING_EMPTY", MessageBoxButton.OK);
            }
            else if ((uint) ex.HResult == NoMoreItemsHResult)
            {
                MessageBox.Show(ex.Message, "SPERR_WINRT_NO_MORE_ITEMS", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK);
            }
        }

        #endregion

    }
}
