using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Ressources;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

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
            get => _speechResult;
            set
            {
                if (_speechResult == value) return;
                _speechResult = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Used to display the right button
        /// </summary>
        public bool IsListenning
        {
            get => _isListenning;
            set
            {
                if (_isListenning == value) return;
                _isListenning = value;
                RaisePropertyChanged();
            }
        }

        private string _speechResult;
        private bool _isListenning;

        public NewIncidentViewModel()
        {
            IncidentButton = new Command<string>(NewIncident);
            MessagingCenter.Subscribe<ISpeechRecognized, string>(this, "Recognized", (sender, arg) =>
            {
                if (arg == AppTexts.NewHole)
                {
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("hole");
                    UserDialogs.Instance.HideLoading();
                }
                else if (arg == AppTexts.NewObstacle)
                {
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("obstacle");
                    UserDialogs.Instance.HideLoading();
                }
                else if (arg == AppTexts.NewSlidingZone)
                {
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("sliding zone");
                    UserDialogs.Instance.HideLoading();
                }
                else if (arg == AppTexts.NewDanger)
                {
                    UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, null);
                    NewIncident("danger");
                    UserDialogs.Instance.HideLoading();
                }
            });

            ListenMicrophone = new Command(OnStartListening);
        }

        public async void OnStartListening()
        {
            if (!Constants.VoiceAlreadyInit)
            {
                try
                {
                    DependencyService.Get<ISpeechService>().StartListening("keyword");
                    IsListenning = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                await DependencyService.Get<ISpeechService>().StopListening();
                IsListenning = false;
            }
        }

        public async void NewIncident(string key)
        {
            if (UserPosition.Latitude != 0 && UserPosition.Longitude != 0)
            {
                String result;
                var actionResult = await XFToast.ActionSheet(AppTexts.WhatOptionForIncident, AppTexts.Cancel, AppTexts.Write, AppTexts.Speak);
                var promptResult = String.Empty;
                if (actionResult == AppTexts.Write)
                {
                    var tempResult = await XFToast.PromptAsync(AppTexts.Description, AppTexts.Done, AppTexts.Cancel,
                        AppTexts.EnterDescription);
                    if (!tempResult.Ok) return;
                    promptResult = tempResult.Text;
                }
                else if(actionResult == AppTexts.Speak)
                {
                    promptResult = await DependencyService.Get<ISpeechRecognition>().Listen();
                }
                if(promptResult == String.Empty)return;
                XFToast.ShowLoading();
                Incident incident = new Incident
                {
                    Latitude = UserPosition.Latitude,
                    Longitude = UserPosition.Longitude,
                    Description = promptResult,
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
}