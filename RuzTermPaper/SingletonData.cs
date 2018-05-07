using RuzTermPaper.Models;
using RuzTermPaper.Tools;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace RuzTermPaper
{
    public class SingletonData
    {
        private static SingletonData instance = null;
        private User _currentUser;
        private event EventHandler<CurrentUserSetEventArgs> CurrentUserSet;

        public event EventHandler<EventArgs> TimetableLoadingSuccessed;
        public event EventHandler<TimetableLoadingFailedEventArgs> TimetableLoadingFailed;

        public User CurrentUser
        {
            get => _currentUser;
            set => CurrentUserSet(this, new CurrentUserSetEventArgs(_currentUser, _currentUser = value));
        }
        public ObservableCollection<LessonsGroup> Lessons { get; set; }
        public ObservableCollection<User> Recent { get; set; }

        private SingletonData()
        {
            Recent = new ObservableCollection<User>();
            Lessons = new ObservableCollection<LessonsGroup>();
            CurrentUserSet += OnCurrentUserChanged;
            TimetableLoadingFailed += (o, e) => { };
            TimetableLoadingSuccessed += (o, e) => { };
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

        public void ResetEvents()
        {
            TimetableLoadingFailed = (o, e) => {};
            TimetableLoadingSuccessed = (o, e) => {};
        }

        private async void OnCurrentUserChanged(object sender, CurrentUserSetEventArgs e)
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
            TimetableLoadingSuccessed(this, EventArgs.Empty/*new TimetableLoadingSuccessedEventArgs()*/);
        }
    }

    public class TimetableLoadingFailedEventArgs : EventArgs
    {
        public TimetableLoadingFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
    }


    public class CurrentUserSetEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousUser">Предыдущий пользователь</param>
        /// <param name="newUser">Новый пользователь</param>
        public CurrentUserSetEventArgs(User previousUser, User newUser)
        {
            PreviousUser = previousUser;
            NewUser = newUser;
        }

        public User PreviousUser { get; private set; }
        public User NewUser { get; private set; }
        public bool IsAnother => PreviousUser.Equals(NewUser);
    }

}
