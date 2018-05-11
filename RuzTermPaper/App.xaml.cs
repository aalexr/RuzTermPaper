using RuzTermPaper.Pages;
using RuzTermPaper.Tools;
using System;
using System.Net.Http;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace RuzTermPaper
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Общий экземпляр <see cref="HttpClient"/> для загрузки данных через Интернет
        /// </summary>
        public static HttpClient Http { get; } = new HttpClient();

        /// <summary>
        /// Экземпляр класса данных приложения
        /// </summary>
        private SingletonData _data;

        /// <summary>
        /// Имя файла с сериализованными данными
        /// </summary>
        private const string dataFileName = "data.json";

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
            #region Инициализация фрейма
            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.PointerPressed += On_PointerPressed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }
            #endregion

            if (e.PrelaunchActivated)
                return;

            // Если стек навигации не восстанавливается для перехода к первой странице,
            // настройка новой страницы путем передачи необходимой информации в качестве параметра
            // параметр
            if (rootFrame.Content == null)
            {
                // Если это первый запуск приложения или первоначальная настройка не завершена,
                // перейти на экран первоначальной настройки
                if (ApplicationData.Current.LocalSettings.Values["FirstRun"] == null)
                {
                    _data = SingletonData.Initialize();
                    rootFrame.Navigate(typeof(FirstRunPage), e.Arguments);
                }
                else
                {

                    // Иначе попытаться восстановить данные из кэша
                    #region Восстановление данных из локального хранилища
                    try
                    {
                        StorageFile dataFile = await ApplicationData.Current.LocalFolder.GetFileAsync(dataFileName);
                        _data = await SingletonData.Initialize(dataFile);
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        // Если кэша данных нет, то создаем пустые данные
                        _data = SingletonData.Initialize();
                    }
                    // И перейти к первой странице
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    #endregion
                }
            }
            // Обеспечение активности текущего окна
            Window.Current.Activate();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            
            //draw into the title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = (Windows.UI.Color)Resources["SystemBaseHighColor"];
            viewTitleBar.ButtonInactiveBackgroundColor = (Windows.UI.Color)Resources["SystemBaseHighColor"];
            viewTitleBar.ButtonForegroundColor = (Windows.UI.Color)Resources["SystemBaseHighColor"];
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            if (_data != null)
            {
                var serialized = await Json.StringifyAsync(_data);
                var file =
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(dataFileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, serialized);
            }

            deferral.Complete();
        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            //e.Handled = On_BackRequested();
        }

        private void On_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            bool isXButton1Pressed = e.GetCurrentPoint(sender as UIElement).Properties.PointerUpdateKind == PointerUpdateKind.XButton1Pressed;

            if (isXButton1Pressed)
            {
                //e.Handled = On_BackRequested();
            }
        }
    }
}
