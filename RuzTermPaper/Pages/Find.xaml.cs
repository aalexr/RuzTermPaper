using System;
using System.Linq;
using Windows.Storage.AccessCache;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using RuzTermPaper.Models;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Find : Page
    {
        private Frame _contentFrame;

        public Find()
        {
            InitializeComponent();
            RecentListView.ItemsSource = StaticData.Recent;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _contentFrame = e.Parameter as Frame;
            base.OnNavigatedTo(e);
        }


        private async void search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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
                    sender.ItemsSource = await Lecturer.FindLecturerAsync(sender.Text);
                }
                else
                {
                    if (GroupRB.IsChecked == true)
                    {
                        sender.ItemsSource = await Group.FindGroupAsync(sender.Text);
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

                Uri uri = receiver.BuildUri(today, today.AddDays(7));

                StaticData.Lessons = (await Lesson.GetLessons(uri)).GroupBy(x => x.DateOfNest).OrderBy(x => x.Key);
                _contentFrame.Navigate(typeof(TimetablePage));
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) => sender.Text = args.SelectedItem.ToString();

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) => await addDialog.ShowAsync();

        private void RB_Checked(object sender, RoutedEventArgs e) => search.Text = string.Empty;

        private async void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Receiver receiver)
            {
                StaticData.Lessons =
                    (await Lesson.GetLessons(receiver.BuildUri(DateTime.Now, DateTime.Now.AddDays(7))))
                    .GroupBy(x => x.DateOfNest).OrderBy(x => x.Key);
                _contentFrame.Navigate(typeof(TimetablePage));
            }
        }

        private async void EmailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == VirtualKey.Enter)
            {
                Student student;
                try
                {
                    student = new Student(textBox.Text);
                }
                catch (ArgumentException)
                {
                    return;
                }
                StaticData.Recent.Add(student);
                StaticData.Lessons =
                    (await Lesson.GetLessons(student.BuildUri(DateTime.Now, DateTime.Now.AddDays(7))))
                    .GroupBy(x => x.DateOfNest).OrderBy(x => x.Key);
                _contentFrame.Navigate(typeof(TimetablePage));
            }
        }
    }
}
