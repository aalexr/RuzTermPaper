﻿using RuzTermPaper.Models;
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
            Uri URI = null;
            DateTime today = DateTime.Today;

            switch (e.Parameter)
            {
                case Group G:
                    URI = Lesson.Build(G, today, today.AddDays(7));
                    goto default;

                case Lecturer L:
                    URI = Lesson.Build(L, today, today.AddDays(7));
                    goto default;

                case string S:
                    if (string.IsNullOrEmpty(S))
                        break;

                    URI = Lesson.Build(S, today, today.AddDays(7));

                    goto default;

                default:
                    try
                    {
                        if (URI != null)
                        {
                            timetableCVS.Source = (from L in await Lesson.GetTimetable(URI)
                                                   group L by L.DateOfNest
                                                   into LL orderby LL.Key
                                                   select LL);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                    break;
            }

            base.OnNavigatedTo(e);
        }

        public TimetablePage()
        {
            this.InitializeComponent();
        }
    }
}
