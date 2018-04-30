﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuzTermPaper.Tools;

namespace RuzTermPaper.Models
{
    public class Lesson : IComparable<Lesson>
    {
        #region Public Fields
        public string Auditorium { get; set; }
        public int AuditoriumOid { get; set; }
        public string BeginLesson { get; set; }
        public string Building { get; set; }
        public string Date { get; set; }
        public DateTime DateOfNest { get; set; }
        public int DayOfWeek { get; set; }
        public string DayOfWeekString { get; set; }
        public string DetailInfo { get; set; }
        public string Discipline { get; set; }
        public string Disciplineinplan { get; set; }
        public int Disciplinetypeload { get; set; }
        public string EndLesson { get; set; }
        public object Group { get; set; }
        public int GroupOid { get; set; }
        public bool IsBan { get; set; }
        public string KindOfWork { get; set; }
        public string Lecturer { get; set; }
        public int LecturerOid { get; set; }
        public string Stream { get; set; }
        public int StreamOid { get; set; }
        public object SubGroup { get; set; }
        public int SubGroupOid { get; set; }
        #endregion

        /// <summary>
        /// Возвращает список занятий по заданному URI
        /// </summary>
        /// <param name="request">Ссылка</param>
        /// <returns>Список найденных занятий</returns>
        public static async Task<List<Lesson>> GetLessonsAsync(Uri request) =>
            await Json.ToObjectAsync<List<Lesson>>(await App.Http.GetStringAsync(request));

        public int CompareTo(Lesson other) => DateOfNest.CompareTo(other.DateOfNest);

        public override string ToString() =>
            $"{DayOfWeekString} {DateOfNest:dd.MM.yy} {BeginLesson}-{EndLesson} {Discipline} ауд. {Auditorium}";
    }
}
