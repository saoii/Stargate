using System.Text;
using System.Text.Json;

namespace Stargate.Services.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true // Set this to true if you want pretty-printed JSON
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static HttpContent GetJsonContent<T>(this T obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Set this to true if you want pretty-printed JSON
            };

            var json = JsonSerializer.Serialize(obj, options); ;

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static T FromJson<T>(this string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
