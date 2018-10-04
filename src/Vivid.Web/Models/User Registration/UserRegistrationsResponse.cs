using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class UserRegistrationsResponse
    {
        [JsonProperty(Required = Required.Always)]
        public UserRegistration[] Registrations { get; set; }
    }
}