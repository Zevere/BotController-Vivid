using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Abstractions
{
    /// <summary>
    /// Contains operations to work with a user registration collection
    /// </summary>
    public interface IUserRegistrationRepository
    {
        /// <summary>
        /// Creates a new user registration document
        /// </summary>
        /// <param name="registration">Registration to be added</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <exception cref="DuplicateKeyException"></exception>
        Task AddAsync(
            Registration registration,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Get a list of all registrations for a user. List could be empty.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>List of registrations</returns>
        Task<IEnumerable<Registration>> GetAllForUserAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
