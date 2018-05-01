using RuzTermPaper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuzTermPaper
{
    static class StaticData
    {
        public static User CurrentUser { get; set; }
        public static ObservableCollection<LessonsGroup> Lessons { get; set; } = new ObservableCollection<LessonsGroup>();
        public static ObservableCollection<User> Recent { get; set; } = new ObservableCollection<User>();
        public static IEnumerable<User> Users { get; set; }
    }
}
