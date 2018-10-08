using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using MongoDB.Bson;
using Ops.IntegrationTests.Shared;
using Vivid.Data;
using Vivid.Data.Entities;
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
                    Platform = "wonder-im"
                });
            }

            IRegistrationService registrationService = new RegistrationService(
                _userRegistrationRepo,
                _botsRepo
            );

            Error error = await registrationService.RegisterUserAsync(
                botName: "awesome-test-bot",
                username: "jsmith",
                chatUserId: "john_smith1"
            );

            Assert.Null(error);
        }

        [OrderedFact(DisplayName = "Should get only 1 registration for user 'jsmith'")]
        public async Task Should_Get_1_User_Registration()
        {
            IRegistrationService registrationService = new RegistrationService(
                _userRegistrationRepo,
                _botsRepo
            );

            (IEnumerable<(Registration, ChatBot)> Registrations, Error Error) result =
                await registrationService.GetUserRegistrationsAsync(
                    username: "jsmith"
                );

            Assert.NotNull(result.Registrations);
            Assert.Null(result.Error);

            // there is only 1 registration for user 'jsmith' so far
            (Registration reg, ChatBot bot) item = Assert.Single(result.Registrations);

            Assert.NotNull(item.bot);
            Assert.NotEmpty(item.bot.Id);
            Assert.True(ObjectId.TryParse(item.bot.Id, out _));
            Assert.Equal("awesome-test-bot", item.bot.Name);
            Assert.Equal("wonder-im", item.bot.Platform);
            Assert.Null(item.bot.Token);
            Assert.Null(item.bot.Url);
            Assert.InRange(
                item.bot.JoinedAt.Ticks,
                DateTime.UtcNow.AddSeconds(-10).Ticks,
                DateTime.UtcNow.Ticks
            );

            // registration document refers to this bot id in 'bots' collection
            string chatBotId = item.bot.Id;

            Assert.NotNull(item.reg);
            Assert.NotEmpty(item.reg.Id);
            Assert.True(ObjectId.TryParse(item.reg.Id, out _));
            Assert.Equal("jsmith", item.reg.Username);
            Assert.Equal(chatBotId, item.reg.ChatBotDbRef.Id);
            Assert.Equal("john_smith1", item.reg.ChatUserId);
            Assert.InRange(
                item.reg.RegisteredAt.Ticks,
                DateTime.UtcNow.AddSeconds(-10).Ticks,
                DateTime.UtcNow.Ticks
            );
        }

        [OrderedFact(DisplayName = "Should throw when registering user 'jsmith' twice using the same bot")]
        public async Task Should_Throw_When_Duplicate_Registration()
        {
            IRegistrationService registrationService = new RegistrationService(
                _userRegistrationRepo,
                _botsRepo
            );

            Error error = await registrationService.RegisterUserAsync(
                botName: "awesome-test-bot",
                username: "jsmith",
                chatUserId: "john_smith1"
            );

            Assert.NotNull(error);
            Assert.Equal("Bot has already registered this user.", error.Message);
            Assert.Equal(ErrorCode.RegistrationExists, error.Code);
            Assert.Null(error.Hint);
        }

        [OrderedFact(DisplayName = "Should delete registration for user 'jsmith' and bot 'awesome-test-bot'")]
        public async Task Should_Delete_Registration()
        {
            IRegistrationService registrationService = new RegistrationService(
                _userRegistrationRepo,
                _botsRepo
            );

            Error error = await registrationService.DeleteUserRegistrationAsync(
                botName: "awesome-test-bot",
                username: "jsmith"
            );

            Assert.Null(error);
        }
    }
}
