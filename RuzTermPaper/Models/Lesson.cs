using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{
    public class Lesson : IDisposable
    {
        public static readonly string baseUri = @"http://ruz.hse.ru/RUZService.svc/";
        private static string requestUri;
        private static System.Net.Http.HttpClient http;

        #region Public Fields
        public string Auditorium { get; set; }
        public int AuditoriumOid { get; set; }
        public string BeginLesson { get; set; }
        public string Building { get; set; }
        public string Date { get; set; }
        public System.DateTime DateOfNest { get; set; }
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
        /// Ищет преподавателя в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns></returns>
        public async static Task<IList<Lecturer>> FindLecturerAsync(string findText)
        {
            requestUri = $"{baseUri}lecturers?findtext={findText}";
            return JsonConvert.DeserializeObject<IList<Lecturer>>(await http.GetStringAsync(requestUri));
        }

        /// <summary>
        /// Ищет группу в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns></returns>
        public async static Task<IList<Group>> FindGroupAsync(string findText)
        {
            requestUri = $"{baseUri}groups?findtext={findText}";
            return await Json.ToObjectAsync<IList<Group>>(await http.GetStringAsync(requestUri));
        }

        /// <summary>
        /// Возвращает список занятий по заданному URI
        /// </summary>
        /// <param name="request">Ссылка</param>
        /// <returns></returns>
        public async static Task<IList<Lesson>> GetTimetable(string request) => await Json.ToObjectAsync<IList<Lesson>>(await http.GetStringAsync(request));

        public static string ConstructRequest(ReceiverType who, DateTime from, DateTime to, string email, Language lang = Language.Russian)
        {
            return $"{baseUri}personlessons?fromdate={from.ToString("yyyy.MM.dd")}&todate={to.ToString("yyyy.MM.dd")}&receivertype={(int)who}&email={email}&language={(int)lang}";
        }
        #endregion

        public override string ToString() =>
            $"{DayOfWeekString} {DateOfNest.Day:D2}.{DateOfNest.Month:D2}.{DateOfNest.Year % 100} {BeginLesson}-{EndLesson} {Discipline} ауд. {Auditorium}";

        public void Dispose() => http.Dispose();
    }
}
