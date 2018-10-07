using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Ops
{
    /// <summary>
    /// Contains operations for connecting a user to a chat bot
    /// </summary>
    public interface IRegistrationService
    {
        Task<Error> RegisterUserAsync(
            string bot,
            string username,
            string chatUserId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets all registrations for a user by his username
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>A tuple containing user registrations and possible error</returns>
        Task<(IEnumerable<(Registration, ChatBot)> Registrations, Error Error)> GetUserRegistrationsAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
