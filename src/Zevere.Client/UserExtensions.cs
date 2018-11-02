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
        /// Get a user profile by his username
        /// </summary>
        /// <param name="client">Instance of Zevere client</param>
        /// <param name="username">Username to query for</param>
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
