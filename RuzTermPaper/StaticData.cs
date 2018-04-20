using RuzTermPaper.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace RuzTermPaper
{
    static class StaticData
    {
        public static IOrderedEnumerable<IGrouping<System.DateTime, Lesson>> Lessons { get; set; }
    }
}
