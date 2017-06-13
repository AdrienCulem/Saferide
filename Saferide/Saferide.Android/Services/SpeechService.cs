using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Java.IO;
using Saferide.Droid.Services;
using Saferide.Droid.SpeechToText;
using Saferide.Interfaces;
using Saferide.ViewModels;
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

        private static string KWS_SEARCH = "newIncident";
        private static string KWS_SEARCH_FILE = "NewIncidentList";

        private string KEYPHRASE = "new incident";

        private static File assetsDir;

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
                if (mode == "keyphrase")
                {
                    SwitchSearch(KWS_SEARCH, KEYPHRASE);
                    Constants.KeyphraseOn = true;
                }
                else if (mode == "keyword")
                {
                    _recognizer.AddKeywordSearch(KWS_SEARCH_FILE, new File(assetsDir, "up_en.txt"));
                    _recognizer.StartListening(KWS_SEARCH_FILE);
                    Constants.KeywordOn = true;

                }
            }
        }

        public async Task StopListening()
        {
            await _recognizer.Stop();
            Constants.KeywordOn = false;
            Constants.KeyphraseOn = false;
        }

        public async Task Setup()
        {
            Assets assets = new Assets(Forms.Context);
            assetsDir = await assets.syncAssets();
            SetupRecognizer();
        }

        private void SetupRecognizer()
        {
            Config config = Decoder.DefaultConfig();

            _recognizer = new SpeechRecognizerSetup(config)
                .SetAcousticModel(new File(assetsDir, "en-us-ptm"))
                .SetDictionary(new File(assetsDir, "cmudict-en-us.dict"))
                .setKeywordThreshold(float.Parse("1e-20"))
                //.SetRawLogDir(assetsDir) // To disable logging of raw audio comment out this call (takes a lot of space on the device)
                .GetRecognizer();

            _recognizer.Result += Recognizer_Result;
            _recognizer.InSpeechChange += Recognizer_InSpeechChange;
            _recognizer.Timeout += Recognizer_Timeout;
            _recognizer.Stopped += Recognizer_Stopped;

            // Create keyword-activation search.
            _recognizer.AddKeyphraseSearch(KWS_SEARCH, KEYPHRASE);
        }

        private void Recognizer_Stopped(object sender, EventArgs e)
        {
        }

        private void Recognizer_Timeout(object sender, EventArgs e)
        {
        }

        private void Recognizer_InSpeechChange(object sender, bool e)
        {
        }

        private void Recognizer_Result(object sender, SpeechResultEvent e)
        {
            if (e.Hypothesis != null && e.Hypothesis.Hypstr.Any() && (n != e.Hypothesis.Hypstr.Count())) // e.Hypothesis.Hypstr expands when words are detected
            {
                lastHypo = e.Hypothesis.Hypstr.Substring(0, e.Hypothesis.Hypstr.Count() - n); // get the last word detected (the first one in Hypstr)
                n = e.Hypothesis.Hypstr.Count();
                if (lastHypo == "new incident")
                {
                    MessagingCenter.Send<ISpeechRecognized>(this, "NewIncident");
                }
                else
                {
                    MessagingCenter.Send<ISpeechRecognized, string>(this, "Incident", lastHypo);
                }
                lastHypo = null;
            }
        }

        private void SwitchSearch(String searchName, string keyPhrase)
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
        }
    }
}