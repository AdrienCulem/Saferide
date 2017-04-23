using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenericWebPageView : ContentPage
    {
        public GenericWebPageView(string url)
        {
            InitializeComponent();
            GenericWebView.Source = url;
        }

        protected override bool OnBackButtonPressed()
        {
            if (!Constants.IsConnected)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageView());
                return true;
            }
            Application.Current.MainPage = new MasterDetailPageView();
            return true;
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private void GenericWebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            XFToast.ShowLoading();
        }

        private void GenericWebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            XFToast.HideLoading();
        }
    }
}
