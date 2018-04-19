using Newtonsoft.Json;
using RuzTermPaper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{

    public class Lecturer : IEquatable<Lecturer>, IReceiver
    {
        private const ReceiverType receivertype = ReceiverType.lecturerOid;
        public string chair { get; set; }
        public int chairOid { get; set; }
        public string fio { get; set; }
        public int lecturerOid { get; set; }
        public string shortFIO { get; set; }

        [JsonIgnore]
        public ReceiverType type => receivertype;
        [JsonIgnore]
        public object Id { get => lecturerOid; set => lecturerOid = (int)value; }

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
    }
}
