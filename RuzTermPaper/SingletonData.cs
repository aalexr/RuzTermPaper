using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace RuzTermPaper
{
    public class SingletonData : INotifyPropertyChanged
    {
        private static SingletonData instance = null;
        private User _currentUser;
        private ObservableCollection<LessonsGroup> _lessons;

        public event PropertyChangedEventHandler PropertyChanged;


        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUser)));
            }
        }
        public ObservableCollection<LessonsGroup> Lessons
        {
            get => _lessons; set
            {
                _lessons = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lessons)));
            }
        }
        public ObservableCollection<User> Recent { get; set; }

        private SingletonData()
        {
            Recent = new ObservableCollection<User>();
            Lessons = new ObservableCollection<LessonsGroup>();
        }

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
        /// <exception cref="Exception">Возникает при попытке повторно создать экземпляр из файла</exception>
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
    }
}
