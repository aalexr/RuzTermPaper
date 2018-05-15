using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace RuzTermPaper.Tools
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (DateTime)value;
            var today = DateTime.Today;
            if (val == today)
                return "Today".Localize();
            if (val == today.AddDays(1))
                return "Tomorrow".Localize();
            return val.ToString("dddd, d MMMM");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
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

    public class ZeroToBoolCOonverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => Equals(0, (int)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }


}
