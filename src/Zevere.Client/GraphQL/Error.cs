using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Zevere.Client.GraphQL
{
    /// <summary>
    /// Represents the error in an GraphQL operation
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Error
    {
        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The Location of an error
        /// </summary>
        public Location[] Locations { get; set; }

        /// <summary>
        /// Additional error entries
        /// </summary>
        public JObject AdditionalEntries { get; set; }
    }
}
