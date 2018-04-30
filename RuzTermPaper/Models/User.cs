﻿using RuzTermPaper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RuzTermPaper.Models
{
    public abstract class User : IEquatable<User>
    {
        public abstract Symbol Symbol { get; }
        protected static readonly Uri BaseUri = new Uri("http://92.242.58.221/RUZService.svc/");
        public string Name => ToString();
        protected abstract Uri BuildUri(DateTime from, DateTime to, Language language = Language.Russian);
        public async Task<List<LessonsGroup>> GetLessonsAsync(DateTime from, int period, Language language = Language.Russian) =>
            await GetLessonsAsync(from, from.AddDays(period), language);
        public async Task<List<LessonsGroup>> GetLessonsAsync(DateTime from, DateTime to, Language language = Language.Russian)
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