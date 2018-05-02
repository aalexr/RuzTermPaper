using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TimetablePage : Page
    {
        private SingletonData data;
        public TimetablePage()
        {
            InitializeComponent();
            data = SingletonData.Initialize();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime lastDate = data.Lessons.Last().Key.AddDays(1);
            foreach(var l in await data.CurrentUser.GetLessonsAsync(lastDate, 7))
            {
                data.Lessons.Add(l);
            }
        }
    }
}
