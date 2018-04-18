using RuzTermPaper.Models;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TimetablePage : Page
    {
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter as string))
            {
                DateTime today = DateTime.Today;
                Uri URI = Lesson.BuildUri(e.Parameter, today, today.AddDays(7));
                //timetableCVS.Source = (from L in await Lesson.GetTimetable(URI)
                //                       group L by L.DateOfNest into LL
                //                       orderby LL.Key
                //                       select LL);

                timetableCVS.Source = (await Lesson.GetLessons(URI)).GroupBy(x => x.DateOfNest).OrderBy(x => x.Key);
            }
            base.OnNavigatedTo(e);
        }

        public TimetablePage()
        {
            this.InitializeComponent();
        }
    }
}
