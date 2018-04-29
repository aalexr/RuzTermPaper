using Newtonsoft.Json;
using RuzTermPaper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{
    public class Lecturer : User
    {
        private const ReceiverType receivertype = ReceiverType.lecturerOid;
        public string chair { get; set; }
        public int chairOid { get; set; }
        public string fio { get; set; }
        public int lecturerOid { get; set; }
        public string shortFIO { get; set; }

        public bool Equals(Lecturer other) => lecturerOid == other.lecturerOid;

        /// <summary>
        /// Ищет преподавателя в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных преподавателей</returns>
        public static async Task<List<Lecturer>> FindLecturerAsync(string findText = "")
        {
            var requestUri = new Uri(BaseUri, $"lecturers?findtext={findText}");
            return JsonConvert.DeserializeObject<List<Lecturer>>(await App.Http.GetStringAsync(requestUri));
        }

        public override string ToString() => fio;

        protected override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype={(int)receivertype}&lecturerOid={lecturerOid}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => lecturerOid.Equals((other as Lecturer).lecturerOid);
    }
}
