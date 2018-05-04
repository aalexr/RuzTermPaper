using Windows.ApplicationModel.Resources;

namespace RuzTermPaper.Tools
{
    internal static class ResourseHelper
    {
        private static ResourceLoader _resourceLoader = new ResourceLoader();

        public static string Localize(this string key) => _resourceLoader.GetString(key);
    }
}
