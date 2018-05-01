﻿using RuzTermPaper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
                await UpdateLessons(user, today);
                navigationView.SelectedItem = navigationView.MenuItems[0];
            }
        }

        private static async Task UpdateLessons(Models.User user, DateTime today)
        {
            StaticData.CurrentUser = user;
            StaticData.Lessons.Clear();
            try
            {
                foreach (var item in await user.GetLessonsAsync(today, 7))
                {
                    StaticData.Lessons.Add(item);
                }
            }
            catch (HttpRequestException ex)
            {
                ContentDialog dialog = new ContentDialog { PrimaryButtonText = "OK", Content = ex.Message, Title = "Ошибка соединения" };
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog { PrimaryButtonText = "OK", Content = ex.Message, Title = "Ошибка" };
                await dialog.ShowAsync();
            }
        }

        public static void UpdateLessons(IEnumerable<LessonsGroup> lessons)
        {
            StaticData.Lessons.Clear();
            foreach (var item in lessons)
            {
                StaticData.Lessons.Add(item);
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
            sender.Text = args.SelectedItem.ToString();


        private async void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Models.User user)
            {
                StaticData.CurrentUser = user;
                navigationView.SelectedItem = navigationView.MenuItems[0];
                await UpdateLessons(user, DateTime.Today);
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

                    navigationView.SelectedItem = navigationView.MenuItems[0];
                    await UpdateLessons(student, DateTime.Today);
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
            catch (HttpRequestException)
            {
                //ToDo Show useful info
                var dialog = new ContentDialog { PrimaryButtonText = "OK", Title = "Ошибка", Content = $"Ошибка при обращении к серверу. Проверьте соединение или попробуйте позднее" };
                await dialog.ShowAsync();
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
            if (!(sender is MenuFlyoutItem item && item.DataContext is Models.User user))
                return;

            StaticData.Recent.Remove(user);
        }
    }
}
