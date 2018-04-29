using System;
using System.Linq;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using RuzTermPaper.Models;
using System.Net.Http;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Find : Page
    {
        private NavigationView navigationView;

        public Find() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e) => navigationView = e.Parameter as NavigationView;

        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    sender.ItemsSource = null;
                    return;
                }
            }
        }

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is Models.User user)
            {
                AddDialog.Hide();
                DateTime today = DateTime.Today;

                if (!StaticData.Recent.Contains(user))
                    StaticData.Recent.Add(user);

                StaticData.Lessons = await user.GetLessonsAsync(today, today.AddDays(7));
                navigationView.SelectedItem = navigationView.MenuItems[0];
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
            sender.Text = (string)args.SelectedItem;


        private async void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Models.User user)
            {
                StaticData.Lessons = await user.GetLessonsAsync(DateTime.Now, DateTime.Now.AddDays(7));

                navigationView.SelectedItem = navigationView.MenuItems[0];
            }
        }

        private async void EmailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == VirtualKey.Enter)
            {
                if (textBox.Text.EndsWith("@edu.hse.ru"))
                {
                    Student student = new Student(textBox.Text);
                    if (!StaticData.Recent.Contains(student))
                        StaticData.Recent.Add(student);

                    try
                    {
                        StaticData.Lessons = await student.GetLessonsAsync(DateTime.Now, DateTime.Now.AddDays(7));
                    }
                    catch (HttpRequestException ex)
                    {
                        ContentDialog dialog = new ContentDialog { PrimaryButtonText = "OK", Content = ex.Message, Title = "Ошибка соединения" };
                        await dialog.ShowAsync();
                        return;
                    }
                    catch (Exception ex)
                    {
                        ContentDialog dialog = new ContentDialog { PrimaryButtonText = "OK", Content = ex.Message, Title = "Error" };
                        await dialog.ShowAsync();
                        return;
                    }

                    navigationView.SelectedItem = navigationView.MenuItems[0];
                }
                else
                {

                }
            }
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuFlyoutItem)sender).Tag)
            {
                case "Group":
                    await AddDialog.ShowAsync();
                    break;

                case "Lecturer":
                    await AddDialog.ShowAsync();
                    break;

            }
        }
    }
}
