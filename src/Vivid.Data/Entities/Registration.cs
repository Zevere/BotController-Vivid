using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace Vivid.Data.Entities
{
    /// <summary>
    /// Represents a user registration document in the Mongo collection
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// Unique identifier of the document
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Unique name of the Zevere user
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Unique identifier of the user's chat account
        /// </summary>
        [Required]
        public string ChatUserId { get; set; }

        /// <summary>
        /// Registration time
        /// </summary>
        [Required]
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// MongoDB reference to the chat bot
        /// </summary>
        [Required]
        public MongoDBRef ChatBotDbRef { get; set; }
    }
}
