using System.Threading;
using System.Threading.Tasks;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Abstractions
{
    public interface IChatBotRepository
    {
        Task<ChatBot> AddAsync(
            ChatBot bot,
            CancellationToken cancellationToken = default
        );

        // ToDo docs
        /// <summary>
        /// Find a chat bot by its name (case-insensitive).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ChatBot> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        Task<ChatBot> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        );
    }
}
