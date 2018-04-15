namespace RuzTermPaper.Tools
{
    public class DateToStringConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            return ((System.DateTime)value).ToString("dddd, d MMMM");
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            return System.DateTime.Parse((string)value);
        }
    }
}
