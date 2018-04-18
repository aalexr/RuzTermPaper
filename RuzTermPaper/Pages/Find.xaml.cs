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
        private List<Group> groups;
        private List<Lecturer> lecturers;

        public Find()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            groups = await Group.FindGroupAsync();
            lecturers = await Lecturer.FindLecturerAsync();
            base.OnNavigatedTo(e);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            search.PlaceholderText = "Поиск";
            await Dialog.ShowAsync();
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

                switch (sender.Tag)
                {
                    case "lecturers":
                        sender.ItemsSource = lecturers?.Where(x => x.fio.Contains(sender.Text, System.StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                    case "groups":
                        sender.ItemsSource = groups?.Where(x => x.number.Contains(sender.Text, System.StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                }
            }
        }

        private void search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
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

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }
    }
}
