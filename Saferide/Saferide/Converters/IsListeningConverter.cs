using System;
using System.Globalization;
using Xamarin.Forms;

namespace Saferide.Converters
{
    public class IsListeningConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isListening = (bool)value;
            return isListening ? Color.FromHex("#B45050") : Color.FromHex("#46B482");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
