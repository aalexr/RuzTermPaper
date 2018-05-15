using RuzTermPaper.Tools;
using System;
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
        private SingletonData _data = SingletonData.Initialize();
        public MainPage()
        {
            InitializeComponent();
            View = PivotView;
            currentFrame = (Frame)Window.Current.Content;
            _data.PropertyChanged += _data_PropertyChanged;
        }

        private async void _data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_data.CurrentUser))
            {
                try
                {
                    _data.Lessons = await _data.CurrentUser.GetLessonsAsync(DateTime.Today, 7);
                }
                catch (Exception ex)
                {
                    await new Dialogs.ErrorDialog(ex).ShowAsync();
                    return;
                }
                
                MainPage.View.SelectedIndex = 0;
                _data.Recent.AddIfNew(_data.CurrentUser);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewPIFrame.Navigate(typeof(TimetablePage));
            FindPIFrame.Navigate(typeof(FindPage));
        }
    }
}
