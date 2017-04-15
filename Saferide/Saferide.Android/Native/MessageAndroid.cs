using Android.Widget;
using Android.App;
using CyberHelp.Mobile.Droid;
using CyberHelp.Mobile.Droid.Native;
using Saferide.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace CyberHelp.Mobile.Droid.Native
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}