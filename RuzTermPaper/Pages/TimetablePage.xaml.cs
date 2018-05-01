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
        public TimetablePage() => this.InitializeComponent();

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime lastDate = StaticData.Lessons.Last().Key.AddDays(1);
            foreach(var l in await StaticData.CurrentUser.GetLessonsAsync(lastDate, 7))
            {
                StaticData.Lessons.Add(l);
            }
        }
    }
}
