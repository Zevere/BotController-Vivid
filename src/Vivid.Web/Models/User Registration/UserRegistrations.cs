using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vivid.Data.Abstractions.Entities;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    /// <summary>
    /// Contains information of a user and the bot connected to him on a specific chat platform
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class UserRegistration
    {
        // ToDo User enum with swagger to allow only specific names
        /// <summary>
        /// Name of the chat platform
        /// </summary>
        [Required]
        public string Platform { get; set; }

        /// <summary>
        /// Unique identifier of the bot that registered that user
        /// </summary>
        [Required]
        public string BotId { get; set; }

        /// <summary>
        /// Unique identifier of the user on that chat platform
        /// </summary>
        [Required]
        public string ChatUserId { get; set; }

        /// <summary>
        /// Convert from a tuple of Registration and ChatBot
        /// </summary>
        public static explicit operator UserRegistration((Registration Reg, ChatBot Bot) tuple) =>
            new UserRegistration
            {
                Platform = tuple.Bot.Platform,
                ChatUserId = tuple.Reg.ChatUserId,
                BotId = tuple.Bot.Name,
            };
    }
}
