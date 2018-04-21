using System;

namespace RuzTermPaper.Models
{
    public abstract class Receiver
    {
        public abstract Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian);

    }
}
