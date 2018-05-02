using RuzTermPaper.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FindPage : Page
    {
        private NavigationView navigationView;
        private SingletonData data;

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

                sender.ItemsSource = data.Users.Where(x => x.Name.Contains(sender.Text, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!(args.ChosenSuggestion is Models.User user))
                return;

            AddDialog.Hide();
            var today = DateTime.Today;

            if (!data.Recent.Contains(user))
                data.Recent.Add(user);
            await UpdateLessons(user, today);
            navigationView.SelectedItem = navigationView.MenuItems[0];
        }

        private async Task UpdateLessons(Models.User user, DateTime today)
        {
            data.CurrentUser = user;
            data.Lessons.Clear();
            try
            {
                foreach (var item in await user.GetLessonsAsync(today, 7))
                {
                    data.Lessons.Add(item);
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Content = ex.Message,
                    Title = "Ошибка соединения"
                };
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Content = ex.Message,
                    Title = "Ошибка"
                };
                await dialog.ShowAsync();
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
            sender.Text = args.SelectedItem.ToString();


        private async void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Models.User user))
                return;

            navigationView.SelectedItem = navigationView.MenuItems[0];
            await UpdateLessons(user, DateTime.Today);
        }

        private async void EmailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!(sender is TextBox textBox) || e.Key != VirtualKey.Enter || !textBox.Text.EndsWith("@edu.hse.ru"))
                return;

            var student = new Student(textBox.Text);

            if (!data.Recent.Contains(student))
                data.Recent.Add(student);

            navigationView.SelectedItem = navigationView.MenuItems[0];
            await UpdateLessons(student, DateTime.Today);
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
                    Content = "Ошибка при обращении к серверу. Проверьте соединение или попробуйте позднее"
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
