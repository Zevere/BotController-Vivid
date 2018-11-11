using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Zevere.Client.Models;

namespace Vivid.Web.Models
{
    /// <summary>
    /// Profile of a Zevere user
    /// </summary>
    [JsonObject(
        NamingStrategyType = typeof(CamelCaseNamingStrategy),
        ItemNullValueHandling = NullValueHandling.Ignore
    )]
    public class UserProfile
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// User first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The date account was created in UTC
        /// </summary>
        [Required]
        public DateTime JoinedAt { get; set; }

        /// <summary>
        /// Converts a Zevere user to a user profile on the BotOps API
        /// </summary>
        public static explicit operator UserProfile(User zvUser) =>
            zvUser == null
                ? null
                : new UserProfile
                {
                    Id = zvUser.Id,
                    FirstName = zvUser.FirstName,
                    JoinedAt = zvUser.JoinedAt,
                    LastName = zvUser.LastName,
                };
    }
}
