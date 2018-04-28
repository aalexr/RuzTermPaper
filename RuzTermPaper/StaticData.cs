using RuzTermPaper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RuzTermPaper
{
    public class ItemsGroup : IGrouping<DateTime, Lesson>
    {
        private List<Lesson> LessonsGroup { get; set; }

        public ItemsGroup(DateTime key, IEnumerable<Lesson> lessons)
        {
            Key = key;
            LessonsGroup = new List<Lesson>(lessons);
        }

        public DateTime Key { get; private set; }

        public IEnumerator<Lesson> GetEnumerator() => LessonsGroup.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => LessonsGroup.GetEnumerator();
    }

    static class StaticData
    {
        public static List<ItemsGroup> Lessons { get; set; }
        public static ObservableCollection<Receiver> Recent { get; set; } = new ObservableCollection<Receiver>();
        public static List<Group> Groups;
        public static List<Lecturer> Lecturers;
    }
}
