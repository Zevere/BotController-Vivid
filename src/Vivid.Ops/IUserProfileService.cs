using System.Threading;
using System.Threading.Tasks;
using Zevere.Client.Models;

namespace Vivid.Ops
{
    /// <summary>
    /// Contains operations for user profiles
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Gets all registrations for a user by his username
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>A tuple containing user registrations and possible error</returns>
        Task<(User User, Error Error)> GetUserAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
