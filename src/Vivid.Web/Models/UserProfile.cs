using System;
using System.ComponentModel.DataAnnotations;
using Zevere.Client.Models;

namespace Vivid.Web.Models
{
    /// <summary>
    /// Profile of a Zevere user
    /// </summary>
    public class UserProfile
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; }

        public static explicit operator UserProfile(User zvUser) =>
            zvUser == null
                ? null
                : new UserProfile
                {
                    Id = zvUser.Id,
                    FirstName = zvUser.FirstName,
                    Token = zvUser.Token,
                    JoinedAt = zvUser.JoinedAt,
                    LastName = zvUser.LastName,
                };
    }
}
