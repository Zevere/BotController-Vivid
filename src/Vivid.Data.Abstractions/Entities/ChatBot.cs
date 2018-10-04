using System;
using System.ComponentModel.DataAnnotations;

namespace Vivid.Data.Abstractions.Entities
{
    public class ChatBot
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Platform { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string Token { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
