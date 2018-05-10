using System.Collections.Generic;

namespace RuzTermPaper.Tools
{
    public static class Extensions
    {
        /// <summary>
        /// Добавляет элемент, если он отсутствует в списке
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="item"></param>
        public static void AddIfNew<T>(this IList<T> container, T item)
        {
            if (!container.Contains(item))
                container.Add(item);
        }
    }
}
