using System;

namespace RuzTermPaper.Models
{
    public class Student : Receiver, IEquatable<Student>
    {
        private const ReceiverType receivertype = ReceiverType.email;
        private string email;

        public override string Id { get => email; }

        public override ReceiverType RType => receivertype;

        public override Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian)
        {
            UriBuilder uriBuilder = new UriBuilder(Lesson.baseUri);
            uriBuilder.Path += "personlessons";

            uriBuilder.Query = $"fromdate={from.ToString("yyyy.MM.dd")}&todate={to.ToString("yyyy.MM.dd")}&receivertype={(int)RType}&{RType}={Id}";

            return uriBuilder.Uri;
        }

        public bool Equals(Student other) => email.Equals(other.email);
    }
}
