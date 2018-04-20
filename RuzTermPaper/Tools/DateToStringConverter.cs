using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Tools
{
    public class DateToStringConverter : Windows.UI.Xaml.Data.IValueConverter
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
}
