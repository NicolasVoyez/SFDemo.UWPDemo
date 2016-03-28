using System;
using Windows.UI.Xaml.Data;

namespace SFDemo.UWPDemo.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleanValue = value as bool?;
            if (booleanValue.HasValue)
            {
                return booleanValue.Value ? "Oui" : "Non";
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
