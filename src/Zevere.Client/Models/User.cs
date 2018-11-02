using System;

namespace Zevere.Client.Models
{
    public class User
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }

        public int DaysJoined { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
