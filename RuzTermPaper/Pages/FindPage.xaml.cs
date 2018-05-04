using RuzTermPaper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FindPage : Page
    {
        private SingletonData data;
        private SolidColorBrush validFormat = new SolidColorBrush(Colors.Green);
        private SolidColorBrush invalidFormat = new SolidColorBrush(Colors.Red);

        public FindPage()
        {
            InitializeComponent();
            if (Flyout.Items != null)
            {
                Flyout.Items[0].Tag = Models.UserType.Group;
                Flyout.Items[1].Tag = Models.UserType.Lecturer;
            }

            data = SingletonData.Initialize();
        }

        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    sender.ItemsSource = null;
                    return;
                }

                sender.ItemsSource = data.Users.Where(x => x.Name.Contains(sender.Text, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!(args.ChosenSuggestion is Models.User user))
                return;

            AddDialog.Hide();
            var today = DateTime.Today;

            if (await data.UpdateLessons(user, today) && !data.Recent.Contains(user))
                data.Recent.Add(user);

        }

        public void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
            sender.Text = args.SelectedItem.ToString();


        private async void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Models.User user))
                return;

            await data.UpdateLessons(user, DateTime.Today);
        }

        private async void EmailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!(sender is TextBox textBox) || e.Key != VirtualKey.Enter)
                return;
            Student student;

            if (textBox.Text.EndsWith("@edu.hse.ru")
                && await data.UpdateLessons(student = new Student(textBox.Text), DateTime.Today)
                && !data.Recent.Contains(student))
                data.Recent.Add(student);
            else
                textBox.BorderBrush = invalidFormat;
        }

        private async void AddMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuFlyoutItem item))
                return;

            try
            {
                await ConfigureDialog((Models.UserType)item.Tag);
            }
            catch (HttpRequestException)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Title = "Ошибка",
                    Content = "Произошла ошибка при обращении к серверу. Проверьте соединение или попробуйте позднее."
                };
                await dialog.ShowAsync();
            }

        }

        private async Task ConfigureDialog(Models.UserType type)
        {
            if (!(AddDialog.Content is AutoSuggestBox suggestBox))
                return;

            switch (type)
            {
                case Models.UserType.Group:
                    suggestBox.Header = "Найдите расписание группы по ее номеру";
                    suggestBox.PlaceholderText = "Начните вводить группу";
                    suggestBox.ItemsSource = data.Users = await Group.FindAsync();
                    break;
                case Models.UserType.Lecturer:
                    suggestBox.Header = "Найдите расписание преподавателя";
                    suggestBox.PlaceholderText = "Введите имя преподавателя";
                    suggestBox.ItemsSource = data.Users = await Lecturer.FindAsync();
                    break;
            }

            await AddDialog.ShowAsync();
        }

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuFlyoutItem item && item.DataContext is Models.User user))
                return;

            data.Recent.Remove(user);
        }
    }
}
