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

        public event EventHandler<EventArgs> TimetableLoadingSuccessed;
        public event EventHandler<TimetableLoadingFailedEventArgs> TimetableLoadingFailed;
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
            PropertyChanged += OnCurrentUserChanged;
            TimetableLoadingFailed += (o, e) => { };
            TimetableLoadingSuccessed += (o, e) => { };
            Lessons.CollectionChanged += (o, e) => { };
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
            TimetableLoadingFailed = (o, e) => { };
            TimetableLoadingSuccessed = (o, e) => { };
        }

        private async void OnCurrentUserChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is SingletonData data && e.PropertyName == nameof(CurrentUser))
            {
                try
                {
                    Lessons = new ObservableCollection<LessonsGroup>(await data.CurrentUser.GetLessonsAsync(DateTime.Today, 7));
                }
                catch (Exception ex)
                {
                    TimetableLoadingFailed(this, new TimetableLoadingFailedEventArgs(ex));
                    return;
                }

                if (!Recent.Contains(data.CurrentUser))
                    Recent.Add(data.CurrentUser);
                TimetableLoadingSuccessed(this, EventArgs.Empty/*new TimetableLoadingSuccessedEventArgs()*/); 
            }
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
