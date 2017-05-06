using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saferide.GPS;
using Saferide.Models;
using Saferide.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPageView : ContentPage
    {
        Geocoder geoCoder;
        public MapPageView()
        {
            InitializeComponent();
            BindingContext = new MapViewModel();
            geoCoder = new Geocoder();
            MessagingCenter.Unsubscribe<MapViewModel>(this, "AddPins");
            MessagingCenter.Subscribe<MapViewModel, List<Incident>>(this, "AddPins", (sender,  args) =>
            {
                 AddPins(args);
            });
        }

        private void  AddPins(List<Incident> incidents)
        {
            foreach (var item in incidents)
            {
                var point = new Xamarin.Forms.Maps.Position(item.Latitude, item.Longitude);
                MyMap.Pins.Add(new Pin
                {
                    Label = item.IncidentType,
                    Position = point,
                    Type = PinType.Generic
                });
            }
        }

        private async void OnGoToClicked(object sender, EventArgs e)
        {
            var item = (await geoCoder.GetPositionsForAddressAsync(EntryLocation.Text)).FirstOrDefault();
            if (item == null)
            {
                await DisplayAlert("Error", "Unable to decode position", "OK");
                return;
            }

            var zoomLevel = 16; // between 1 and 18
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            MyMap.MoveToRegion(new MapSpan(item, latlongdegrees, latlongdegrees));
        }

        private void OnSliderChanged(object sender, ValueChangedEventArgs e)
        {
            var zoomLevel = e.NewValue; // between 1 and 18
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            MyMap.MoveToRegion(new MapSpan(MyMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var position = new Xamarin.Forms.Maps.Position(UserPosition.Latitude, UserPosition.Longitude);
            var zoomLevel = 16; // between 1 and 18
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            MyMap.MoveToRegion(new MapSpan(position, latlongdegrees, latlongdegrees));
        }
    }
}
