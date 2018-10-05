using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Abstractions
{
    public interface IUserRegistrationRepository
    {
        Task<Registration> AddAsync(
            Registration registration,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<Registration>> GetAllForUserAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
