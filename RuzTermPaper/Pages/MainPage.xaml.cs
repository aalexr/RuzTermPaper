using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            contentFrame.Navigate(typeof(Pages.TimetablePage), e.Parameter);
            base.OnNavigatedTo(e);
        }

        private void navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                contentFrame.Navigate(typeof(Pages.SettingsPage));
            }
            else
            {
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);

                switch (item.Tag)
                {
                    case "timetable":
                        contentFrame.Navigate(typeof(Pages.TimetablePage));
                        break;
                    case "Find":
                        contentFrame.Navigate(typeof(Pages.Find), contentFrame);
                        break;
                }
            }
        }

        private void navView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args) => AppTitle.Visibility = args.DisplayMode == NavigationViewDisplayMode.Expanded
                ? Windows.UI.Xaml.Visibility.Visible
                : Windows.UI.Xaml.Visibility.Collapsed;
    }
}
