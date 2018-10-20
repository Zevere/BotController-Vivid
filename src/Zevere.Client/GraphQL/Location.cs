using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Zevere.Client.GraphQL
{
    /// <summary>
    /// Represents the location where the <see cref="Error"/> has been found
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Location
    {
        /// <summary>
        /// The Column
        /// </summary>
        public uint Column { get; set; }

        /// <summary>
        /// The Line
        /// </summary>
        public uint Line { get; set; }
    }
}
