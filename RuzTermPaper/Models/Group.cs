using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuzTermPaper.Tools;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Group : User
    {
        //public int chairOid { get; set; }
        //public int course { get; set; }
        //public string faculty { get; set; }
        //public int facultyOid { get; set; }
        //public string formOfEducation { get; set; }
        public int GroupOid { get; set; }
        public string Number { get; set; }
        //public string speciality { get; set; }

        public override Symbol Symbol => Symbol.People;

        /// <summary>
        /// Ищет группу в базе РУЗ по тексту
        /// </summary>
        /// <param name="findText">Текст для поиска</param>
        /// <returns>Список найденных групп</returns>
        public static async Task<List<Group>> FindAsync(string findText = "")
        {
            var requestUri = new Uri(BaseUri, $"groups?findtext={findText}");
            return await Json.ToObjectAsync<List<Group>>(await App.Http.GetStringAsync(requestUri));
        }

        public override string ToString() => Number;

        protected override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            var uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";
            uriBuilder.Query = $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype=3&groupOid={GroupOid}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Group group && this.GroupOid.Equals(group.GroupOid);
    }
}
