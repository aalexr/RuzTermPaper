using RuzTermPaper.Models;
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
    public sealed partial class SearchPage : Page
    {
        private MainPage page;
        private List<string> EmailSg { get; set; } = new List<string>();
        private List<Group> groups;
        private List<Lecturer> lecturers;
        public SearchPage()
        {
            this.InitializeComponent();
            groupsListView.ItemsSource = StaticData.Groups;
            lecturersListView.ItemsSource = StaticData.Lecturers;
            studentListView.ItemsSource = StaticData.StudentEmails;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            page = e.Parameter as MainPage;
            groups = await Group.FindGroupAsync();
            lecturers = await Lecturer.FindLecturerAsync();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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
                        sender.ItemsSource = lecturers?.Where(x => x.fio.Contains(sender.Text,  System.StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                    case "groups":
                        sender.ItemsSource = groups?.Where(x => x.number.Contains(sender.Text, System.StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                }
            }

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) => sender.Text = args.SelectedItem.ToString();

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

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            switch (args.ChosenSuggestion)
            {
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

                case null:
                    break;
            }
        }
    }
}
