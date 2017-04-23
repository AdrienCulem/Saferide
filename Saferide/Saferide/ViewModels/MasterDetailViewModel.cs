using Saferide.Models;
using Saferide.Views;
using System;
using System.Collections.Generic;
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

        private MasterPageItem _itemSelected;
        private string _versionNumber;

        public string VersionNumber
        {
            get { return _versionNumber; }
            set
            {
                if (_versionNumber != value)
                {
                    _versionNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MasterPageItem ItemSelected
        {
            set
            {
                if (_itemSelected != value)
                {
                    _itemSelected = value;

                    var page = ItemSelected.TargetType;

                    var mainpage = Application.Current.MainPage as MasterDetailPage;

                    if (page == typeof(LoginPageView))
                    {
                        Application.Current.MainPage = new NavigationPage(new LoginPageView());
                    }else if (page == typeof(IncidentsPageView))
                    {
                        if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
                        {
                            DependencyService.Get<ISpeechRecognition>().Talk("U need to start locating first");
                            XFToast.LongMessage("U need to start locating your phone first");
                        }
                        else
                        {
                            mainpage.Detail = new NavigationPage(new IncidentsPageView());
                        }
                    }
                    else
                    {
                        if (mainpage != null)
                        {
                            mainpage.Detail = new NavigationPage((Page) Activator.CreateInstance(page));
                        }
                    }
                    if (mainpage != null)
                    {
                        mainpage.IsPresented = false;
                    }
                    RaisePropertyChanged();
                }
            }
            get { return _itemSelected; }
        }


        public MasterDetailViewModel()
        {
            VersionNumber = "Version : " + DependencyService.Get<IGetVersion>().GetVersion();
            MenuList = new List<MasterPageItem>();

            var page1 = new MasterPageItem() {Title = AppTexts.Home, Icon = "homeIcon.png", TargetType = typeof(HomePageView)};
            var page2 = new MasterPageItem() {Title = AppTexts.Incidents, Icon = "incidentIcon.png", TargetType = typeof(IncidentsPageView)};
            var page3 = new MasterPageItem() {Title = AppTexts.Logoff, Icon = "logoutIcon.png", TargetType = typeof(LoginPageView)};


            MenuList.Add(page1);
            MenuList.Add(page2);
            MenuList.Add(page3);
        }
    }
}