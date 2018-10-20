using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Zevere.Client.GraphQL
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Request
    {
        public string Query { get; set; }

        public object Variables { get; set; }
    }
}
