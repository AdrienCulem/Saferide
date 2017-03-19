using Saferide.Interfaces;
using Xamarin.Forms;

namespace Saferide.Helpers
{
    public static class XFToast
    {
        public static void ShortMessage(string message)
        {
            DependencyService.Get<IMessage>().ShortAlert(message);
        }

        public static void LongMessage(string message)
        {
            DependencyService.Get<IMessage>().LongAlert(message);
        }

        public static void ShortErrorMessage()
        {
            DependencyService.Get<IMessage>().ShortAlert("Une erreur est survenue... Vérifiez votre connexion internet");
        }
    }
}