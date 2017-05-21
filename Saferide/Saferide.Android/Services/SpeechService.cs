﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Java.IO;
using Saferide.Droid.Services;
using Saferide.Droid.SpeechToText;
using Saferide.Interfaces;
using SphinxBase;
using Xamarin.Forms;
using Decoder = PocketSphinx.Decoder;

[assembly: Dependency(typeof(SpeechService))] // Communication PCL -> specific plateform
namespace Saferide.Droid.Services
{
    public class SpeechService : ISpeechService, ISpeechRecognized
    {
        private SpeechRecognizer _recognizer;

        private int n;
        private string lastHypo = "";

        private static string KWS_SEARCH = "wakeup";
        private static string KWS_SEARCH_FILE = "wakeupList";

        private string KEYPHRASE = "new incident";

        private static File assetsDir;

        public SpeechService()
        {

        }

        public void ChangeKeyPhrase(string keyPhrase)
        {
            KEYPHRASE = keyPhrase;
        }

        public void StartListening(string mode)
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Output an alert
                var alert = new AlertDialog.Builder(Forms.Context);
                alert.SetTitle("You don't seem to have a microphone to record with.");
                alert.SetPositiveButton("OK", (sender, e) => { return; });

                alert.Show();
            }
            else
            {
                n = 0;
                if(mode == "keyphrase")
                {
                    switchSearch(KWS_SEARCH, KEYPHRASE);
                }else if (mode == "keyword")
                {
                    _recognizer.AddKeywordSearch(KWS_SEARCH_FILE, new File(assetsDir, "up_en.txt"));
                    _recognizer.StartListening(KWS_SEARCH_FILE);
                }
            }
        }

        public async Task StopListening()
        {
            await _recognizer.Stop();
            //MainPage.ViewModel.IsInSpeech = false;
            //MainPage.ViewModel.IsListening = false;
            //MainPage.ViewModel.IsStartEnabled = true;
            //MainPage.ViewModel.IsStopEnabled = false;
        }

        public async Task Setup()
        {
            Assets assets = new Assets(Forms.Context);
            assetsDir = await assets.syncAssets();
            await SetupRecognizer();
        }

        private async Task SetupRecognizer()
        {
            Config config = Decoder.DefaultConfig();

            _recognizer = new SpeechRecognizerSetup(config)
                .SetAcousticModel(new File(assetsDir, "en-us-ptm"))
                .SetDictionary(new File(assetsDir, "cmudict-en-us.dict"))
                //.setKeywordThreshold(float.Parse("1e-1"))
                //.SetRawLogDir(assetsDir) // To disable logging of raw audio comment out this call (takes a lot of space on the device)
                .GetRecognizer();

            _recognizer.Result += Recognizer_Result;
            _recognizer.InSpeechChange += Recognizer_InSpeechChange;
            _recognizer.Timeout += Recognizer_Timeout;
            _recognizer.Stopped += _recognizer_Stopped;

            // Create keyword-activation search.
            _recognizer.AddKeyphraseSearch(KWS_SEARCH, KEYPHRASE);
        }

        private void _recognizer_Stopped(object sender, EventArgs e)
        {
            //MainPage.ViewModel.IsListening = false;
            //StartListening();
        }

        private void Recognizer_Timeout(object sender, EventArgs e)
        {
            //MainPage.ViewModel.IsListening = false;
        }

        private void Recognizer_InSpeechChange(object sender, bool e)
        {
            //MainPage.ViewModel.IsInSpeech = e;
        }

        private void Recognizer_Result(object sender, SpeechResultEvent e)
        {
            bool isFinalResult = e.FinalResult;
            if (e.Hypothesis != null && e.Hypothesis.Hypstr.Count() > 0 && (n != e.Hypothesis.Hypstr.Count())) // e.Hypothesis.Hypstr expands when words are detected
            {
                lastHypo = e.Hypothesis.Hypstr.Substring(0, e.Hypothesis.Hypstr.Count() - n); // get the last word detected (the first one in Hypstr)
                n = e.Hypothesis.Hypstr.Count();
                //Still to be worked on
                MessagingCenter.Send<ISpeechRecognized, string>(this, "Recognized", lastHypo);
            }
        }

        private void switchSearch(String searchName, string keyPhrase)
        {
            if (!String.IsNullOrEmpty(keyPhrase))
            {
                _recognizer.AddKeyphraseSearch(searchName, keyPhrase);
            }

            // If we are not spotting, start listening with timeout (10000 ms or 10 seconds).
            if (searchName.Equals(KWS_SEARCH))
                _recognizer.StartListening(searchName);
            else
                _recognizer.StartListening(searchName, 10000);

            //MainPage.ViewModel.IsListening = true;
        }
    }
}