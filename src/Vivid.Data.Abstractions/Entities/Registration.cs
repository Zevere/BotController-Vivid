using System;
using System.ComponentModel.DataAnnotations;

namespace Vivid.Data.Abstractions.Entities
{
    public class Registration
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string ChatUserId { get; set; }

        [Required]
        public string ChatBotId { get; set; }

        [Required]
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
