using System.Collections.ObjectModel;

namespace RuzTermPaper
{
    static class StaticData
    {
        public static ObservableCollection<Group> Groups { get; private set; } = new ObservableCollection<Group>();
        public static ObservableCollection<Lecturer> Lecturers { get; private set; } = new ObservableCollection<Lecturer>();
        public static ObservableCollection<string> StudentEmails { get; private set; } = new ObservableCollection<string>();
    }
}
