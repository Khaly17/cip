using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gefco.CipQuai
{
    /// <summary>
    ///     Classe contenant des méthodes statiques pour sérialiser / désérialiser des données.
    /// </summary>
    public static class HelperJson
    {
        private static JsonSerializerSettings jsonSettings;

        public static JsonSerializerSettings JsonSettings => jsonSettings ?? (jsonSettings = new JsonSerializerSettings
        {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            DefaultValueHandling = DefaultValueHandling.Ignore
        });

        /// <summary>
        ///     Serialize the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, JsonSettings);
        }

        /// <summary>
        ///     Deserialize the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The data of T.</returns>
        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, JsonSettings);
        }

        /// <summary>
        ///     Deserializes the collection.
        /// </summary>
        /// <returns>The collection.</returns>
        /// <param name="data">The data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The collection data of T.</returns>
        public static List<T> DeserializeCollection<T>(string data)
        {
            return JsonConvert.DeserializeObject<List<T>>(data, JsonSettings);
        }
    }
}