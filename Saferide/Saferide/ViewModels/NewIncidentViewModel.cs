using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Geolocator;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Ressources;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Saferide.ViewModels
{
    public class NewIncidentViewModel : BaseViewModel
    {
        public ICommand GetPosition { get; set; }
        public ICommand IncidentButton { get; set; }
        public ICommand ListenMicrophone { get; set; }


        /// <summary>
        /// The result of the speech recognition
        /// </summary>
        public string SpeechResult
        {
            get { return _speechResult; }
            set
            {
                if (_speechResult != value)
                {
                    _speechResult = value;
                    RaisePropertyChanged();
                }
            }
        }


        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private Geocoder _geoCoder;
        private string _speechResult;

        public NewIncidentViewModel()
        {
            IncidentButton = new Command<string>(NewIncident);
            _geoCoder = new Geocoder();
            ListenMicrophone = new Command(async () =>
            {
                await VoiceRecognition();
            });
        }

        public async void NewIncident(string key)
        {
            if (UserPosition.Latitude != 0 && UserPosition.Longitude != 0)
            {
                String result;
                var promptResult = await XFToast.PromptAsync(AppTexts.Description, AppTexts.Done, AppTexts.Cancel, AppTexts.EnterDescription);
                if (!promptResult.Ok)
                {
                }
                else
                {
                    XFToast.ShowLoading();
                    Incident incident = new Incident
                    {
                        Latitude = UserPosition.Latitude,
                        Longitude = UserPosition.Longitude,
                        Description = promptResult.Text,
                        Street = UserPosition.Address,
                        IncidentType = key
                    };
                    result = await App.IncidentManager.NewIncident(incident);
                    XFToast.HideLoading();
                    switch (result)
                    {
                        case "Success":
                            XFToast.ShowSuccess();
                            TextToSpeech.Talk(AppTexts.SendAnIncident);
                            break;
                        case "Invalid":
                        case "Error":
                            XFToast.ShowError();
                            //XFToast.ShortMessage(AppTexts.Oups);
                            TextToSpeech.Talk(AppTexts.Oups);
                            break;
                    }
                }
            }
        }

        public async Task VoiceRecognition()
        {
            var result = await DependencyService.Get<ISpeechRecognition>().Listen();
            SpeechResult = result;
            switch (result)
            {
                case "new hole":
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("hole");
                    UserDialogs.Instance.HideLoading();
                    break;
                case "new obstacle":
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("obstacle");
                    UserDialogs.Instance.HideLoading();
                    break;
                case "new sliding zone":
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("sliding zone");
                    UserDialogs.Instance.HideLoading();
                    break;
                case "new danger":
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("danger");
                    UserDialogs.Instance.HideLoading();
                    break;
            }
        }
    }
}