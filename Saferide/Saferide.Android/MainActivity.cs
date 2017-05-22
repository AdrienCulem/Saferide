using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Speech;
using Android.Speech.Tts;   
using Plugin.Permissions;
using Saferide.Droid;
using Saferide.Interfaces;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]

namespace Saferide.Droid
{
    [Activity(Label = "Saferide", Icon = "@drawable/ic_launcher", ScreenOrientation = ScreenOrientation.Portrait,  Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISpeechRecognition, TextToSpeech.IOnInitListener, IGpsEnabled, IGetVersion, IAskPermissions
    {
        private readonly int VOICE = 10;
        private static string _textRecognized;
        private static bool _activityResult;
        private static TextToSpeech _textToSpeech;
        private Java.Util.Locale _lang;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            UserDialogs.Init(this);
            //var langAvailable = new List<string>();
            //var localesAvailable = Java.Util.Locale.GetAvailableLocales().ToList();
            //foreach (var locale in localesAvailable)
            //{
            //    var res = _textToSpeech.IsLanguageAvailable(locale);
            //    switch (res)
            //    {
            //        case LanguageAvailableResult.Available:
            //            langAvailable.Add(locale.DisplayLanguage);
            //            break;
            //        case LanguageAvailableResult.CountryAvailable:
            //            langAvailable.Add(locale.DisplayLanguage);
            //            break;
            //        case LanguageAvailableResult.CountryVarAvailable:
            //            langAvailable.Add(locale.DisplayLanguage);
            //            break;
            //    }
            //}
            
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task<string> Listen()
        {
            _textRecognized = "";
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

        public void Talk(string textToSay)
        {
            if (_textToSpeech == null)
            {
                _textToSpeech = new TextToSpeech(Forms.Context, this);
                _textToSpeech.SetLanguage(_lang);
                _textToSpeech.SetPitch(0.8f);
                _textToSpeech.SetSpeechRate(1);
            }
            _textToSpeech.Speak(textToSay, QueueMode.Flush, null, null);
        }

        void TextToSpeech.IOnInitListener.OnInit(OperationResult status)
        {
            // if we get an error, default to the default language
            if (status == OperationResult.Error)
                _textToSpeech.SetLanguage(Java.Util.Locale.Default);
            // if the listener is ok, set the lang
            if (status == OperationResult.Success)
                _textToSpeech.SetLanguage(_lang);
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

        public bool IsGpsEnabled()
        {
            var context = Forms.Context;
            var locMgr = context.GetSystemService(LocationService) as LocationManager;
            string provider = LocationManager.GpsProvider;
            if (locMgr == null)
            {
                return false;
            }
            return locMgr.IsProviderEnabled(provider);
        }

        public string GetVersion()
        {
            var version =
                Forms.Context.PackageManager.GetPackageInfo(Forms.Context.ApplicationContext.PackageName, 0).VersionName;
            return version;
        }

        public void AskPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GetPermissionAsync();
            }
        }

        public void GetPermissionAsync()
        {
            string[] PermissionsLocation =
            {
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.RecordAudio,
                Manifest.Permission.Bluetooth,
                Manifest.Permission.BluetoothAdmin
            };
            var activity = (Activity)Forms.Context;
            var view = activity.FindViewById(Android.Resource.Id.Content);
            //const string permission = Manifest.Permission.WriteExternalStorage;
            foreach (var item in PermissionsLocation)
            {
                if (Forms.Context.CheckSelfPermission(item) != Permission.Granted)
                {
                    activity.RequestPermissions(PermissionsLocation, 0);

                }
            }
        }
    }
}

