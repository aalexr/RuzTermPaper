namespace RuzTermPaper.Models
{
    /// <summary>
    /// Тип получателя расписания
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// Расписание для преподавателя
        /// </summary>
        Lecturer,
        /// <summary>
        /// Расписание для группы
        /// </summary>
        Group
    }

    /// <summary>
    /// Язык полученного расписания
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// Русский язык
        /// </summary>
        Russian,
        /// <summary>
        /// Английский язык
        /// </summary>
        English = 2
    }
}
