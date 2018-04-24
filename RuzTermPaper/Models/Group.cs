using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuzTermPaper.Tools;

namespace RuzTermPaper.Models
{

    public class Group : Receiver, IEquatable<Group>
    {
        private const ReceiverType receivertype = ReceiverType.groupOid;
        public int chairOid { get; set; }
        public int course { get; set; }
        public string faculty { get; set; }
        public int facultyOid { get; set; }
        public string formOfEducation { get; set; }
        public int groupOid { get; set; }
        public string number { get; set; }
        public string speciality { get; set; }

        public bool Equals(Group other) => groupOid == other.groupOid;

        /// <summary>
        /// Ищет группу в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных групп</returns>
        public static async Task<List<Group>> FindAsync(string findText = "")
        {
            var requestUri = new Uri(Lesson.BaseUri, $"groups?findtext={findText}");
            return await Json.ToObjectAsync<List<Group>>(await App.Http.GetStringAsync(requestUri));
        }

        public override string ToString() => number;

        public override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(Lesson.BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype={(int)receivertype}&groupOid={groupOid}";

            return uriBuilder.Uri;
        }
    }
}
