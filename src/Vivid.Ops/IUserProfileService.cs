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
        /// Get a user by name
        /// </summary>
        /// <param name="username">Name of user</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>User or an error, if occured</returns>
        Task<(User User, Error Error)> GetUserAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
