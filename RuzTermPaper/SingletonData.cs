using Newtonsoft.Json;
using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

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
    }
}
