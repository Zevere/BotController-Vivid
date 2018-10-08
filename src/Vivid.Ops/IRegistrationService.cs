using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vivid.Data.Entities;

namespace Vivid.Ops
{
    /// <summary>
    /// Contains operations for connecting a user to a chat bot
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Registers a user with the bot
        /// </summary>
        /// <param name="botName">Name of the bot</param>
        /// <param name="username">Name of the user</param>
        /// <param name="chatUserId">Unique user identifier on the chat platform</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>Null on success or an error, if one occurs</returns>
        Task<Error> RegisterUserAsync(
            string botName,
            string username,
            string chatUserId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets all registrations for a user by his username
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>A tuple containing user registrations and possible error</returns>
        Task<(IEnumerable<(Registration, ChatBot)> Registrations, Error Error)> GetUserRegistrationsAsync(
            string username,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes registration of a user with the bot
        /// </summary>
        /// <param name="botName">Name of the bot</param>
        /// <param name="username">Name of the user</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>Null on success or an error, if one occurs</returns>
        Task<Error> DeleteUserRegistrationAsync(
            string botName,
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
