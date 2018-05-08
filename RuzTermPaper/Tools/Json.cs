using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RuzTermPaper.Tools
{
    public static class Json
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };

        public static async Task<T> ToObjectAsync<T>(string value, CancellationToken? cancellation = null)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(value, settings), cancellation ?? CancellationToken.None);
        }

        public static async Task<string> StringifyAsync<T>(T value, CancellationToken? cancellation = null)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(value, Formatting.Indented, settings), cancellation ?? CancellationToken.None);
        }
    }
}
