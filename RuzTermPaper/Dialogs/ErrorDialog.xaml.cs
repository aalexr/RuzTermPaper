using System;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using RuzTermPaper.Tools;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Dialogs
{
    public sealed partial class ErrorDialog : ContentDialog
    {
        private ErrorDialog() => InitializeComponent();

        public ErrorDialog(Exception exception) : this()
        {
            if (exception is HttpRequestException ex)
                Content = "ErrorDialog_NetErrMsg".Localize() + ex.Message;
            else
                Content = exception.Message;
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
