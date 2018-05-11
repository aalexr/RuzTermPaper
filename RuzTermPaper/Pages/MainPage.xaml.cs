using System;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Pivot View { get; private set; }
        private Frame currentFrame;
        public MainPage()
        {
            InitializeComponent();
            View = PivotView;
            currentFrame = (Frame)Window.Current.Content;

            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                var GoBack = new KeyboardAccelerator
                {
                    Key = VirtualKey.GoBack
                };
                GoBack.Invoked += BackInvoked;
                var AltLeft = new KeyboardAccelerator
                {
                    Key = VirtualKey.Left
                };
                AltLeft.Invoked += BackInvoked;
                this.KeyboardAccelerators.Add(GoBack);
                this.KeyboardAccelerators.Add(AltLeft);
                // ALT routes here
                AltLeft.Modifiers = VirtualKeyModifiers.Menu; 
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewPIFrame.Navigate(typeof(TimetablePage));
            FindPIFrame.Navigate(typeof(FindPage));
        }

        private void SettingAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            currentFrame.Navigate(typeof(SettingsPage));
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

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }
    }
}
