namespace RuzTermPaper.Models
{
    /// <summary>
    /// Тип получателя расписания
    /// </summary>
    public enum ReceiverType { email, lecturerOid, groupOid = 3 }

    /// <summary>
    /// Язык полученного расписания
    /// </summary>
    public enum Language { Russian, English = 2 }
}
