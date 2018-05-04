using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
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
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                this.FindName("ForLecturersButton");
                this.FindName("EmailBlock");
                button.Visibility = Visibility.Collapsed;
                Hint.Text = "FirstRunPage_Hint_Text".Localize();
            }
        }

        private void EmailBlock_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (!(sender is TextBox textBox) || e.Key != VirtualKey.Enter || !textBox.Text.EndsWith("@edu.hse.ru"))
                return;

            data.CurrentUser = new Student(textBox.Text);
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }

        private async void ForLecturersButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.ChooseDialog(Models.UserType.Lecturer);
            await dialog.ShowAsync();
        }
    }
}
