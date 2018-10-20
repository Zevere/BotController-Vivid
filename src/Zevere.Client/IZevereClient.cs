using System.Threading;
using System.Threading.Tasks;
using Zevere.Client.GraphQL;

namespace Zevere.Client
{
    /// <summary>
    /// A client for Zevere's GraphQL web API
    /// </summary>
    public interface IZevereClient
    {
        /// <summary>
        /// Make a GraphQL request over HTTP
        /// </summary>
        /// <param name="request">GraphQL request to be sent</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns>The operation response</returns>
        Task<Response> MakeRequestAsync(
            Request request,
            CancellationToken cancellationToken = default
        );
    }
}
