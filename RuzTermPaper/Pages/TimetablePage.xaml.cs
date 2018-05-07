using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RuzTermPaper.Pages
{
    /// <inheritdoc />
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TimetablePage : Page
    {
        private readonly SingletonData _data;
        public TimetablePage()
        {
            InitializeComponent();
            _data = SingletonData.Initialize();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Тоже опасно, может скрыть кнопку, если расписания нет?
            DateTime lastDate = _data.Lessons.Last().Key.AddDays(1);
            // Упадет с исключением если нет интернета
            foreach(var l in await _data.CurrentUser.GetLessonsAsync(lastDate, 7))
            {
                _data.Lessons.Add(l);
            }
        }
    }
}
