using System;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
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
        /// <summary>
        /// Экземпляр <see cref="NavigationView"/> главной страницы
        /// </summary>
        public static Pivot View { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            View = NavView;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => PivotItem1.Navigate(typeof(TimetablePage));

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                PivotItem1.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                var pageName = (string)selectedItem.Tag;
                PivotItem1.Navigate(Type.GetType(pageName));
            }
        }

        private void NavView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
