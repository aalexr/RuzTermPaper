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
        public User CurrentUser { get; set; }
        public ObservableCollection<LessonsGroup> Lessons { get; set; }
        public ObservableCollection<User> Recent { get; set; }
        [JsonIgnore]
        public IEnumerable<User> Users { get; set; }

        private SingletonData()
        {
            Recent = new ObservableCollection<User>();
            Lessons = new ObservableCollection<LessonsGroup>();
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

            throw new Exception("Уже существует обьект этого класса.");
        }

        public async Task<bool> UpdateLessons(Models.User user, DateTime date)
        {
            IEnumerable<LessonsGroup> lessons = null;

            try
            {
                lessons = await user.GetLessonsAsync(date, 7);
            }
            catch (HttpRequestException ex)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Content = "Произошла ошибка при обращении к серверу. Проверьте соединение или попробуйте позднее. Подробности: " + ex.Message,
                    Title = "Ошибка соединения"
                };
                await dialog.ShowAsync();
                return false;
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog
                {
                    PrimaryButtonText = "OK",
                    Content = "Произошла ошибка. Проверьте данные и попробуйте еще раз. Подробности: " + ex.Message,
                    Title = "Ошибка"
                };
                await dialog.ShowAsync();
                return false;
            }

            CurrentUser = user;
            Lessons.Clear();
            foreach (var item in lessons)
            {
                Lessons.Add(item);
            }
            if (MainPage.View != null)
            {
                MainPage.View.SelectedItem = MainPage.View.MenuItems[0];
            }
            return true;
        }
    }
}
