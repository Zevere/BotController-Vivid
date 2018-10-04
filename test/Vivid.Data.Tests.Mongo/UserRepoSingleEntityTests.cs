using System.Threading.Tasks;
using MongoDB.Bson;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Data.Tests.Common;
using Vivid.Data.Tests.Mongo.Framework;
using Vivid.Tests.Framework;
using Xunit;

namespace Vivid.Data.Tests.Mongo
{
    public class UserRepoSingleEntityTests : UserRepoSingleEntityTestsBase,
        IClassFixture<UserRepoSingleEntityTests.Fixture>
    {
        public UserRepoSingleEntityTests(Fixture classFixture)
            : base(classFixture, () => new UserRegistrationRepository(classFixture.Collection))
        {
        }

        [OrderedFact]
        public async Task Should_Add_User_Mongo()
        {
            IUserRegistrationRepository repo = CreateUserRepository();

            User user = new User
            {
                FirstName = "Charles",
                DisplayId = "chuck",
                PassphraseHash = "secret_passphrase"
            };

            var entity = await repo.AddAsync(user);

            Assert.Same(user, entity);
            Assert.True(ObjectId.TryParse(entity.Id, out var _));
            Assert.Equal(17, entity.PassphraseHash.Length);
            Assert.NotEmpty(entity.DisplayId);
            Assert.Null(entity.LastName);
            Assert.False(entity.IsDeleted);
            Assert.Null(entity.ModifiedAt);
        }

        public class Fixture : FixtureBase<User>, IUserRepoSingleEntityFixture
        {
            public User NewUser { get; set; }

            public Fixture()
                : base(MongoConstants.Collections.Users.Name)
            {
            }
        }
    }
}