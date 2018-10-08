using System.Threading.Tasks;
using Framework;
using Ops.IntegrationTests.Shared;
using Vivid.Data;
using Vivid.Data.Entities;
using Vivid.Ops;
using Xunit;
using Zevere.Client;

namespace Ops.IntegrationTests
{
    [Collection("user registration errors")]
    public class UserRegistrationFailureTests : IClassFixture<DatabaseFixture>
    {
        private readonly IChatBotRepository _botsRepo;

        private readonly IUserRegistrationRepository _registrationRepo;

        private readonly IZevereClient _zevereClient;

        public UserRegistrationFailureTests(DatabaseFixture fixture)
        {
            _botsRepo = new ChatBotRepository(fixture.ChatBotsCollection);
            _registrationRepo = new UserRegistrationRepository(fixture.RegistrationsCollection);
            // ToDo use Docker Compose to run test dependencies
            _zevereClient = new ZevereClient("https://zevere-staging.herokuapp.com/zv/GraphQL");
        }

        [OrderedFact(DisplayName = "Should throw when creating a user that doesn't exist on Zevere")]
        public async Task Should_Throw_When_Username_Not_Found()
        {
            // add a test bot to the database
            {
                await _botsRepo.AddAsync(new ChatBot
                {
                    Name = "awesome-test-bot",
                    Platform = "wonder-im"
                });
            }

            IRegistrationService registrationService = new RegistrationService(
                _registrationRepo,
                _botsRepo,
                _zevereClient
            );

            Error error = await registrationService.RegisterUserAsync(
                botName: "awesome-test-bot",
                username: "NON-EXISTING_user",
                chatUserId: "whatever_id"
            );

            Assert.NotNull(error);
            Assert.Equal(ErrorCode.UserNotFound, error.Code);
            Assert.Null(error.Message);
            Assert.Null(error.Hint);
        }
    }
}
