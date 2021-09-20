using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JudgeSystem.Common.Extensions
{
    public static class JsonExtensions
    {
        private static JsonSerializer JsonSerializer
            => JsonSerializer.Create(JsonSerializerSettings);

        private static JsonSerializerSettings JsonSerializerSettings
            => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
            };

        public static object FromJson(this string json)
            => JsonConvert.DeserializeObject(json, JsonSerializerSettings);

        public static T FromJson<T>(this string json)
            => JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);

        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj, JsonSerializerSettings);
    }
}
