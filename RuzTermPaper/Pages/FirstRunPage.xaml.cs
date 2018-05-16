using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            InitializeComponent();
            _data = SingletonData.Initialize();
            _data.PropertyChanged += _data_PropertyChanged;
            _invalidFormat = new SolidColorBrush(Colors.Red);
        }

        private async void _data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_data.CurrentUser))
            {
                try
                {
                    _data.Lessons = await _data.CurrentUser.GetLessonsAsync(DateTime.Today, 7);
                    ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                }
                catch (Exception ex)
                {
                    await new Dialogs.ErrorDialog(ex).ShowAsync();
                    return;
                }
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["FirstRun"] = false;
                _data.Recent.AddIfNew(_data.CurrentUser);
            }
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

        private void StudentRB_Checked(object sender, RoutedEventArgs e)
        {
            if (SearchBox == null)
                FindName("SearchBox");

            _type = UserType.Student;
            SearchBox.PlaceholderText = "example@edu.hse.ru";
            SearchBox.ItemsSource = null;
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (_type == UserType.Student)
            {
                if (args.QueryText.EndsWith("@edu.hse.ru"))
                {
                    _data.CurrentUser = new Student(args.QueryText);
                }
                else
                    sender.BorderBrush = _invalidFormat;
            }
            else
            {
                if (args.ChosenSuggestion != null && args.ChosenSuggestion is User user)
                    _data.CurrentUser = user;
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

                    switch (_type)
                    {
                        case UserType.Student:
                            break;
                        case UserType.Lecturer:
                            {
                                List<Lecturer> _suggestions = await Lecturer.FindAsync(sender.Text, tokenSource.Token);
                                if (_suggestions.Count > 0)
                                    sender.ItemsSource = await Lecturer.FindAsync(sender.Text, tokenSource.Token);
                                else
                                    sender.ItemsSource = new[] { "NoResult".Localize() };
                            }
                            break;
                        case UserType.Group:
                            {
                                List<Group> _suggestions = await Group.FindAsync(sender.Text, tokenSource.Token);
                                if (_suggestions.Count > 0)
                                    sender.ItemsSource = await Group.FindAsync(sender.Text, tokenSource.Token);
                                else
                                    sender.ItemsSource = new[] { "NoResult".Localize() };
                            }
                            break;
                    }
                }
                catch (TaskCanceledException)
                {
                    // Игнорировать
                }
                catch (HttpRequestException ex)
                {
                    await new Dialogs.ErrorDialog(ex).ShowAsync();
                }
                catch (Exception) { }
            }

        }

        private void RB_Unchecked(object sender, RoutedEventArgs e) => SearchBox.ItemsSource = null;

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            _data.PropertyChanged -= _data_PropertyChanged;
        }
    }
}