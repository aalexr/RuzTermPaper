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
            string uri;
            Uri URI;

            switch (e.Parameter)
            {
                case Group G:
                    StaticData.Lessons.Clear();
                    URI = Lesson.Build(G, DateTime.Today, DateTime.Today.AddDays(7));
                    //URI = Lesson.ConstructRequest(IdType.groupOid, G.groupOid.ToString(), DateTime.Today, DateTime.Today.AddDays(7));
                    //uri = $"{Lesson.baseUri}personlessons?fromdate={DateTime.Today.ToString("yyyy.MM.dd")}&todate={DateTime.Today.AddDays(7).ToString("yyyy.MM.dd")}&receivertype=3&groupOid={G.groupOid}";
                    try
                    {
                        foreach (var p in await Lesson.GetTimetable(URI.ToString()))
                            StaticData.Lessons.Add(p);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                    goto default;

                case Lecturer L:
                    StaticData.Lessons.Clear();
                    uri = $"{Lesson.baseUri}personlessons?fromdate={DateTime.Today.ToString("yyyy.MM.dd")}&todate={DateTime.Today.AddDays(7).ToString("yyyy.MM.dd")}&receivertype=1&lecturerOid={L.lecturerOid}";
                    try
                    {
                        foreach (var p in await Lesson.GetTimetable(uri))
                            StaticData.Lessons.Add(p);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                    goto default;

                case string S:
                    StaticData.Lessons.Clear();
                    if (string.IsNullOrEmpty(S))
                        break;

                    uri = $"{Lesson.baseUri}personlessons?fromdate={DateTime.Today.ToString("yyyy.MM.dd")}&todate={DateTime.Today.AddDays(7).ToString("yyyy.MM.dd")}&receivertype=0&&email={S}";
                    try
                    {
                        foreach (var p in await Lesson.GetTimetable(uri))
                            StaticData.Lessons.Add(p);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                    goto default;

                default:
                    break;
            }

            base.OnNavigatedTo(e);
        }

        public TimetablePage()
        {
            this.InitializeComponent();
            this.timetable.ItemsSource = StaticData.Lessons;
        }
    }
}
