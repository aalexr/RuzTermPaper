using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RuzTermPaper.Models
{
    public class LessonsGroup : IGrouping<DateTime, Lesson>
    {
        public LessonsGroup() { }
        public LessonsGroup(DateTime key, IEnumerable<Lesson> elements)
        {
            Key = key;
            Elements = new List<Lesson>(elements);
        }

        public DateTime Key { get; set; }
        public List<Lesson> Elements { get; set; }

        public IEnumerator<Lesson> GetEnumerator() => Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();
    }
}
