using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.System;
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
        private ObservableCollection<Models.User> _suggestions;
        private Models.UserType _type;
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
                try
                {
                    _type = (Models.UserType)radio.Tag;
                    await SetSuggestionsList(_type);
                }
                catch (Exception ex)
                {
                    radio.IsChecked = false;
                    await new Dialogs.ErrorDialog(ex).ShowAsync();
                    return;
                }
            }

            if (SearchBox == null)
                FindName("SearchBox");
        }

        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = new Frame();
            Window.Current.Content = frame;
            frame.Navigate(typeof(FirstRunPage));
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (StudentRB.IsChecked == true && args.QueryText.EndsWith("@edu.hse.ru"))
            {
                _data.TimetableLoadingSuccessed += (o, e) => ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                _data.TimetableLoadingFailed += (o, e) =>
                {
                    sender.Text = string.Empty;
                    ErrorBlock.Visibility = Visibility.Visible;
                };
                _data.CurrentUser = new Student(args.QueryText);
            }
        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            await SetSuggestionsList(_type);
        }

        private async Task SetSuggestionsList(Models.UserType type)
        {
            switch (type)
            {
                case Models.UserType.Student:
                    _suggestions = null;
                    break;
                case Models.UserType.Lecturer:
                    _suggestions = new ObservableCollection<Models.User>(await Lecturer.FindAsync());
                    break;
                case Models.UserType.Group:
                    _suggestions = new ObservableCollection<Models.User>(await Group.FindAsync());
                    break;
            }
        }
    }
}