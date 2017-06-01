using Saferide.Models;
using Saferide.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Ressources;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel
    {
        public List<MasterPageItem> MenuList { get; set; }
        public ICommand LogOff => new Command(() =>
        {
            Constants.LogOff();
            Application.Current.MainPage = new NavigationPage(new LoginPageView());
        });
        private MasterPageItem _itemSelected;
        private string _versionNumber;
        private string _connectedUser;

        public string VersionNumber
        {
            get => _versionNumber;
            set
            {
                if (_versionNumber == value) return;
                _versionNumber = value;
                RaisePropertyChanged();
            }
        }

        public string ConnectedUser
        {
            get => _connectedUser;
            set
            {
                if (_connectedUser == value) return;
                _connectedUser = value;
                RaisePropertyChanged();
            }
        }

        public MasterPageItem ItemSelected
        {
            set
            {
                if (_itemSelected == value) return;
                _itemSelected = value;

                var page = ItemSelected.TargetType;

                var mainpage = Application.Current.MainPage as MasterDetailPage;
                if (mainpage == null)
                {
                    return;
                }
                if (page == typeof(MapPageView))
                {
                    if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
                    {
                        DependencyService.Get<ISpeechRecognition>().Talk(AppTexts.StartRidingFirst);
                        XFToast.LongMessage(AppTexts.StartRidingFirst);
                    }
                    else
                    {
                        mainpage.Detail = new NavigationPage(new MapPageView());
                    }
                }
                else
                {
                    mainpage.Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                }
                mainpage.IsPresented = false;
                RaisePropertyChanged();
            }
            get => _itemSelected;
        }

        public MasterDetailViewModel()
        {
            VersionNumber = "Version : " + DependencyService.Get<IGetVersion>().GetVersion();
            ConnectedUser = Constants.Username;
            MenuList = new List<MasterPageItem>();

            var page1 = new MasterPageItem() { Title = AppTexts.Home, Icon = "home.png", TargetType = typeof(HomePageView) };
            var page2 = new MasterPageItem() { Title = AppTexts.Map, Icon = "map.png", TargetType = typeof(MapPageView) };
            var page3 = new MasterPageItem() { Title = AppTexts.Help, Icon = "help.png", TargetType = typeof(HelpPageView) };
            var page4 = new MasterPageItem() { Title = AppTexts.Settings, Icon = "settings.png", TargetType = typeof(SettingsPageView) };


            MenuList.Add(page1);
            MenuList.Add(page2);
            MenuList.Add(page3);
            MenuList.Add(page4);
        }
    }
}