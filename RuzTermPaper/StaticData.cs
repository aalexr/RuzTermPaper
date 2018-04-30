using RuzTermPaper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuzTermPaper
{

    static class StaticData
    {
        public static List<LessonsGroup> Lessons { get; set; }
        public static ObservableCollection<User> Recent { get; set; } = new ObservableCollection<User>();
        public static IEnumerable<User> Users { get; set; }
    }
}
