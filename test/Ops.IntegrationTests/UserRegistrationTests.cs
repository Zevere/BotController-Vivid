using System.Threading.Tasks;
using Framework;
using Ops.IntegrationTests.Shared;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Ops;
using Xunit;

namespace Ops.IntegrationTests
{
    public class UserRegistrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly IChatBotRepository _botsRepo;

        private readonly IUserRegistrationRepository _userRegistrationRepo;

        public UserRegistrationTests(DatabaseFixture fixture)
        {
            _botsRepo = new ChatBotRepository(fixture.ChatBotsCollection);
            _userRegistrationRepo = new UserRegistrationRepository(fixture.RegistrationsCollection);
        }

        [OrderedFact(DisplayName = "Should register a user successfully")]
        public async Task Should_Register_User()
        {
            // add a test bot to the database
            {
                await _botsRepo.AddAsync(new ChatBot
                {
                    Name = "awesome-test-bot",
                });
            }

            IRegistrationService registrationService = new RegistrationService(
                _userRegistrationRepo,
                _botsRepo
            );

            Error error = await registrationService.RegisterUserAsync(
                bot: "awesome-test-bot",
                username: "jsmith",
                chatUserId: "john_smith1"
            );

            Assert.Null(error);
        }
    }
}
