using Windows.UI.Xaml;
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
        public static Pivot View { get; private set; }
        private Frame currentFrame;
        public MainPage()
        {
            InitializeComponent();
            View = PivotView;
            currentFrame = (Frame)Window.Current.Content;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewPIFrame.Navigate(typeof(TimetablePage));
            FindPIFrame.Navigate(typeof(FindPage));
        }
    }
}
