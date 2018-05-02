using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace RuzTermPaper.Tools
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((DateTime)value).ToString("dddd, d MMMM");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.Parse((string)value);
        }
    }

    public class VisibleWhenEmpty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Equals(0, (int)value))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }

}
