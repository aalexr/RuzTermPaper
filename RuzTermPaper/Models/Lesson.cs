using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{
    public class Lesson

    {
        public static readonly Uri baseUri = new Uri("http://ruz.hse.ru/RUZService.svc/");
        private static Uri requestUri;

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
        /// Ищет преподавателя в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных преподавателей</returns>
        public static async Task<IList<Lecturer>> FindLecturerAsync(string findText)
        {
            requestUri = new Uri(baseUri, $"lecturers?findtext={findText}");
            return JsonConvert.DeserializeObject<IList<Lecturer>>(await App.http.GetStringAsync(requestUri));
        }

        /// <summary>
        /// Ищет группу в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных групп</returns>
        public static async Task<IList<Group>> FindGroupAsync(string findText)
        {
            requestUri = new Uri(baseUri, $"groups?findtext={findText}");
            return await Json.ToObjectAsync<IList<Group>>(await App.http.GetStringAsync(requestUri));
        }

        /// <summary>
        /// Возвращает список занятий по заданному URI
        /// </summary>
        /// <param name="request">Ссылка</param>
        /// <returns>Список найденных занятий</returns>
        public static async Task<IList<Lesson>> GetTimetable(Uri request)
        {
            return await Json.ToObjectAsync<IList<Lesson>>(await App.http.GetStringAsync(request));
        }

        /// <summary>
        /// Создает URI-запрос
        /// </summary>
        /// <param name="o">Для кого</param>
        /// <param name="from">Откуда</param>
        /// <param name="to">Куда</param>
        /// <param name="lang">Язык</param>
        /// <returns></returns>
        public static Uri BuildUri(object o, DateTime from, DateTime to, Language lang = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(baseUri);
            uriBuilder.Path += "personlessons";

            switch (o)
            {
                case Group G:
                    uriBuilder.Query = $"fromdate={from.ToString("yyyy.MM.dd")}" +
                $"&todate={to.ToString("yyyy.MM.dd")}" +
                $"&receivertype={RuzTermPaper.Group.receivertype}" +
                $"&groupOid={G.groupOid}";
                    break;

                case Lecturer L:
                    uriBuilder.Query = $"fromdate={from.ToString("yyyy.MM.dd")}" +
                $"&todate={to.ToString("yyyy.MM.dd")}" +
                $"&receivertype={RuzTermPaper.Lecturer.receivertype}" +
                $"&lecturerOid={L.lecturerOid}";
                    break;

                case string email:
                    if (string.IsNullOrEmpty(email))
                        goto default;

                    uriBuilder.Query = $"fromdate={from.ToString("yyyy.MM.dd")}" +
                $"&todate={to.ToString("yyyy.MM.dd")}" +
                $"&receivertype=0" +
                $"&email=" + email;
                    break;

                default:
                    throw new ArgumentException();
            }

            return uriBuilder.Uri;
        }
        #endregion

        public override string ToString() =>
            $"{DayOfWeekString} {DateOfNest.ToString("dd.MM.yy")} {BeginLesson}-{EndLesson} {Discipline} ауд. {Auditorium}";
    }
}
