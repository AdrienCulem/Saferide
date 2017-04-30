using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MoreLinq;
using Plugin.Geolocator;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Views;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
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

        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private Geocoder _geoCoder;
        private string _speechResult;
        private static int WhenToUpdateIncidents;
        private string _positionStatus = string.Empty;
        private string _positionLatitude = string.Empty;
        private string _positionLongitude = string.Empty;
        private string _positionHeading = string.Empty;
        private string _positionAddress = string.Empty;
        private string _positionSpeed;
        private bool _isStoped;
        private bool _isStarted;

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
        /// Where the user is heading (Relative to the north)
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
        /// The speed of the user in km/h
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
        /// The adress of the user (Current location)
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

        /// <summary>
        /// Used to display the right button
        /// </summary>
        public bool IsStoped
        {
            get { return _isStoped; }
            set
            {
                if (_isStoped != value)
                {
                    _isStoped = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Used to display the right button
        /// </summary>
        public bool IsStarted
        {
            get { return _isStarted; }
            set
            {
                if (_isStarted != value)
                {
                    _isStarted = value;
                    RaisePropertyChanged();
                }
            }
        }

        public HomeViewModel()
        {
            IsStoped = true;
            IsStarted = false;
            StartRiding = new Command(async () => { await GetGpsInfos(); });
            StopRiding = new Command(async () =>
            {
                await CrossGeolocator.Current.StopListeningAsync();
                IsStoped = true;
                IsStarted = false;
            });
            _geoCoder = new Geocoder();
        }

        /// <summary>
        /// Getting the user's gps informations and starts listening for changes
        /// </summary>
        public async Task GetGpsInfos()
        {
            try
            {
                if (!CrossGeolocator.Current.IsGeolocationEnabled)
                {
                    var textToSay = "I need you to enable the location first";
                    XFToast.LongMessage(textToSay);
                    TextToSpeech.Talk(textToSay);
                    return;
                }
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;
                XFToast.ShowLoading();
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
                XFToast.HideLoading();
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
                await GetIncidents();
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
                CrossGeolocator.Current.AllowsBackgroundUpdates = true;
                CrossGeolocator.Current.DesiredAccuracy = 1;
                await CrossGeolocator.Current.StartListeningAsync(2000, 10, true);
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
                if (WhenToUpdateIncidents > 5)
                {
                    await GetIncidents();
                    WhenToUpdateIncidents = 0;
                }
                else
                {
                    WhenToUpdateIncidents++;
                }
                WarnIncident();
            });
        }

        /// <summary>
        /// Warns an incident if there is one to
        /// </summary>
        private void WarnIncident()
        {
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
                    var sCoord = new Plugin.Geolocator.Abstractions.Position()
                    {
                        Latitude = UserPosition.Latitude,
                        Longitude = UserPosition.Longitude
                    };
                    //Incident to calculate
                    var eCoord = new Plugin.Geolocator.Abstractions.Position()
                    {
                        Latitude = item.Latitude,
                        Longitude = item.Longitude
                    };
                    //Radius of the earth
                    const int radiusoftheearth = 6371;
                    var dLat = PositionConverter.ConvertDegreesToRadians(eCoord.Latitude - sCoord.Latitude);
                    var dLon = PositionConverter.ConvertDegreesToRadians(eCoord.Longitude - sCoord.Longitude);
                    var a =
                            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                            Math.Cos(PositionConverter.ConvertDegreesToRadians(sCoord.Latitude)) * Math.Cos(PositionConverter.ConvertDegreesToRadians(eCoord.Latitude)) *
                            Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                        ;
                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    //Distance between current position and that incident
                    var distance = radiusoftheearth * c;
                    item.DistanceFromCurrentPosition = distance;
                    //direction to take from current position 
                    var directionFromCurrentPosition = Math.Atan2(dLon, dLat);
                    item.DirectionFromCurrentPosition =
                        Math.Abs(PositionConverter.ConvertRadiansToDegrees(directionFromCurrentPosition));
                }
                //Closest incident in the direction
                if (UserPosition.Heading != 0)
                {
                    var closestIncident = incidentsWithinRadius.
                        Where(a => a.DirectionFromCurrentPosition > UserPosition.Heading - 90 && a.DirectionFromCurrentPosition < UserPosition.Heading + 90).
                        MinBy(a => a.DistanceFromCurrentPosition);
                    double distanceBetweenTheIncident;
                    string unit;
                    if (closestIncident.DistanceFromCurrentPosition < 1)
                    {
                        distanceBetweenTheIncident = (Math.Round(closestIncident.DistanceFromCurrentPosition, 3)) * 100;
                        unit = "meters";
                    }
                    else
                    {
                        distanceBetweenTheIncident = Math.Round(closestIncident.DistanceFromCurrentPosition, 1);
                        unit = "kilometers";
                    }
                    TextToSpeech.Talk("There is and incident of the type" + closestIncident.IncidentType
                                      + "in" + distanceBetweenTheIncident + unit);
                }
            }
        }

        public async Task GoToNewIncident()
        {
            if (DependencyService.Get<IGpsEnabled>().IsGpsEnabled())
            {
                if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
                {
                    await GetGpsInfos();
                }
                var mainpage = Application.Current.MainPage as MasterDetailPage;
                if (mainpage != null)
                {
                    await mainpage.Detail.Navigation.PushAsync(new NewIncidentPageView());
                }
            }
            else
            {
                var textToSay = "You need to enable location first";
                XFToast.ShowCustomError(textToSay);
                TextToSpeech.Talk(textToSay);
            }

        }

        private async Task GetIncidents()
        {
            await GetIncident.GetIncidents();
        }
    }
}