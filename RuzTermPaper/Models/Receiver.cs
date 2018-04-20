namespace RuzTermPaper.Models
{
    public abstract class Receiver
    {
        abstract public ReceiverType RType { get; }
        abstract public object Id { get; set; }
    }
}
