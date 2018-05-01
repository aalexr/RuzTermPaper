using System;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    public class Student : User
    {
        public string fio { get; set; }
        public string shortFIO { get; set; }
        public int studentOid { get; set; }
        public string Email { get; set; }

        public Student(string email) => Email = email;

        public override Symbol Symbol => Symbol.Account;

        protected override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query =
                $"fromdate={from:yyyy.M.d}&todate={to:yyyy.M.d}&receivertype=0&email={Email}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Student student && this.Email.Equals(student.Email);

        public override string ToString() => Email;
    }
}
