using System.Threading;
using System.Threading.Tasks;

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
    }
}
