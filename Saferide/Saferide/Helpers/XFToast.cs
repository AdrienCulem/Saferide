using System;
using Acr.UserDialogs;
using Saferide.Interfaces;
using Xamarin.Forms;

namespace Saferide.Helpers
{
    public static class XFToast
    {
        public static void ShortMessage(string message)
        {
            TimeSpan time = new TimeSpan(0, 0, 2);
            UserDialogs.Instance.Toast(message, time);
        }

        public static void LongMessage(string message)
        {
            TimeSpan time = new TimeSpan(0, 0, 1);
            UserDialogs.Instance.Toast(message, time);
        }

        public static void ShortErrorMessage()
        {
            UserDialogs.Instance.Toast("Une erreur est survenue... Vérifiez votre connexion internet");
        }
    }
}