using RuzTermPaper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RuzTermPaper
{

    static class StaticData
    {
        public static List<LessonsGroup> Lessons { get; set; }
        public static ObservableCollection<User> Recent { get; set; } = new ObservableCollection<User>();
        //public static List<Group> Groups;
        //public static List<Lecturer> Lecturers;
    }
}
