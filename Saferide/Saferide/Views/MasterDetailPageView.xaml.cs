using Saferide.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{

    public partial class MasterDetailPageView : MasterDetailPage
    {
        public MasterDetailPageView()
        {
            InitializeComponent();
            BindingContext = new MasterDetailViewModel();
            Detail = new NavigationPage(new HomePageView());
        }
    }

}
