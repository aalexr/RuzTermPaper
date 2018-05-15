using System;

namespace RuzTermPaper.Models
{
    public class Lesson : IComparable<Lesson>
    {
        #region Public Properties
        public string Auditorium { get; set; }

        public string BeginLesson { get; set; }

        public string Building { get; set; }

        public DateTime DateOfNest { get; set; }

        public string Discipline { get; set; }

        public string EndLesson { get; set; }

        public string KindOfWork { get; set; }

        public string Lecturer { get; set; }
        #endregion

        public int CompareTo(Lesson other) => DateOfNest.CompareTo(other.DateOfNest);

        public override string ToString() =>
            $"{DateOfNest:ddd dd.MM.yy} {BeginLesson}-{EndLesson} {Discipline} ауд. {Auditorium}";
    }
}
