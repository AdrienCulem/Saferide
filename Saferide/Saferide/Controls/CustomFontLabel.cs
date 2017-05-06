using Xamarin.Forms;

namespace Saferide.Controls
{
    public class CustomFontLabel : Label
    {
        public CustomFontLabel()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    FontFamily = "OpenSans-CondLight.ttf#OpenSans-CondLight";
                    break;
            }
        }
    }
}
