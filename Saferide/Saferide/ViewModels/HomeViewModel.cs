using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MoreLinq;
using Plugin.Geolocator;
using Saferide.Extensions;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Ressources;
using Saferide.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Saferide.Models.Position;

namespace Saferide.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand StartRiding { get; set; }
        public ICommand StopRiding { get; set; }
        public ICommand ListenMicrophone { get; set; }
        public ICommand IncidentButton => new Command(async () =>
        {
            await GoToNewIncident();
        });

        private readonly Geocoder _geoCoder;
        private string _speechResult;
        private static int _whenToUpdateIncidents;
        private string _positionStatus = string.Empty;
        private string _positionLatitude = string.Empty;
        private string _positionLongitude = string.Empty;
        private string _positionHeading = string.Empty;
        private string _positionAddress = string.Empty;
        private string _positionSpeed;
        private bool _isStoped;
        private bool _isStarted;
        private string _metricSystemToShow;

        public string PositionStatus
        {
            get => _positionStatus;
            set
            {
                if (_positionStatus == value) return;
                _positionStatus = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the position latitude.
        /// </summary>
        /// <value>The position latitude.</value>
        public string PositionLatitude
        {
            get => _positionLatitude;
            set
            {
                if (_positionLatitude == value) return;
                _positionLatitude = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the position longitude.
        /// </summary>
        /// <value>The position longitude.</value>
        public string PositionLongitude
        {
            get => _positionLongitude;
            set
            {
                if (_positionLongitude == value) return;
                _positionLongitude = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Where the user is heading (Relative to the north)
        /// </summary>
        public string PositionHeading
        {
            get => _positionHeading;
            set
            {
                if (_positionHeading == value) return;
                _positionHeading = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The speed of the user in km/h
        /// </summary>
        public string PositionSpeed
        {
            get => _positionSpeed;
            set
            {
                if (_positionSpeed == value) return;
                _positionSpeed = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// The adress of the user (Current location)
        /// </summary>
        public string PositionAddress
        {
            get => _positionAddress;
            set
            {
                if (_positionAddress == value) return;
                _positionAddress = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The speed sytem to display on the screen
        /// </summary>
        public string MetricSystemToShow
        {
            get => _metricSystemToShow;
            set
            {
                if (_metricSystemToShow == value) return;
                _metricSystemToShow = value;
                RaisePropertyChanged();
            }
        }


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
        public bool IsStoped
        {
            get => _isStoped;
            set
            {
                if (_isStoped == value) return;
                _isStoped = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Used to display the right button
        /// </summary>
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                if (_isStarted == value) return;
                _isStarted = value;
                RaisePropertyChanged();
            }
        }

        public HomeViewModel()
        {
            DependencyService.Get<IAskPermissions>().AskPermissions();
            if (!CrossGeolocator.Current.IsListening)
            {
                GetGpsInfos();
            }
            ListenMicrophone = new Command(async () =>
            {
                await VoiceRecognition();
            });
            StartRiding = new Command(async () =>
            {
                await GetGpsInfos();
            });
            _geoCoder = new Geocoder();
            PositionHeading = "N";
            PositionSpeed = "0";
            MetricSystemToShow = Constants.MetricSystem.ToString();
        }
        /// <summary>
        /// Starts the voice recognition
        /// </summary>
        /// <returns></returns>
        public async Task VoiceRecognition()
        {
            var result = await DependencyService.Get<ISpeechRecognition>().Listen();
            SpeechResult = result;
            var listenNewIncidentResults = AppTexts.ListenNewIncident.Spintax();
            if (listenNewIncidentResults.Contains(result))
                await GoToNewIncident();
            if (result == AppTexts.ListenStartRiding)
                await GetGpsInfos();
            if (result == AppTexts.ListenStopRiding)
            {
                await CrossGeolocator.Current.StopListeningAsync();
                IsStoped = true;
                IsStarted = false;
            }
        }

        /// <summary>
        /// Getting the user's gps informations and starts listening for changes
        /// </summary>
        public async Task GetGpsInfos(bool shouldStartListening = true)
        {
            DependencyService.Get<IAskPermissions>().AskPermissions();
            try
            {
                if (!DependencyService.Get<IGpsEnabled>().IsGpsEnabled())
                {
                    var textToSay = AppTexts.EnableLocation;
                    XFToast.LongMessage(textToSay);
                    TextToSpeech.Talk(textToSay);
                    IsStoped = true;
                    return;
                }
                XFToast.ShowLoading();
                var position = await CrossGeolocator.Current.GetPositionAsync(15000, null, true);
                if (position == null)
                    return;
                UserPosition.Latitude = position.Latitude;
                PositionLatitude = Math.Round(UserPosition.Latitude, 2).ToString();
                UserPosition.Longitude = position.Longitude;
                PositionLongitude = Math.Round(UserPosition.Longitude, 2).ToString();
                UserPosition.Heading = position.Heading;
                PositionHeading = PositionHelper.ConvertHeadingToDirection(position.Heading);
                UserPosition.Speed = position.Speed;
                if (Constants.MetricSystem == Constants.MetricSystems.Kmh)
                {
                    PositionSpeed = Math.Round(position.Speed * 3.6).ToString();
                }
                else if (Constants.MetricSystem == Constants.MetricSystems.Mph)
                {
                    PositionSpeed = Math.Round(position.Speed * 2.23694).ToString();
                }
                try
                {
                    var revposition = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                    var addresses = await _geoCoder.GetAddressesForPositionAsync(revposition);
                    var fullAddress = addresses.FirstOrDefault();
                    var address = Regex.Match(fullAddress, @"^[^0-9]*").Value;
                    UserPosition.Address = address;
                    PositionAddress = address.Replace("\n", " ");
                }
                catch (Exception ex)
                {
                    XFToast.HideLoading();
                    Debug.WriteLine("Unable to get address: " + ex);
                }
                if (shouldStartListening == false)
                {
                    XFToast.HideLoading();
                    return;
                }
                await GetIncidents();
                await StartListening();
                XFToast.HideLoading();
            }
            catch (Exception ex)
            {
                XFToast.HideLoading();
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
        }

        /// <summary>
        /// Starts listening for changes
        /// </summary>
        /// <returns></returns>
        public async Task StartListening()
        {
            if (!CrossGeolocator.Current.IsListening)
            {
                CrossGeolocator.Current.AllowsBackgroundUpdates = true;
                //Change distance
                await CrossGeolocator.Current.StartListeningAsync(3000, 20, true);
                IsStoped = false;
                IsStarted = true;
            }
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        /// <summary>
        /// Reaction to the positionChanged event
        /// </summary>
        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var position = e.Position;
                UserPosition.Latitude = position.Latitude;
                PositionLatitude = Math.Round(UserPosition.Latitude, 2).ToString();
                UserPosition.Longitude = position.Longitude;
                PositionLongitude = Math.Round(UserPosition.Longitude, 2).ToString();
                UserPosition.Heading = position.Heading;
                PositionHeading = PositionHelper.ConvertHeadingToDirection(position.Heading); ;
                UserPosition.Speed = position.Speed;
                if (Constants.MetricSystem == Constants.MetricSystems.Kmh)
                {
                    PositionSpeed = Math.Round(position.Speed * 3.6).ToString();
                }
                else if (Constants.MetricSystem == Constants.MetricSystems.Mph)
                {
                    PositionSpeed = Math.Round(position.Speed * 2.23694).ToString();
                }
                try
                {
                    var revposition = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                    var addresses = await _geoCoder.GetAddressesForPositionAsync(revposition);
                    var fullAddress = addresses.FirstOrDefault();
                    var address = Regex.Match(fullAddress, @"^[^0-9]*").Value;
                    UserPosition.Address = address;
                    PositionAddress = address.Replace("\n", " ");
                }
                catch (Exception ex)
                {
                    XFToast.HideLoading();
                    Debug.WriteLine("Unable to get address: " + ex);
                }
                if (_whenToUpdateIncidents > 20)
                {
                    await GetIncidents();
                    await WarnIncident();
                    _whenToUpdateIncidents = 0;
                }
                else
                {
                    await WarnIncident();
                    _whenToUpdateIncidents++;
                }
            });
        }

        /// <summary>
        /// Warns an incident if there is one to
        /// </summary>
        private async Task WarnIncident()
        {
            if (Constants.IsBeingAskedToConfirm)
                return;
            Position pos = new Position
            {
                Latitude = UserPosition.Latitude,
                Longitude = UserPosition.Longitude,
            };
            PositionConverter pConvert = new PositionConverter(pos);
            var result = pConvert.BoundingCoordinates(1.5);
            List<Incident> incidentsWithinRadius;
            if (pos.Latitude == 0 || pos.Longitude == 0)
            {
                return;
            }
            //Select all incident within a 1.5 kilometers radius around current position
            var minLat = result[0].Latitude;
            var minLong = result[0].Longitude;
            var maxLat = result[1].Latitude;
            var maxLong = result[1].Longitude;
            if (result[0].Longitude <= result[1].Longitude)
            {
                incidentsWithinRadius = Constants.NearestIncidents.Where(
                    i => (i.Latitude >= minLat) &&
                         (i.Latitude <= maxLat) &&
                         (i.Longitude >= minLong) &&
                         (i.Longitude <= maxLong)).ToList();
            }
            else
            {
                incidentsWithinRadius = Constants.NearestIncidents.Where(
                    i => (i.Latitude >= minLat) &&
                         (i.Latitude <= maxLat) &&
                         (i.Longitude >= minLong) ||
                         (i.Longitude <= maxLong)).ToList();
            }
            if (incidentsWithinRadius.Count != 0)
            {
                foreach (var item in incidentsWithinRadius)
                {
                    //Current position
                    var sCoord = new Position()
                    {
                        Latitude = UserPosition.Latitude,
                        Longitude = UserPosition.Longitude
                    };
                    //Incident to calculate
                    var eCoord = new Position()
                    {
                        Latitude = item.Latitude,
                        Longitude = item.Longitude
                    };
                    //Radius of the earth
                    item.DistanceFromCurrentPosition =  PositionHelper.DistanceBetweenPoints(eCoord, sCoord);
                    //direction to take from current position 
                    item.DirectionFromCurrentPosition = PositionHelper.DirectionFromPosition(eCoord, sCoord);
                }
                //Closest incident in the direction of the user
                //Only works if the user is moving
                if (Math.Abs(UserPosition.Speed) > 0)
                {
                    //All incidents in front of the user in a certain radius
                    var incidentsInDirection = incidentsWithinRadius
                        .Where(a => a.DirectionFromCurrentPosition > UserPosition.Heading - 45 &&
                                    a.DirectionFromCurrentPosition < UserPosition.Heading + 45)
                        .ToList();
                    if (incidentsInDirection.Count != 0)
                    {
                        //Closest incident in the direction
                        var closestIncident = incidentsInDirection.
                            MinBy(a => a.DistanceFromCurrentPosition);
                        //If the incident is in the same street
                        if (closestIncident.Street == UserPosition.Address)
                        {
                            double distanceBetweenTheIncident;
                            string unit;
                            if (closestIncident.DistanceFromCurrentPosition < 0.4)
                            {
                                distanceBetweenTheIncident =
                                    (Math.Round(closestIncident.DistanceFromCurrentPosition, 3)) * 1000;
                                unit = AppTexts.Meters;
                                ResourceManager rm = AppTexts.ResourceManager;
                                //Don't signal an already confirmed incident
                                if (closestIncident.Confirmed)
                                    return;
                                string typeOfIncident = rm.GetString(closestIncident.IncidentType.ToUpperFirstLetter());
                                TextToSpeech.Talk(String.Format(AppTexts.SignalIncident, typeOfIncident,
                                    distanceBetweenTheIncident, unit, closestIncident.Description));
                                Constants.IsBeingAskedToConfirm = true;
                                await Task.Delay(5000);
                                //Prompts the user to confirm the incident
                                var resultOfConfirmation = await DependencyService.Get<ISpeechRecognition>().Listen();
                                bool isConfirmed;
                                if (resultOfConfirmation == AppTexts.YesListen)
                                {
                                    isConfirmed = true;
                                }
                                else if (resultOfConfirmation == AppTexts.NoListen)
                                {
                                    isConfirmed = false;
                                }
                                else
                                {
                                    isConfirmed = await XFToast.ConfirmAsync(AppTexts.Confirm, AppTexts.ConfirmText,
                                        AppTexts.Yes,
                                        AppTexts.No);
                                }
                                //Setting the incident to confirmed
                                closestIncident.Confirmed = isConfirmed;
                                Constants.IsBeingAskedToConfirm = false;
                                //Sending the confirmation to the API
                                await App.IncidentManager.ConfirmIncident(closestIncident);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Redirection to the page used to signal a new incident
        /// </summary>
        /// <returns></returns>
        public async Task GoToNewIncident()
        {
            if (DependencyService.Get<IGpsEnabled>().IsGpsEnabled())
            {
                if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
                {
                    await GetGpsInfos(false);
                }
                var mainpage = Application.Current.MainPage as MasterDetailPage;
                if (mainpage != null)
                {
                    await mainpage.Detail.Navigation.PushAsync(new NewIncidentPageView());
                }
            }
            else
            {
                var textToSay = AppTexts.EnableLocation;
                XFToast.ShowCustomError(textToSay);
                TextToSpeech.Talk(textToSay);
            }

        }

        /// <summary>
        /// Getting the incidents from the API
        /// </summary>
        /// <returns></returns>
        private async Task GetIncidents()
        {
            if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
            {
                TextToSpeech.Talk(AppTexts.StartRidingFirst);
            }
            Position pos = new Position
            {
                Latitude = UserPosition.Latitude,
                Longitude = UserPosition.Longitude,
            };
            Constants.NearestIncidents = await App.IncidentManager.GetIncidents(pos);
        }
    }
}