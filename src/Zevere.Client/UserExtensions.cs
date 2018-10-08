using System.Threading;
using System.Threading.Tasks;
using Zevere.Client.GraphQL;

namespace Zevere.Client
{
    public static class UserExtensions
    {
        public static Task<bool> UserExists(
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
    }
}
