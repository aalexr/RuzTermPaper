using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;
using System.Runtime.Serialization;

namespace RuzTermPaper.Models
{
    public abstract class User : IEquatable<User>
    {
        [JsonIgnore]
        /// <summary>
        /// Символ пользователя
        /// </summary>
        public abstract Symbol Symbol { get; }

        /// <summary>
        /// Базовое URI
        /// </summary>
        protected static readonly Uri BaseUri = new Uri("http://92.242.58.221/RUZService.svc/");
        [JsonIgnore]
        public string Name => ToString();

        /// <summary>
        /// Создает URI для запроса расписания
        /// </summary>
        /// <param name="from">Начальная дата</param>
        /// <param name="to">Конечная дата</param>
        /// <param name="language">Язык расписания</param>
        /// <returns></returns>
        protected abstract Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian);

        /// <summary>
        /// Получает и группирует по дате занятия
        /// </summary>
        /// <param name="from">Начальная дата</param>
        /// <param name="period">Количество дней начиная с from</param>
        /// <param name="language">Язык расписания</param>
        /// <returns></returns>
        public async Task<IEnumerable<LessonsGroup>> GetLessonsAsync(DateTime from, int period, Language language = Language.Russian) =>
            await GetLessonsAsync(from, from.AddDays(period), language);

        /// <summary>
        /// Получает и группирует по дате занятия
        /// </summary>
        /// <param name="from">Начальная дата</param>
        /// <param name="to">Конечная дата</param>
        /// <param name="language">Язык расписания</param>
        /// <returns></returns>
        public async Task<IEnumerable<LessonsGroup>> GetLessonsAsync(DateTime from, DateTime to, Language language = Language.Russian)
        {
            List<Lesson> list =
                await Json.ToObjectAsync<List<Lesson>>
                (await App.Http.GetStringAsync(BuildUri(from, to, language)));
            list = list.OrderBy(L => L.DateOfNest).ToList();
            List<LessonsGroup> res = new List<LessonsGroup>();
            for (var i = from; i < to; i = i.AddDays(1))
                res.Add(new LessonsGroup(i, list.Where(x => x.DateOfNest == i)));

            return res;
        }

        public abstract bool Equals(User other);
    }
}
