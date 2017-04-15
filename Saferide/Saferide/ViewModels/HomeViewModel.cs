using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Geolocator;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Ressources;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using XLabs.Platform.Services.Geolocation;

namespace Saferide.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand GetPosition { get; set; }
        public ICommand ListenMicrophone { get; set; }

        public ICommand IncidentButton
        {
            get
            {
                return new Command<string>(async (key) =>
                {
                    IsBusy = true;
                    if (UserPosition.Latitude == 0 && UserPosition.Longitude == 0)
                    {
                        GetGpsInfos();
                    }
                    Incident incident = new Incident
                    {
                        Latitude = UserPosition.Latitude,
                        Longitude = UserPosition.Longitude,
                        IncidentType = key
                    };
                    var result = await App.IncidentManager.NewIncident(incident);
                    IsBusy = false;
                    switch (result)
                    {
                        case "Success":
                            XFToast.ShortMessage(AppTexts.SendAnIncident);
                            break;
                        case "Error":
                            XFToast.ShortMessage(AppTexts.Oups);
                            break;
                    }
                });
            }
        }

        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// The position status
        /// </summary>
        private string _positionStatus = string.Empty;

        /// <summary>
        /// The position latitude
        /// </summary>
        private string _positionLatitude = string.Empty;

        /// <summary>
        /// The position longitude
        /// </summary>
        private string _positionLongitude = string.Empty;

        /// <summary>
        /// The position heading
        /// </summary>
        private string _positionHeading = string.Empty;

        /// <summary>
        /// The address of the user
        /// </summary>
        private string _positionAddress = string.Empty;


        /// <summary>
        /// The speed
        /// </summary>
        private string _positionSpeed;

        public string PositionStatus
        {
            get { return _positionStatus; }
            set
            {
                if (_positionStatus != value)
                {
                    _positionStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the position latitude.
        /// </summary>
        /// <value>The position latitude.</value>
        public string PositionLatitude
        {
            get { return _positionLatitude; }
            set
            {
                if (_positionLatitude != value)
                {
                    _positionLatitude = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the position longitude.
        /// </summary>
        /// <value>The position longitude.</value>
        public string PositionLongitude
        {
            get { return _positionLongitude; }
            set
            {
                if (_positionLongitude != value)
                {
                    _positionLongitude = value;
                    RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Where the user is heading
        /// </summary>
        public string PositionHeading
        {
            get { return _positionHeading; }
            set
            {
                if (_positionHeading != value)
                {
                    _positionHeading = value;
                    RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// The speed of the user
        /// </summary>
        public string PositionSpeed
        {
            get { return _positionSpeed; }
            set
            {
                if (_positionSpeed != value)
                {
                    _positionSpeed = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The speed of the user
        /// </summary>
        public string PositionAddress
        {
            get { return _positionAddress; }
            set
            {
                if (_positionAddress != value)
                {
                    _positionAddress = value;
                    RaisePropertyChanged();
                }
            }
        }

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

        private Geocoder _geoCoder;
        private string _speechResult;

        public HomeViewModel()
        {
            GetPosition = new Command(GetGpsInfos);
            ListenMicrophone = new Command(async () =>
            {
                //SpeechResult = "I'm listening";
                var result = await DependencyService.Get<ISpeechRecognition>().Listen();
                SpeechResult = result;
            });
            _geoCoder = new Geocoder();
        }
        /// <summary>
        /// Getting the user's gps informations and starts listening for changes
        /// </summary>
        public async void GetGpsInfos()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                IsBusy = true;
                var position = await locator.GetPositionAsync(15000);
                if (position == null)
                    return;

                UserPosition.Latitude = position.Latitude;
                PositionLatitude = Math.Round(UserPosition.Latitude, 2).ToString();
                UserPosition.Longitude = position.Longitude;
                PositionLongitude = Math.Round(UserPosition.Longitude, 2).ToString();
                UserPosition.Heading = position.Heading;
                PositionHeading = Math.Round(position.Heading, 2).ToString();
                UserPosition.Speed = position.Speed;
                PositionSpeed = Math.Round(position.Speed * 3.6).ToString();
                IsBusy = false;
                try
                {
                    var revposition = new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude);
                    var addresses = await _geoCoder.GetAddressesForPositionAsync(revposition);
                    var address = addresses.FirstOrDefault();
                    PositionAddress = address.Replace("\n", " ");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get address: " + ex);
                }
                await StartListening();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
        }
        /// <summary>
        /// Starts listening for changes
        /// </summary>
        /// <returns></returns>
        async Task StartListening()
        {
            if (!CrossGeolocator.Current.IsListening)
            {
                await CrossGeolocator.Current.StartListeningAsync(15000, 10, true);
            }
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var test = e.Position;
                UserPosition.Latitude = test.Latitude;
                PositionLatitude = Math.Round(UserPosition.Latitude, 2).ToString();
                UserPosition.Longitude = test.Longitude;
                PositionLongitude = Math.Round(UserPosition.Longitude, 2).ToString();
                UserPosition.Heading = test.Heading;
                PositionHeading = Math.Round(test.Heading, 2).ToString();
                UserPosition.Speed = test.Speed;
                PositionSpeed = Math.Round(test.Speed * 3.6).ToString();
                try
                {
                    var revposition = new Xamarin.Forms.GoogleMaps.Position(test.Latitude, test.Longitude);
                    var addresses = await _geoCoder.GetAddressesForPositionAsync(revposition);
                    var address = addresses.FirstOrDefault();
                    PositionAddress = address.Replace("\n", " ");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get address: " + ex);
                }
            });
        }
    }
}