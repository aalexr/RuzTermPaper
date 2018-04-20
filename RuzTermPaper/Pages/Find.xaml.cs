using RuzTermPaper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Find : Page
    {
        private Frame contentFrame;

        public Find()
        {
            this.InitializeComponent();
            this.recentListView.ItemsSource = StaticData.Recent;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.contentFrame = e.Parameter as Frame;
            base.OnNavigatedTo(e);
        }


        private void search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    sender.ItemsSource = null;
                    return;
                }

                if (LecturerRB.IsChecked == true)
                {
                    sender.ItemsSource = StaticData.Lecturers?.Where(x => x.fio.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    if (GroupRB.IsChecked == true)
                    {
                        sender.ItemsSource = StaticData.Groups?.Where(x => x.number.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
            }
        }

        private async void search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is Receiver receiver)
            {
                addDialog.Hide();
                DateTime today = DateTime.Today;

                if (!StaticData.Recent.Contains(receiver))
                    StaticData.Recent.Add(receiver);

                Uri URI = receiver.BuildUri(today, today.AddDays(7));

                StaticData.Lessons = (await Lesson.GetLessons(URI)).GroupBy(x => x.DateOfNest).OrderBy(x => x.Key);
                contentFrame.Navigate(typeof(TimetablePage));
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) => sender.Text = args.SelectedItem.ToString();

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) => await addDialog.ShowAsync();

        private void RB_Checked(object sender, RoutedEventArgs e) => search.Text = string.Empty;

    }
}
