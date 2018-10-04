using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ErrorResponse
    {
        [JsonProperty(Required = Required.Always)]
        public Error[] Errors { get; set; }
    }
}