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
        public object Convert(object value, Type targetType, object parameter, string language) =>
            Equals(0, (int)value) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }

}
