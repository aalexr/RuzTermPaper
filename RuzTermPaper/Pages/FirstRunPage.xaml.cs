using RuzTermPaper.Models;
using System;
using Windows.Storage;
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
        private SingletonData data = SingletonData.Initialize();
        private SolidColorBrush invalidFormat = new SolidColorBrush(Colors.Red);
        public FirstRunPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                this.FindName("ForLecturersButton");
                this.FindName("EmailBlock");
                button.Visibility = Visibility.Collapsed;
                Hint.Text = "Введите корпоративную почту на домене edu.hse.ru и нажмите Enter";
            }
        }

        private async void EmailBlock_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (!(sender is TextBox textBox) || e.Key != VirtualKey.Enter)
                return;
            Student student;

            if (textBox.Text.EndsWith("@edu.hse.ru")
                && await data.UpdateLessons(student = new Student(textBox.Text), DateTime.Today)
                && !data.Recent.Contains(student))
            {
                data.Recent.Add(student);
                ApplicationData.Current.LocalSettings.Values["FirstRun"] = false;
                Window.Current.Content = new Frame();
                (Window.Current.Content as Frame).Navigate(typeof(MainPage));
            }
            else
            {
                textBox.BorderBrush = invalidFormat;
            }
        }
    }
}
