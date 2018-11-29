using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Zevere.Client.GraphQL
{
    /// <summary>
    /// Represent the response of a GraphQL request
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Response
    {
        /// <summary>
        /// The data of the response
        /// </summary>
        public JObject Data { get; set; }

        /// <summary>
        /// The Errors, if occurred
        /// </summary>
        public Error[] Errors { get; set; }

        /// <summary>
        /// <code>true</code> there are any errors, otherwise <code>false</code>
        /// </summary>
        public bool HasErrors => Errors?.Length > 0;
    }
}
