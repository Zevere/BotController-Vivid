using System;
using System.ComponentModel.DataAnnotations;

namespace Vivid.Data.Entities
{
    /// <summary>
    /// Represents a chatbot document in the Mongo collection
    /// </summary>
    public class ChatBot
    {
        /// <summary>
        /// Unique identifier of the document
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique name of the chatbot
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The chat platform
        /// </summary>
        [Required]
        public string Platform { get; set; }

        /// <summary>
        /// Chatbot's webhook URL
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// Authentication token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// The document creation time
        /// </summary>
        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
