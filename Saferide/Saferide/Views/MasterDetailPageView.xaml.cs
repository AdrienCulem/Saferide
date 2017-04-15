using Saferide.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{

    public partial class MasterDetailPageView : MasterDetailPage
    {
        public ViewCell Stack;
        public MasterDetailPageView()
        {
            InitializeComponent();
            BindingContext = new MasterDetailViewModel();
            Detail = new NavigationPage(new HomePageView());
        }

        private void Cell_OnTapped(object sender, EventArgs e)
        {
            if (Stack != null)
            {
                Stack.View.BackgroundColor = Color.Transparent;
            }
            Stack = (ViewCell)sender;
            var mpi = (MasterPageItem)Stack.BindingContext;
            Stack.View.BackgroundColor = Color.White;
        }
    }

}
