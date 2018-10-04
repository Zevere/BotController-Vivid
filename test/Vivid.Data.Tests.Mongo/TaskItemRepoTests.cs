using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Data.Mongo.Entities;
using Vivid.Data.Tests.Common;
using Vivid.Data.Tests.Mongo.Framework;
using Xunit;

namespace Vivid.Data.Tests.Mongo
{
    public class TaskItemRepoTests :
        TaskItemRepoTestsBase,
        IClassFixture<TaskItemRepoTests.Fixture>
    {
        public TaskItemRepoTests(Fixture fixture)
            : base(() => new TaskItemRepository(fixture.Collection, fixture.TaskListRepo))
        {
        }

        public class Fixture : FixtureBase<TaskItemMongo>
        {
            public ITaskListRepository TaskListRepo { get; }

            public Fixture()
                : base(MongoConstants.Collections.TaskItems.Name)
            {
                var userRepo = new UserRegistrationRepository(
                    Collection.Database.GetCollection<User>(MongoConstants.Collections.Users.Name)
                );

                TaskListRepo = new TaskListRepository(
                    Collection.Database.GetCollection<TaskListMongo>(MongoConstants.Collections.TaskLists.Name),
                    userRepo
                );

                TestDataSeeder.SeedUsersAsync(userRepo).GetAwaiter().GetResult();

                TaskListRepo.SetUsernameAsync("bobby").GetAwaiter().GetResult();
                TestDataSeeder.SeedTaskListAsync(TaskListRepo).GetAwaiter().GetResult();
            }
        }
    }
}