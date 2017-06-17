using Xamarin.Forms;

namespace Saferide.Controls
{
    public class CustomFontBoldLabel : Label
    {
        public CustomFontBoldLabel()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    FontFamily = "OpenSans-CondBold.ttf#OpenSans-CondBold";
                    break;
            }
        }
    }
}
