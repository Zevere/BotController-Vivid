using System;

namespace Zevere.Client.Models
{
    /// <summary>
    /// Zevere user account
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique username
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Authentication token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Number of the days this user has been created(joined)
        /// </summary>
        public int DaysJoined { get; set; }

        /// <summary>
        /// The date that user account was created at
        /// </summary>
        public DateTime JoinedAt { get; set; }
    }
}
