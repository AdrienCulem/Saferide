﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Ressources;
using Saferide.Views;
using Xamarin.Forms;

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
            TextToSpeech.Talk(AppTexts.GiveANewIncident);
            IncidentButton = new Command<string>(NewIncident);
            ListenMicrophone = new Command(() =>
            {
                OnStartListening();
            });
            MessagingCenter.Unsubscribe<ISpeechRecognized, string>(this, "Incident");
            MessagingCenter.Subscribe<ISpeechRecognized, string>(this, "Incident", (sender, arg) =>
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
            OnStartListening();
        }

        public async void OnStartListening(bool shouldStop = true)
        {
            if (!Constants.KeywordOn)
            {
                try
                {
                    if (Constants.Listening)
                    {
                        await DependencyService.Get<ISpeechService>().StopListening();
                    }
                    DependencyService.Get<ISpeechService>().StartListening("keyword");
                    IsListenning = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else if(shouldStop)
            {
                await DependencyService.Get<ISpeechService>().StopListening();
                IsListenning = false;
            }
        }

        public async void NewIncident(string key)
        {
            var isSure = false;
            if (UserPosition.Latitude != 0 && UserPosition.Longitude != 0)
            {
                await DependencyService.Get<ISpeechService>().StopListening();
                String result;
                var promptResult = String.Empty;
                while (!isSure)
                {
                    promptResult = await DependencyService.Get<ISpeechRecognition>().Listen();
                    if (promptResult == String.Empty) return;
                    TextToSpeech.Talk(AppTexts.ConfirmDescription + promptResult);
                    await Task.Delay(2000);
                    var tempResult = await XFToast.ConfirmAsync(AppTexts.ConfirmDescription, promptResult, AppTexts.Yes, AppTexts.No);
                    if (tempResult) isSure = true;
                }
                DependencyService.Get<ISpeechService>().StartListening("keyword");
                IsListenning = true;
                if (promptResult == String.Empty)return;
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