using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private ApplicationDataContainer LocalSettings { get; } = ApplicationData.Current.LocalSettings;
        private Frame currentFrame = (Frame)Window.Current.Content;

        public SettingsPage()
        {
            this.InitializeComponent();
            ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => (ElementTheme)c?.Tag == App.CurrentTheme).IsChecked = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e) => On_BackRequested();

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (currentFrame.CanGoBack)
            {
                currentFrame.GoBack();
                return true;
            }
            return false;
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio)
            {
                App.CurrentTheme = (ElementTheme)radio.Tag;
            }
        }
    }
}
