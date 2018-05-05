using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Dialogs
{
    public sealed partial class ChooseDialog : ContentDialog
    {
        private SingletonData _data = SingletonData.Initialize();
        private UserType _type;
        private IEnumerable<User> _users;
        private User _choosedUser;
        private AutoSuggestBox suggestBox;

        private ChooseDialog() => InitializeComponent();
        public ChooseDialog(UserType type) : this()
        {
            _type = type;
            suggestBox = (AutoSuggestBox)Content;

            switch (_type)
            {
                case UserType.Group:
                    suggestBox.Header = "ChooseDialog_SuggestBox_Header_Group".Localize(); ;
                    suggestBox.PlaceholderText = "ChooseDialog_SuggestBox_Placeholder_Group".Localize(); ;
                    break;
                case UserType.Lecturer:
                    suggestBox.Header = "ChooseDialog_SuggestBox_Header_Lecturer".Localize();
                    suggestBox.PlaceholderText = "ChooseDialog_SuggestBox_Placeholder_Lecturer".Localize();
                    break;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (_choosedUser != null)
                _data.CurrentUser = _choosedUser;
        }

        #region AutoSuggestBox EventHandlers
        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = string.IsNullOrWhiteSpace(sender.Text)
                    ? null
                    : _users?.Where(x => x.Name.Contains(sender.Text, StringComparison.CurrentCultureIgnoreCase));
                IsPrimaryButtonEnabled = false;
            }
        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!(args.ChosenSuggestion is Models.User user))
                return;
            _choosedUser = user;
            IsPrimaryButtonEnabled = true;
        }

        private async void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            if (_type == UserType.Lecturer)
                _users = await Lecturer.FindAsync();
            else
                _users = await Group.FindAsync();
        }
        #endregion
    }
}
