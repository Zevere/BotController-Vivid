using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Error
    {
        [JsonProperty(Required = Required.Always)]
        public string Code { get; set; }

        public string Message { get; set; }

        public string Hint { get; set; }
    }
}