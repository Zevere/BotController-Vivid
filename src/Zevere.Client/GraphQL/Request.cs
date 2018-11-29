using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Zevere.Client.GraphQL
{
    /// <summary>
    /// Represents a GraphQL request body over HTTP
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Request
    {
        /// <summary>
        /// GraphQL's query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Optional query variables
        /// </summary>
        public object Variables { get; set; }
    }
}
