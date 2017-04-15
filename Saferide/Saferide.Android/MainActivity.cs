using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;
using Plugin.Permissions;
using Saferide.Droid;
using Saferide.Interfaces;
using Xamarin.Forms;
using XLabs.Platform.Services.Geolocation;
[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]

namespace Saferide.Droid
{
    [Activity(Label = "Saferide", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISpeechRecognition
    {
        private readonly int VOICE = 10;
        private static string _textRecognized;
        private static bool _activityResult;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            LoadApplication(new App());
            Xamarin.Forms.DependencyService.Register<IGeolocator, Geolocator>();
            UserDialogs.Init(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task<string> Listen()
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            ((Activity)Forms.Context).StartActivityForResult(voiceIntent, VOICE);
            _activityResult = false;
            await WaitForActivityResult();
            return _textRecognized;
        }

        public async Task WaitForActivityResult()
        {
            await Task.Run(() => { while (!_activityResult) { } });
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultVal == Result.Ok)
                {
                    _textRecognized = "";
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = _textRecognized + matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        _textRecognized = textInput;
                    }
                    else
                        _textRecognized = "No speech was recognised";
                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
            _activityResult = true;
        }
    }
}

