using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuzTermPaper.Models
{
    public abstract class User : IEquatable<User>
    {
        protected static readonly Uri BaseUri = new Uri("http://ruz.hse.ru/RUZService.svc/");
        protected abstract Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian);
        public async Task<List<LessonsGroup>> GetLessonsAsync(DateTime from, DateTime to, Language language = Language.Russian)
        {
            List<Lesson> list = await Json.ToObjectAsync<List<Lesson>>(await App.Http.GetStringAsync(BuildUri(from, to, language)));
            return list.GroupBy(L => L.DateOfNest).Select(G => new LessonsGroup(G.Key, G)).ToList();
        }

        public abstract bool Equals(User other);
    }
}
