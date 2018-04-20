using RuzTermPaper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [JsonIgnore]
        public override ReceiverType RType => receivertype;

        public override object Id { get => groupOid; set => groupOid = (int)value; }

        public bool Equals(Group other) => groupOid == other.groupOid;

        /// <summary>
        /// Ищет группу в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных групп</returns>
        public static async Task<List<Group>> FindGroupAsync(string findText = "")
        {
            var requestUri = new Uri(Lesson.baseUri, $"groups?findtext={findText}");
            return await Json.ToObjectAsync<List<Group>>(await App.http.GetStringAsync(requestUri));
        }

        public override string ToString() => number;
    }
}
