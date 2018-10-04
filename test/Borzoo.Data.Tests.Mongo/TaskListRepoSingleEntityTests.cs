using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Data.Mongo.Entities;
using Vivid.Data.Tests.Common;
using Vivid.Data.Tests.Mongo.Framework;
using Xunit;

namespace Vivid.Data.Tests.Mongo
{
    public class TaskListRepoSingleEntityTests :
        TaskListRepoSingleEntityTestsBase,
        IClassFixture<TaskListRepoSingleEntityTests.Fixture>
    {
        private readonly Fixture _fixture;

        public TaskListRepoSingleEntityTests(Fixture fixture)
            : base(() => new TaskListRepository(fixture.Collection, fixture.UserRepo))
        {
            _fixture = fixture;
        }

        public class Fixture : FixtureBase<TaskListMongo>
        {
            public IUserRepository UserRepo { get; }

            public Fixture()
                : base(MongoConstants.Collections.TaskLists.Name)
            {
                UserRepo = new UserRepository(
                    Collection.Database.GetCollection<User>(MongoConstants.Collections.Users.Name)
                );
                
                SeedUserDataAsync().GetAwaiter().GetResult();
            }
        }
    }
}