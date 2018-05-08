using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Dialogs
{
    public sealed partial class ChooseDialog : ContentDialog
    {
        private SingletonData _data = SingletonData.Initialize();
        private UserType _type;
        private User _choosedUser;
        private CancellationTokenSource tokenSource;

        private ChooseDialog() => InitializeComponent();
        public ChooseDialog(UserType type) : this()
        {
            _type = type;
            tokenSource = new CancellationTokenSource();

            switch (_type)
            {
                case UserType.Group:
                    Search.Header = "ChooseDialog_SuggestBox_Header_Group".Localize();
                    Search.PlaceholderText = "ChooseDialog_SuggestBox_Placeholder_Group".Localize();
                    break;
                case UserType.Lecturer:
                    Search.Header = "ChooseDialog_SuggestBox_Header_Lecturer".Localize();
                    Search.PlaceholderText = "ChooseDialog_SuggestBox_Placeholder_Lecturer".Localize();
                    break;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (_choosedUser != null)
                _data.CurrentUser = _choosedUser;
        }

        #region AutoSuggestBox EventHandlers
        private async void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    if (string.IsNullOrEmpty(sender.Text))
                        return;

                    if (_type == UserType.Lecturer)
                        sender.ItemsSource = await Lecturer.FindAsync(sender.Text, tokenSource.Token);
                    else
                        sender.ItemsSource = await Group.FindAsync(sender.Text, tokenSource.Token);

                    IsPrimaryButtonEnabled = false;
                }
                catch (TaskCanceledException)
                {
                    // Игнорировать
                }
                catch (HttpRequestException ex)
                {
                    FindName("ErrorSP");
                    ErrorBlock.Text = ex.Message;
                }
                catch (Exception) { }
            }
        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!(args.ChosenSuggestion is Models.User user))
                return;
            _choosedUser = user;
            IsPrimaryButtonEnabled = true;
        }
        #endregion
    }
}
