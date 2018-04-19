namespace RuzTermPaper.Models
{
    public interface IReceiver
    {
        ReceiverType type { get; }
        object Id { get; set; }
    }
}
