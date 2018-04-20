using RuzTermPaper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Receiver> Recent { get; set; } = new ObservableCollection<Receiver>();

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
                    sender.ItemsSource = lecturers?.Where(x => x.fio.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    if (GroupRB.IsChecked == true)
                    {
                        sender.ItemsSource = groups?.Where(x => x.number.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
            }
        }

        private void search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            switch (args.ChosenSuggestion)
            {
                case Group G:
                    if (!Recent.Contains(G))
                        Recent.Add(G);
                    //(Window.Current.Content as Frame).Navigate(typeof(MainPage), G);
                    break;

                case Lecturer L:
                    if (!Recent.Contains(L))
                        Recent.Add(L);
                    //(Window.Current.Content as Frame).Navigate(typeof(MainPage), L);
                    break;

                case null:
                    break;
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) => await addDialog.ShowAsync();

        private void RB_Checked(object sender, RoutedEventArgs e) => search.Text = string.Empty;
    }
}
