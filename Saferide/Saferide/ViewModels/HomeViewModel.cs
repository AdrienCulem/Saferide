using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Models;
using Xamarin.Forms;
using XLabs.Platform.Services.Geolocation;

namespace Saferide.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand GetPosition { get; set; }
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
                            XFToast.ShortMessage("Tu viens de signaler un incident!");
                            break;
                        case "Error":
                            XFToast.ShortMessage("Oups, une erreur est survenue");
                            break;
                    }
                });
            }
        }

        private IGeolocator _geolocator;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private CancellationTokenSource _cancelSource;
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

        bool _isBusy;

        public bool IsBusy
        {
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _isBusy;
            }
        }

        public string PositionStatus
        {
            get
            {
                return _positionStatus;
            }
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
            get
            {
                return _positionLatitude;
            }
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
            get
            {
                return _positionLongitude;
            }
            set
            {
                if (_positionLongitude != value)
                {
                    _positionLongitude = value;
                    RaisePropertyChanged();
                }
            }
        }

        private IGeolocator Geolocator
        {
            get
            {
                if (_geolocator == null)
                {
                    _geolocator = DependencyService.Get<IGeolocator>(); /*?? Resolver.Resolve<IGeolocator>();*/
                    _geolocator.PositionError += OnListeningError;
                    _geolocator.PositionChanged += OnPositionChanged;
                }
                return _geolocator;
            }
        }

        public HomeViewModel()
        {
            GetPosition = new Command(GetGpsInfos);
        }
        public async void GetGpsInfos()
        {
            _cancelSource = new CancellationTokenSource();

            PositionStatus = String.Empty;
            PositionLatitude = String.Empty;
            PositionLongitude = String.Empty;
            IsBusy = true;
            await Geolocator.GetPositionAsync(timeout: 10000, cancelToken: _cancelSource.Token, includeHeading: true)
                .ContinueWith(t =>
                {
                    IsBusy = false;
                    if (t.IsFaulted)
                        PositionStatus = ((GeolocationException)t.Exception.InnerException).Error.ToString();
                    else if (t.IsCanceled)
                        PositionStatus = "Canceled";
                    else
                    {
                        PositionStatus = t.Result.Timestamp.ToString("G");
                        PositionLatitude = "La: " + t.Result.Latitude.ToString("N4");
                        UserPosition.Latitude = Convert.ToDouble(t.Result.Latitude.ToString());
                        UserPosition.RadLat = PositionConverter.ConvertDegreesToRadians(UserPosition.Latitude);
                        PositionLongitude = "Lo: " + t.Result.Longitude.ToString("N4");
                        UserPosition.Longitude = Convert.ToDouble(t.Result.Longitude.ToString());
                        UserPosition.RadLon = PositionConverter.ConvertDegreesToRadians(UserPosition.Longitude);
                    }

                }, _scheduler);
            if (!Geolocator.IsListening)
            {
                Geolocator.StartListening(1000, 10);
            }
        }

        private void OnListeningError(object sender, PositionErrorEventArgs e)
        {
            ////			BeginInvokeOnMainThread (() => {
            ////				ListenStatus.Text = e.Error.ToString();
            ////			});
        }

        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            PositionStatus = e.Position.Timestamp.ToString("G");
            PositionLatitude = "La: " + e.Position.Latitude.ToString("N4");
            UserPosition.Latitude = Convert.ToDouble(e.Position.Latitude.ToString());
            UserPosition.RadLat = PositionConverter.ConvertDegreesToRadians(UserPosition.Latitude);
            PositionLongitude = "Lo: " + e.Position.Longitude.ToString("N4");
            UserPosition.Longitude = Convert.ToDouble(e.Position.Longitude.ToString());
            UserPosition.RadLon = PositionConverter.ConvertDegreesToRadians(UserPosition.Longitude);
        }
    }
}
