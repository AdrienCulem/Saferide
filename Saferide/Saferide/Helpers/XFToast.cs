using System;
using System.Threading.Tasks;
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
            TimeSpan time = new TimeSpan(0, 0, 4);
            UserDialogs.Instance.Toast(message, time);
        }

        public static void ShortErrorMessage()
        {
            UserDialogs.Instance.Toast("Une erreur est survenue... Vérifiez votre connexion internet");
        }

        public static void ShowLoading()
        {
            UserDialogs.Instance.ShowLoading("Wait a second", MaskType.Gradient);
        }

        public static void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }

        public static void ShowSuccess()
        {
            UserDialogs.Instance.ShowSuccess("Done!");
        }

        public static void ShowError()
        {
            UserDialogs.Instance.ShowError("Oups! Didn't work");
        }

        public static async Task<PromptResult> PromptAsync(string title, string yes, string no, string placeholder)
        {
            return await UserDialogs.Instance.PromptAsync(null, title, yes, no, placeholder);
        }
    }
}