using Newtonsoft.Json;
using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    public abstract class User : IEquatable<User>
    {
        [JsonIgnore]
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
        public async Task<ObservableCollection<LessonsGroup>> GetLessonsAsync(DateTime from, int period, Language language = Language.Russian) =>
            await GetLessonsAsync(from, from.AddDays(period), language);

        /// <summary>
        /// Получает и группирует по дате занятия
        /// </summary>
        /// <param name="from">Начальная дата</param>
        /// <param name="to">Конечная дата</param>
        /// <param name="language">Язык расписания</param>
        /// <returns></returns>
        public async Task<ObservableCollection<LessonsGroup>> GetLessonsAsync(DateTime from, DateTime to, Language language = Language.Russian)
        {
            var list =
                await Json.ToObjectAsync<List<Lesson>>
                (await App.Http.GetStringAsync(BuildUri(from, to, language)));
            list = list.OrderBy(L => L.DateOfNest).ToList();
            var res = new List<LessonsGroup>(list.Count);
            for (var i = from; i < to; i = i.AddDays(1))
                res.Add(new LessonsGroup(i, list.Where(x => x.DateOfNest == i)));

            return new ObservableCollection<LessonsGroup>(res);
        }

        public abstract bool Equals(User other);
    }
}
