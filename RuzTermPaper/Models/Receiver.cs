using System;

namespace RuzTermPaper.Models
{
    public abstract class Receiver
    {
        public abstract ReceiverType RType { get; }
        public abstract string Id { get; }
        public abstract Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian);

    }
}
