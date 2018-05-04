using Newtonsoft.Json;
using RuzTermPaper.Models;
using RuzTermPaper.Pages;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper
{
    public class SingletonData
    {
        private static SingletonData instance = null;
        private User _currentUser;
        private event EventHandler<CurrentUserChangedEventArgs> CurrentUserChanged;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser?.Equals(value) == true)
                    return;

                CurrentUserChanged(this, new CurrentUserChangedEventArgs(_currentUser, value));
                _currentUser = value;

            }
        }
        public ObservableCollection<LessonsGroup> Lessons { get; set; }
        public ObservableCollection<User> Recent { get; set; }

        private SingletonData()
        {
            Recent = new ObservableCollection<User>();
            Lessons = new ObservableCollection<LessonsGroup>();
            CurrentUserChanged += OnCurrentUserChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SingletonData Initialize()
        {
            if (instance == null)
                return instance = new SingletonData();

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <exception cref="Exception">Возникает при попытки повторно создать экземпляр из файла</exception>
        /// <returns></returns>
        public static async Task<SingletonData> Initialize(StorageFile file)
        {
            if (instance == null)
            {
                string jsonString = await FileIO.ReadTextAsync(file);
                instance = await Json.ToObjectAsync<SingletonData>(jsonString);
                return instance;
            }

            throw new Exception("SingletonAlreadyExistsException".Localize());
        }

        private async void OnCurrentUserChanged(object sender, CurrentUserChangedEventArgs e)
        {
            try
            {
                Lessons = new ObservableCollection<LessonsGroup>(await e.NewUser.GetLessonsAsync(DateTime.Today, 7));

                if (!Recent.Contains(e.NewUser))
                    Recent.Add(e.NewUser);
                if (MainPage.View != null)
                {
                    MainPage.View.SelectedItem = MainPage.View.MenuItems[0];
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Content = "Connection_Error_Details".Localize() + ex.Message,
                    Title = "Connection_Error".Localize()
                };
                await dialog.ShowAsync();
            }
            //catch (Exception ex)
            //{
            //    var dialog = new ContentDialog
            //    {
            //        PrimaryButtonText = "OK",
            //        Content = "Произошла ошибка. Проверьте данные и попробуйте еще раз. Подробности: " + ex.Message,
            //        Title = "Ошибка"
            //    };
            //    await dialog.ShowAsync();
            //}
        }
    }

    public class CurrentUserChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousUser">Предыдущий пользователь</param>
        /// <param name="newUser">Новый пользователь</param>
        public CurrentUserChangedEventArgs(User previousUser, User newUser)
        {
            PreviousUser = previousUser;
            NewUser = newUser;
        }

        public User PreviousUser { get; private set; }
        public User NewUser { get; private set; }
    }

}
