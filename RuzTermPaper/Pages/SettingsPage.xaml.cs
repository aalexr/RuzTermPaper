using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            currentFrame.KeyDown += (o, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.Back)
                    On_BackRequested();
            };
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LocalSettings.Values["FirstRun"] = null;
            Application.Current.Exit();
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            base.OnNavigatedTo(e);
        }
    }
}
