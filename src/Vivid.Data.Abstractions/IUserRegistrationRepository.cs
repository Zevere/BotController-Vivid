using System.Threading;
using System.Threading.Tasks;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Abstractions
{
    public interface IUserRegistrationRepository
    {
//        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
//
//        Task<TEntity> GetByIdAsync(string id, bool includeDeletedRecords = false,
//            CancellationToken cancellationToken = default);
//
//        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, bool hardDelete = false, CancellationToken cancellationToken = default);
        
        Task<User> GetByNameAsync(string name, bool includeDeletedRecords = false,
            CancellationToken cancellationToken = default);

        Task<User> GetByTokenAsync(string token, bool includeDeletedRecords = false,
            CancellationToken cancellationToken = default);

        Task<User> GetByPassphraseLoginAsync(string userName, string passphrase, bool includeDeletedRecords = false,
            CancellationToken cancellationToken = default);

        Task SetTokenForUserAsync(string userId, string token, CancellationToken cancellationToken = default);

        Task<bool> RevokeTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}