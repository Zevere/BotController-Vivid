using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Zevere.Client;
using Zevere.Client.Models;

namespace Vivid.Ops
{
    /// <inheritdoc />
    public class UserProfileService : IUserProfileService
    {
        private readonly IZevereClient _zvClient;

        private readonly ILogger _logger;

        /// <inheritdoc />
        public UserProfileService(
            IZevereClient zvClient,
            ILogger logger = default
        )
        {
            _zvClient = zvClient;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<(User User, Error Error)> GetUserAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            (User User, Error Error) result;

            var resp = await _zvClient.GetUserAsync(username, cancellationToken)
                .ConfigureAwait(false);

            if (resp.HasErrors)
            {
                // ToDo
                result = (null, new Error(ErrorCode.UserNotFound));
            }
            else
            {
                var user = resp.Data["user"].ToObject<User>();
                result = (user, null);
            }

            return result;
        }
    }
}
