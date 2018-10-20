using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    /// <summary>
    /// Contains a list of associations of a Zevere account with chat platforms
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class UserRegistrationsResponse
    {
        /// <summary>
        /// ID of the Zevere user account
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        /// <summary>
        /// List of user registrations
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [MinLength(1)]
        public UserRegistration[] Registrations { get; set; }
    }
}
