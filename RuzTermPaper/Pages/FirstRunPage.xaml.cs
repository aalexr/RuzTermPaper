using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FirstRunPage : Page
    {
        private SingletonData _data;
        private SolidColorBrush _invalidFormat;
        private UserType _type;
        private CancellationTokenSource tokenSource;

        public FirstRunPage()
        {
            _data = SingletonData.Initialize();
            _invalidFormat = new SolidColorBrush(Colors.Red);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;
            FindName("Buttons");
            Hint.Text = "FirstRunPage_Hint_Text".Localize();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio)
            {
                _type = (UserType)radio.Tag;
                if (SearchBox == null)
                    FindName("SearchBox");
            }
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            _data.TimetableLoadingSuccessed += (o, e) =>
            {
                ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["FirstRun"] = false;
            };

            if (_type == UserType.Student)
            {
                if (args.QueryText.EndsWith("@edu.hse.ru"))
                {
                    _data.TimetableLoadingFailed += (o, e) =>
                    {
                        sender.Text = string.Empty;
                        ErrorBlock.Visibility = Visibility.Visible;
                    };
                    _data.CurrentUser = new Student(args.QueryText);
                }
            }
            else
            {
                if (args.ChosenSuggestion != null)
                    _data.CurrentUser = (User)args.ChosenSuggestion;
            }
        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    if (string.IsNullOrEmpty(sender.Text))
                        return;
                    tokenSource?.Cancel();
                    tokenSource = new CancellationTokenSource();
                    if (_type == UserType.Lecturer)
                    {
                        List<Lecturer> _suggestions = await Lecturer.FindAsync(sender.Text, tokenSource.Token);
                        if (_suggestions.Count > 0)
                            sender.ItemsSource = await Lecturer.FindAsync(sender.Text, tokenSource.Token);
                        else
                            sender.ItemsSource = new[] { "NoResult".Localize() };

                    }
                    else
                    {
                        List<Group> _suggestions = await Group.FindAsync(sender.Text, tokenSource.Token);
                        if (_suggestions.Count > 0)
                            sender.ItemsSource = await Group.FindAsync(sender.Text, tokenSource.Token);
                        else
                            sender.ItemsSource = new[] { "NoResult".Localize() };
                    }
                }
                catch (TaskCanceledException)
                {
                    // Игнорировать
                }
                catch (HttpRequestException ex)
                {
                    // Показать сообщение
                }
                catch (Exception) { }
            }

        }

        private void RB_Unchecked(object sender, RoutedEventArgs e) => SearchBox.ItemsSource = null;

        private void StudentRB_Checked(object sender, RoutedEventArgs e)
        {
            if (SearchBox == null)
                FindName("SearchBox");

            SearchBox.PlaceholderText = "Введите почту на edu.hse.ru";
        }
    }
}