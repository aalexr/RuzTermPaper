using System;

namespace RuzTermPaper.Models
{
    public abstract class Receiver : IEquatable<Receiver>
    {
        protected static readonly Uri BaseUri = new Uri("http://ruz.hse.ru/RUZService.svc/");
        public abstract Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian);

        public abstract bool Equals(Receiver other);
    }
}
