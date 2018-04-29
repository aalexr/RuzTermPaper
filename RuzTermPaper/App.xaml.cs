using Newtonsoft.Json.Linq;
using RuzTermPaper.Models;
using RuzTermPaper.Pages;
using RuzTermPaper.Tools;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RuzTermPaper
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        public static readonly HttpClient Http = new HttpClient();
        /// <inheritdoc />
        /// <summary>
        /// Инициализирует одноэлементный объект приложения. Это первая выполняемая строка разрабатываемого
        /// кода; поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем.  Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated)
                return;

            // Если стек навигации не восстанавливается для перехода к первой странице,
            // настройка новой страницы путем передачи необходимой информации в качестве параметра
            // параметр
            if (rootFrame.Content == null)
            {
                #region Restore Lessons List
                StorageFile lessonsFile = await ApplicationData.Current.LocalFolder.GetFileAsync("lessons.json");
                string value = await FileIO.ReadTextAsync(lessonsFile);
                Lesson[] deserialized = await Json.ToObjectAsync<Lesson[]>(value);

                StaticData.Lessons = deserialized
                    .GroupBy(L => L.DateOfNest)
                    .Select(G => new LessonsGroup(G.Key, G))
                    .ToList();
                #endregion

                #region Restore Recent List
                try
                {
                    StorageFile recentFile = await ApplicationData.Current.LocalFolder.GetFileAsync("recent.json");
                    string recent = await FileIO.ReadTextAsync(recentFile);

                    foreach (var token in JArray.Parse(recent))
                    {
                        if (token["groupOid"] != null)
                            StaticData.Recent.Add(token.ToObject<Group>());
                        else
                        {
                            if (token["lecturerOid"] != null)
                                StaticData.Recent.Add(token.ToObject<Lecturer>());
                            else
                            {
                                if (token["Email"] != null)
                                    StaticData.Recent.Add(token.ToObject<Student>());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Content = ex.Message,
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                }
                #endregion

                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Обеспечение активности текущего окна
            Window.Current.Activate();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            if (StaticData.Lessons != null)
            {
                await SaveAsJsonAsync(StaticData.Lessons.SelectMany(x => x.Elements), "lessons.json");
            }

            if (StaticData.Recent != null)
            {
                await SaveAsJsonAsync(StaticData.Recent, "recent.json");
            }

            deferral.Complete();
        }

        private async static Task SaveAsJsonAsync<T>(T obj, string fileName)
        {
            string content = await Json.StringifyAsync(obj);
            StorageFile file =
                await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }
    }
}
