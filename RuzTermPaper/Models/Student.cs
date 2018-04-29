using System;

namespace RuzTermPaper.Models
{
    public class Student : User
    {
        private const ReceiverType receivertype = ReceiverType.email;
        private string email;

        public Student(string email)
        {
            if (email.EndsWith("@edu.hse.ru"))
                this.email = email;
            else
            {
                throw new ArgumentException();
            }
        }

        public string Email => email;

        protected override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query =
                $"fromdate={from:yyyy.MM.dd}&todate={to:yyyy.MM.dd}&receivertype={(int)receivertype}&email={email}";

            return uriBuilder.Uri;
        }

        public override bool Equals(User other) => other is Student student && email.Equals(student.email);

        public override string ToString() => Email;
    }
}
