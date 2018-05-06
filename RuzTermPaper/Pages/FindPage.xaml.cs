using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FindPage : Page
    {
        private SingletonData _data;

        public FindPage()
        {
            InitializeComponent();
            _data = SingletonData.Initialize();
            _data.ResetEvents();
            _data.TimetableLoadingSuccessed += (o, args) => MainPage.View.SelectedItem = MainPage.View.MenuItems[0];
            _data.TimetableLoadingFailed += async (o, args) => await Task.Delay(0);
        }

        private void RecentListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Models.User user)
                _data.CurrentUser = user;
        }

        private void EmailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!(sender is TextBox textBox) || e.Key != VirtualKey.Enter)
                return;
            if (textBox.Text.TrimEnd().EndsWith("@edu.hse.ru"))
                _data.CurrentUser = new Student(textBox.Text);
        }

        private async void AddMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuFlyoutItem item))
                return;

            try
            {
                await new Dialogs.ChooseDialog((Models.UserType)item.Tag).ShowAsync();
            }
            catch (HttpRequestException ex)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Title = "Connection_Error".Localize(),
                    Content = "Connection_Error_Details".Localize() + ex.Message
                };
                await dialog.ShowAsync();
            }

        }

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuFlyoutItem item && item.DataContext is Models.User user))
                return;

            _data.Recent.Remove(user);
        }
    }
}
