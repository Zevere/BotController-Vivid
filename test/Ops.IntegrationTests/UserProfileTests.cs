using System;
using System.Threading.Tasks;
using Framework;
using Ops.IntegrationTests.Shared;
using Vivid.Data;
using Vivid.Data.Entities;
using Vivid.Ops;
using Xunit;
using Zevere.Client;
using Zevere.Client.Models;

namespace Ops.IntegrationTests
{
    [Collection("user profile")]
    public class UserProfileTests : IClassFixture<DatabaseFixture>
    {
        private readonly IChatBotRepository _botsRepo;

        private readonly IUserRegistrationRepository _userRegistrationRepo;

        private readonly IZevereClient _zevereClient;

        public UserProfileTests(DatabaseFixture fixture)
        {
            _botsRepo = new ChatBotRepository(fixture.ChatBotsCollection);
            _userRegistrationRepo = new UserRegistrationRepository(fixture.RegistrationsCollection);
            _zevereClient = new ZevereClient(new Settings().ZevereApiEndpoint);
        }

        [OrderedFact(DisplayName = "Should get a user profile successfully")]
        public async Task Should_Get_Profile()
        {
            {
                // add a test bot to the database
                await _botsRepo.AddAsync(new ChatBot
                {
                    Name = "awesome-test-bot",
                    Platform = "wonder-im"
                });

                IRegistrationService registrationService = new RegistrationService(
                    _userRegistrationRepo,
                    _botsRepo,
                    _zevereClient
                );

                // register the user with the test bot
                Error error = await registrationService.RegisterUserAsync(
                    botName: "awesome-test-bot",
                    username: "jsmith",
                    chatUserId: "john_smith1"
                );
                Assert.Null(error);
            }

            IUserProfileService userProfileService = new UserProfileService(
                _zevereClient
            );

            (User User, Error Error) result = await userProfileService.GetUserAsync(
                username: "jsmith"
            );

            Assert.Null(result.Error?.Message);
            Assert.Null(result.Error);

            User user = result.User;
            Assert.NotNull(user);
            Assert.Equal("jsmith", user.Id);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("11TOKEN11", user.Token);
            Assert.Equal(5, user.DaysJoined);
            Assert.InRange(user.JoinedAt, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-4));
            Assert.Null(user.LastName);
        }
    }
}
