using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RuzTermPaper.Tools;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Group : User
    {
        public int GroupOid { get; set; }

        public string Number { get; set; }

        public override Symbol Symbol => Symbol.People;

        /// <summary>
        /// Ищет группу в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список найденных групп</returns>
        public static async Task<List<Group>> FindAsync(string findText, CancellationToken cancellationToken)
        {
            var requestUri = new Uri(BaseUri, $"groups?findtext={findText}");
            var response = await App.Http.GetAsync(requestUri, cancellationToken);
            if (response.IsSuccessStatusCode)
                return await Json.ToObjectAsync<List<Group>>(await response.Content.ReadAsStringAsync(), cancellationToken);
            else
                throw new System.Net.Http.HttpRequestException($"Error Code {(int)response.StatusCode} - {response.ReasonPhrase}");
        }

        protected override Uri BuildUri(DateTime from, DateTime to)
        {
            var uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";
            uriBuilder.Query = $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype=3&groupOid={GroupOid}&language={(int)App.Language}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Group group && this.GroupOid.Equals(group.GroupOid);

        public override string ToString() => Number;
    }
}
