using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    public class Lecturer : User
    {
        public string Fio { get; set; }

        public int LecturerOid { get; set; }

        public override Symbol Symbol => Symbol.Contact;

        /// <summary>
        /// Ищет преподавателя в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных преподавателей</returns>
        public static async Task<List<Lecturer>> FindAsync(string findText, CancellationToken cancellationToken)
        {
            var requestUri = new Uri(BaseUri, $"lecturers?findtext={findText}");
            var response = await App.Http.GetAsync(requestUri, cancellationToken);
            if (response.IsSuccessStatusCode)
                return await Json.ToObjectAsync<List<Lecturer>>(await response.Content.ReadAsStringAsync(), cancellationToken);
            else
                throw new System.Net.Http.HttpRequestException($"Error Code {(int)response.StatusCode} - {response.ReasonPhrase}");
        }

        public override string ToString() => Fio;

        protected override Uri BuildUri(DateTime from, DateTime to)
        {
            var uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype=1&lecturerOid={LecturerOid}&language={(int)App.Language}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Lecturer lecturer && this.LecturerOid.Equals(lecturer.LecturerOid);
    }
}
