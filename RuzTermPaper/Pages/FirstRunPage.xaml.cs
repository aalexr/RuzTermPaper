using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Linq;
using System.Collections.ObjectModel;
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
        private ObservableCollection<User> _suggestions;
        private UserType _type;
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
            FindName("StartOver");
            Hint.Text = "FirstRunPage_Hint_Text".Localize();
        }

        private async void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio)
            {
                _type = (UserType)radio.Tag;
                try
                {
                    Progress.IsActive = true;
                    await SetSuggestionsList(_type, string.Empty);
                    if (SearchBox == null)
                        FindName("SearchBox");
                }
                catch (Exception ex)
                {
                    radio.IsChecked = false;
                    await new Dialogs.ErrorDialog(ex).ShowAsync();
                    return;
                }
                finally
                {
                    Progress.IsActive = false;
                }
            }
        }

        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(FirstRunPage));
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

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                    sender.ItemsSource = null;

                sender.ItemsSource = _suggestions?.Where(s => s.ToString().Contains(sender.Text));

                //await SetSuggestionsList(_type, sender.Text);
                //sender.ItemsSource = _suggestions;
            }

        }

        private async Task SetSuggestionsList(UserType type, string textToFind)
        {
            //switch (type)
            //{
            //    case UserType.Student:
            //        _suggestions = null;
            //        break;
            //    case UserType.Lecturer:
            //        _suggestions = new ObservableCollection<User>(await Lecturer.FindAsync(textToFind));
            //        break;
            //    case UserType.Group:
            //        _suggestions = new ObservableCollection<User>(await Group.FindAsync(textToFind));
            //        break;
            //}
        }

        private void RB_Unchecked(object sender, RoutedEventArgs e) => _suggestions = null;
    }
}