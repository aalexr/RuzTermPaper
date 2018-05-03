using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
// ReSharper disable MemberCanBePrivate.Global

namespace RuzTermPaper.Models
{
    public class Lecturer : User
    {
        //public string chair { get; set; }
        //public int chairOid { get; set; }
        public string Fio { get; set; }
        public int LecturerOid { get; set; }
/*
        public string shortFIO { get; set; }
*/

        public override Symbol Symbol => Symbol.Contact;

        /// <summary>
        /// Ищет преподавателя в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных преподавателей</returns>
        public static async Task<List<Lecturer>> FindAsync(string findText = "")
        {
            var requestUri = new Uri(BaseUri, $"lecturers?findtext={findText}");
            return JsonConvert.DeserializeObject<List<Lecturer>>(await App.Http.GetStringAsync(requestUri));
        }

        public override string ToString() => Fio;

        protected override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            var uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype=1&lecturerOid={LecturerOid}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Lecturer lecturer && this.LecturerOid.Equals(lecturer.LecturerOid);
    }
}
