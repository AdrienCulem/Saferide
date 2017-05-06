using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Saferide.Interfaces;
using Saferide.Ressources;
using Xamarin.Forms;

namespace Saferide.Helpers
{
    public static class XFToast
    {
        /// <summary>
        /// Short message of 2 seconds
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void ShortMessage(string message)
        {
            TimeSpan time = new TimeSpan(0, 0, 2);
            UserDialogs.Instance.Toast(message, time);
        }
        /// <summary>
        /// Long message of 4 seconds
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void LongMessage(string message)
        {
            TimeSpan time = new TimeSpan(0, 0, 4);
            UserDialogs.Instance.Toast(message, time);
        }

        /// <summary>
        /// Short error message saying "Something went wrong check your internet connection
        /// </summary>
        public static void ShortErrorMessage()
        {
            UserDialogs.Instance.Toast(AppTexts.InternetError);
        }

        /// <summary>
        /// Modal loading with "Wait a second" text
        /// </summary>
        public static void ShowLoading()
        {
            UserDialogs.Instance.ShowLoading(AppTexts.WaitASec, MaskType.Gradient);
        }

        /// <summary>
        /// Hide the modal loading
        /// </summary>
        public static void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }

        /// <summary>
        /// Done message with "Done" text
        /// </summary>
        public static void ShowSuccess()
        {
            UserDialogs.Instance.ShowSuccess(AppTexts.Done);
        }

        /// <summary>
        /// Modal message saying "something went wrong"
        /// </summary>
        public static void ShowError()
        {
            UserDialogs.Instance.ShowError(AppTexts.Oups);
        }

        /// <summary>
        /// Custom error with the message in parameters
        /// </summary>
        /// <param name="mess">The message to display</param>
        public static void ShowCustomError(string mess)
        {
            UserDialogs.Instance.ShowError(mess);
        }

        public static async Task<PromptResult> PromptAsync(string title, string yes, string no, string placeholder)
        {
            return await UserDialogs.Instance.PromptAsync(null, title, yes, no, placeholder);
        }

        public static async Task<string> ActionSheet(string title, string no, params string[] options)
        {
            return await UserDialogs.Instance.ActionSheetAsync(title, no, null, null, options);
        }
    }
}