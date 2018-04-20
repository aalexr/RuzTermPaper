using Newtonsoft.Json;
using RuzTermPaper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{

    public class Lecturer : Receiver, IEquatable<Lecturer>
    {
        private const ReceiverType receivertype = ReceiverType.lecturerOid;
        public string chair { get; set; }
        public int chairOid { get; set; }
        public string fio { get; set; }
        public int lecturerOid { get; set; }
        public string shortFIO { get; set; }

        public override ReceiverType RType => receivertype;

        public override string Id { get => lecturerOid.ToString(); }

        public bool Equals(Lecturer other) => lecturerOid == other.lecturerOid;

        /// <summary>
        /// Ищет преподавателя в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных преподавателей</returns>
        public static async Task<List<Lecturer>> FindLecturerAsync(string findText = "")
        {
            var requestUri = new Uri(Lesson.baseUri, $"lecturers?findtext={findText}");
            return JsonConvert.DeserializeObject<List<Lecturer>>(await App.http.GetStringAsync(requestUri));
        }

        public override string ToString() => fio;

        public override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(Lesson.baseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from.ToString("yyyy.MM.dd")}&todate={to.ToString("yyyy.MM.dd")}&receivertype={(int)RType}&{RType}={Id}";

            return uriBuilder.Uri;
        }
    }
}
