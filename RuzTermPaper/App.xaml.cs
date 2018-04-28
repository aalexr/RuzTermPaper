using Newtonsoft.Json.Linq;
using RuzTermPaper.Models;
using RuzTermPaper.Pages;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            Current.Resuming += OnResuming;
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

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated ||
                    e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated) return;
            if (rootFrame.Content == null)
            {
                // Если стек навигации не восстанавливается для перехода к первой странице,
                // настройка новой страницы путем передачи необходимой информации в качестве параметра
                // параметр

                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("lessons.json");
                string value = await FileIO.ReadTextAsync(file);
                List<ItemsGroup> deser = await Json.ToObjectAsync<List<ItemsGroup>>(value);

                //StaticData.Lessons =
                //    new List<ItemsGroup>(deser
                //    .GroupBy(x => x.DateOfNest, (key, list) => new ItemsGroup(key, list)));

                try
                {
                    StorageFile recent = await ApplicationData.Current.LocalFolder.GetFileAsync("recent.json");
                    string str = await FileIO.ReadTextAsync(recent);

                    foreach (var token in JArray.Parse(str))
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
                catch (Exception)
                {

                }



                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Обеспечение активности текущего окна
            Window.Current.Activate();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
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
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции

            var storage = ApplicationData.Current.LocalFolder;

            if (StaticData.Lessons != null)
            {
                string contents = await Json.StringifyAsync(StaticData.Lessons);
                StorageFile file = await storage.CreateFileAsync("lessons.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, contents);
            }

            if (StaticData.Recent != null)
            {
                await FileIO.WriteTextAsync(
                    await storage.CreateFileAsync("recent.json", CreationCollisionOption.ReplaceExisting),
                    await Json.StringifyAsync(StaticData.Recent));
            }

            deferral.Complete();
        }

        private void OnResuming(object sender, object args)
        {

        }
    }
}
