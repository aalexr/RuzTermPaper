using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{
    public class Lesson
    {
        public static readonly Uri baseUri = new Uri("http://ruz.hse.ru/RUZService.svc/");

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

        #region Static Methods

        /// <summary>
        /// Возвращает список занятий по заданному URI
        /// </summary>
        /// <param name="request">Ссылка</param>
        /// <returns>Список найденных занятий</returns>
        public static async Task<List<Lesson>> GetLessons(Uri request)
        {
            return await Json.ToObjectAsync<List<Lesson>>(await App.http.GetStringAsync(request));
        }

        /// <summary>
        /// Создает URI-запрос
        /// </summary>
        /// <param name="t">Для кого</param>
        /// <param name="from">Откуда</param>
        /// <param name="to">Куда</param>
        /// <param name="lang">Язык</param>
        /// <returns></returns>
        public static Uri BuildUri<T>(T t, DateTime from, DateTime to, Language lang = Language.Russian) where T : IReceiver
        {
            UriBuilder uriBuilder = new UriBuilder(baseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from.ToString("yyyy.MM.dd")}&todate={to.ToString("yyyy.MM.dd")}&receivertype={(int)t.type}&{t.type}={t.Id}";

            return uriBuilder.Uri;
        }
        #endregion

        public override string ToString() =>
            $"{DayOfWeekString} {DateOfNest.ToString("dd.MM.yy")} {BeginLesson}-{EndLesson} {Discipline} ауд. {Auditorium}";
    }
}
