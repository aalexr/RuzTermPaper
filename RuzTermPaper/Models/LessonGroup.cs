using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RuzTermPaper.Models
{
    public class LessonsGroup
    {
        public LessonsGroup(DateTime key)
        {
            Key = key;
            Elements = new List<Lesson>(0);
        }
        public LessonsGroup(DateTime key, IEnumerable<Lesson> elements)
        {
            Key = key;
            Elements = new List<Lesson>(elements);
        }

        public DateTime Key { get; set; }
        public List<Lesson> Elements { get; set; }
    }
}
