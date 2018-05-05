using System;

namespace RuzTermPaper.Models
{
    public class Lesson : IComparable<Lesson>
    {
        #region Public Fields
        public string Auditorium { get; set; }
        //public int AuditoriumOid { get; set; }
        public string BeginLesson { get; set; }
        public string Building { get; set; }
        //public string Date { get; set; }
        public DateTime DateOfNest { get; set; }
        //public int DayOfWeek { get; set; }
        //public string DayOfWeekString { get; set; }
        //public string DetailInfo { get; set; }
        public string Discipline { get; set; }
        //public string Disciplineinplan { get; set; }
        //public int Disciplinetypeload { get; set; }
        public string EndLesson { get; set; }
        //public object Group { get; set; }
        //public int GroupOid { get; set; }
        //public bool IsBan { get; set; }
        public string KindOfWork { get; set; }
        public string Lecturer { get; set; }
        //public int LecturerOid { get; set; }
        //public string Stream { get; set; }
        //public int StreamOid { get; set; }
        //public object SubGroup { get; set; }
        //public int SubGroupOid { get; set; }
        #endregion

        public int CompareTo(Lesson other) => DateOfNest.CompareTo(other.DateOfNest);

        public override string ToString() =>
            $"{DateOfNest:ddd dd.MM.yy} {BeginLesson}-{EndLesson} {Discipline} ауд. {Auditorium}";
    }
}
