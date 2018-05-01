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
    public sealed partial class Find : Page
    {
        private NavigationView navigationView;

        public Find()
        {
            InitializeComponent();
            Flyout.Items[0].Tag = Models.UserType.Group;
            Flyout.Items[1].Tag = Models.UserType.Lecturer;
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

                sender.ItemsSource = StaticData.Users.Where(x => x.Name.Contains(sender.Text, StringComparison.CurrentCultureIgnoreCase));
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

                StaticData.Lessons = await user.GetLessonsAsync(today, 21);
                navigationView.SelectedItem = navigationView.MenuItems[0];
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
            sender.Text = args.SelectedItem.ToString();


        private async void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Models.User user)
            {
                StaticData.Lessons = await user.GetLessonsAsync(DateTime.Today, 7);

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
                        StaticData.Lessons = await student.GetLessonsAsync(DateTime.Today, 7);
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

        private async void AddMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuFlyoutItem item))
                return;

            try
            {
                //ToDo Change name
                await Do((Models.UserType)item.Tag);
            }
            catch (HttpRequestException ex)
            {
                //ToDo Show useful info
            }

        }

        private async Task Do(Models.UserType type)
        {
            if (!(AddDialog.Content is AutoSuggestBox suggestBox))
                return;

            if (type == Models.UserType.Group)
            {
                suggestBox.Header = "Найдите расписание группы по ее номеру";
                suggestBox.PlaceholderText = "Начните вводить группу";
                suggestBox.ItemsSource = StaticData.Users = await Group.FindAsync();
            }
            else
            {
                if (type == Models.UserType.Lecturer)
                {
                    suggestBox.Header = "Найдите расписание преподавателя";
                    suggestBox.PlaceholderText = "Введите имя преподавателя";
                    suggestBox.ItemsSource = StaticData.Users = await Lecturer.FindAsync();
                }
            }

            await AddDialog.ShowAsync();
        }

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuFlyoutItem item))
                return;

            StaticData.Recent.Remove((Models.User)item.DataContext);
        }
    }
}
