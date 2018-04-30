using System;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    public class Student : User
    {
        public Student(string email) => Email = email;

        public string Email { get; set; }

        public override Symbol Symbol => Symbol.Account;

        protected override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query =
                $"fromdate={from:yyyy.M.d}&todate={to:yyyy.M.d}&receivertype=0&email={Email}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Student student && Email.Equals(student.Email);

        public override string ToString() => Email;
    }
}
