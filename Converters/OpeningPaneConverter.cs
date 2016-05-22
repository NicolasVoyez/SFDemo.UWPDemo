using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SFDemo.UWPDemo.Converters
{
    public class OpeningPaneConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return IsPaneAlwaysOpen || (bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public bool IsPaneAlwaysOpen
        {
            get { return (bool)GetValue(IsPaneAlwaysOpenProperty); }
            set { SetValue(IsPaneAlwaysOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPaneAlwaysOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPaneAlwaysOpenProperty =
            DependencyProperty.Register("IsPaneAlwaysOpen", typeof(bool), typeof(OpeningPaneConverter), new PropertyMetadata(false));


    }
}
