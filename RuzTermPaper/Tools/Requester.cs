using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuzTermPaper
{
    class Requester : IDisposable
    {
        public static readonly string baseUri = @"http://ruz.hse.ru/RUZService.svc/";
        private string requestUri;
        private System.Net.Http.HttpClient http;

        public Requester() => http = new System.Net.Http.HttpClient();

        public async Task<IList<Lecturer>> FindLecturerAsync(string findText)
        {
            requestUri = $"{baseUri}lecturers?findtext={findText}";
            return JsonConvert.DeserializeObject<IList<Lecturer>>(await http.GetStringAsync(requestUri));
        }

        public async Task<IList<Group>> FindGroupAsync(string findText)
        {
            requestUri = $"{baseUri}groups?findtext={findText}";
            return await Json.ToObjectAsync<IList<Group>>(await http.GetStringAsync(requestUri));
        }

        public async Task<IList<Models.Lesson>> GetTimetable (string request)
        {
            return await Json.ToObjectAsync<IList<Models.Lesson>>(await http.GetStringAsync(request));
        }

        public string ConstructRequest(Models.ReceiverType who, DateTime from, DateTime to, string email, Models.Language lang = Models.Language.Russian)
        {
            return $"{baseUri}personlessons?fromdate={from.ToString("yyyy.MM.dd")}&todate={to.ToString("yyyy.MM.dd")}&receivertype={(int)who}&email={email}&language={(int)lang}";
        }

        public void Dispose() => http.Dispose();
    }
}
