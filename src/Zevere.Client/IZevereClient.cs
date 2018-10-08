using System.Threading;
using System.Threading.Tasks;
using Zevere.Client.GraphQL;

namespace Zevere.Client
{
    public interface IZevereClient
    {
        Task<Response> MakeRequestAsync(
            Request request,
            CancellationToken cancellationToken = default
        );
    }
}
