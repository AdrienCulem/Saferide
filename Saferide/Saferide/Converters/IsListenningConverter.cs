using System;
using System.Globalization;
using Xamarin.Forms;

namespace Saferide.Converters
{
    public class IsListenningConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isListening = (bool)value;
            return isListening ? Color.FromHex("#46B482") : Color.FromHex("#B45050");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
