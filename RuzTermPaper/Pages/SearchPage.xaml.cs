using RuzTermPaper.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private MainPage page;
        private List<string> EmailSg { get; set; } = new List<string>();

        public SearchPage()
        {
            this.InitializeComponent();
            groupsListView.ItemsSource = StaticData.Groups;
            lecturersListView.ItemsSource = StaticData.Lecturers;
            studentListView.ItemsSource = StaticData.StudentEmails;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            page = e.Parameter as MainPage;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private async void AutoSuggestBox_TextChangedAsync(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    sender.ItemsSource = null;
                    return;
                }

                switch (sender.Tag)
                {
                    case "lecturers":
                        sender.ItemsSource = await Lesson.FindAsync<Lecturer>(sender.Text, sender.Tag.ToString().ToLower());
                        break;
                    case "groups":
                        sender.ItemsSource = await Lesson.FindGroupAsync(sender.Text);
                        break;
                }
            }

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            switch (args.SelectedItem)
            {
                case string email:
                    break;

                case Group G:
                    if (!StaticData.Groups.Contains(G))
                        StaticData.Groups.Add(G);
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage), G);
                    break;

                case Lecturer L:
                    if (!StaticData.Lecturers.Contains(L))
                        StaticData.Lecturers.Add(L);
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage), L);
                    break;
            }
        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (!(sender is TextBox text))
                return;

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (!StaticData.StudentEmails.Contains(text.Text))
                    StaticData.StudentEmails.Add(text.Text);

                (Window.Current.Content as Frame).Navigate(typeof(MainPage), text.Text);
            }
        }
    }
}
