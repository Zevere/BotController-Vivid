using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    /// <summary>
    /// Contains arguments for associating a Zevere user to his account on a chat platform
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class NewUserRegistration
    {
        /// <summary>
        /// ID of the Zevere user account
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        /// <summary>
        /// Unique identifier of the user on the chat platform
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ChatUserId { get; set; }
    }
}