using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Tests.Common
{
    public interface IUserRepoSingleEntityFixture
    {
        User NewUser { get; set; }
    }
}