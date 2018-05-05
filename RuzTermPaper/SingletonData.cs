using RuzTermPaper.Models;
using RuzTermPaper.Pages;
using RuzTermPaper.Tools;
using System;
using System.Collections.ObjectModel;
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

        public event EventHandler<TimetableLoadingSuccessedEventArgs> TimetableLoadingSuccessed;
        public event EventHandler<TimetableLoadingFailedEventArgs> TimetableLoadingFailed;

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
            //TimetableLoadingFailed += (o, e) => { };
            //TimetableLoadingSuccessed += (o, e) => { };
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
            }
            catch (Exception ex)
            {
                TimetableLoadingFailed(this, new TimetableLoadingFailedEventArgs(ex));
                return;
            }
            
            if (!Recent.Contains(e.NewUser))
                Recent.Add(e.NewUser);
            TimetableLoadingSuccessed(this, new TimetableLoadingSuccessedEventArgs());
        }
    }

    public class TimetableLoadingSuccessedEventArgs : EventArgs
    { }
    public class TimetableLoadingFailedEventArgs : EventArgs
    {
        public TimetableLoadingFailedEventArgs(Exception exception = null)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
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
