using RuzTermPaper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RuzTermPaper
{
    static class StaticData
    {
        public static IOrderedEnumerable<IGrouping<System.DateTime, Lesson>> Lessons { get; set; }
        public static ObservableCollection<Receiver> Recent { get; set; } = new ObservableCollection<Receiver>();
        public static List<Group> Groups;
        public static List<Lecturer> Lecturers;
    }
}
