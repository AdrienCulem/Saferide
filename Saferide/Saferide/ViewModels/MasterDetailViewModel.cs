using Saferide.Models;
using Saferide.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel
    {
        public List<MasterPageItem> MenuList { get; set; }

        private MasterPageItem _itemSelected;

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
                        Application.Current.MainPage = new NavigationPage(new StartPageView());
                    }
                    else
                    {
                        mainpage.Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                    }

                    mainpage.IsPresented = false;

                    RaisePropertyChanged();
                }
            }
            get
            {
                return _itemSelected;
            }
        }


        public MasterDetailViewModel()
        {

            MenuList = new List<MasterPageItem>();

            var page1 = new MasterPageItem() { Title = "Accueil", Icon = "home_icon.png", TargetType = typeof(HomePageView) };
            var page3 = new MasterPageItem() { Title = "Se déconnecter", Icon = "logoff_black.png", TargetType = typeof(LoginPageView) };

            MenuList.Add(page1);
            MenuList.Add(page3);
        }
    }
}