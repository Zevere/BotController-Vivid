using System.Threading;
using System.Threading.Tasks;
using Zevere.Client.GraphQL;

namespace Zevere.Client
{
    /// <summary>
    /// Contains extension methods on <see cref="IZevereClient"/> for user profile operations
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Checks whether the user account exists by username
        /// </summary>
        /// <param name="client"><see cref="IZevereClient"/> instance</param>
        /// <param name="username">Username</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        public static Task<bool> UserExistsAsync(
            this IZevereClient client,
            string username,
            CancellationToken cancellationToken = default
        ) =>
            client.MakeRequestAsync(
                new Request
                {
                    Query = "query($u: String!) { user(userId: $u) { id } }",
                    Variables = new { u = username }
                },
                cancellationToken
            ).ContinueWith(
                t => t.Result.Data.Value<string>("user.id", null) != null,
                TaskContinuationOptions.OnlyOnRanToCompletion
            );

        /// <summary>
        /// Get a user profile by username
        /// </summary>
        /// <param name="client"><see cref="IZevereClient"/> instance</param>
        /// <param name="username">Username</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>User profile</returns>
        public static Task<Response> GetUserAsync(
            this IZevereClient client,
            string username,
            CancellationToken cancellationToken = default
        ) =>
            client.MakeRequestAsync(
                new Request
                {
                    Query = "query($u: String!) { user(userId: $u) " +
                            "{ id firstName lastName token daysJoined joinedAt } }",
                    Variables = new { u = username }
                },
                cancellationToken
            );
    }
}
