using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RuzTermPaper.Tools
{
    public static class Json
    {
        public static async Task<T> ToObjectAsync<T>(string value) =>
            await Task.Run(() =>
            JsonConvert.DeserializeObject<T>(value,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));

        public static async Task<string> StringifyAsync<T>(T value) =>
            await Task.Run(() =>
            JsonConvert.SerializeObject(value, Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
    }
}
