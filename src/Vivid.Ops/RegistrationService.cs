using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Vivid.Data;
using Vivid.Data.Entities;

namespace Vivid.Ops
{
    /// <inheritdoc />
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRegistrationRepository _regsRepo;

        private readonly IChatBotRepository _botsRepo;

        private readonly ILogger _logger;

        /// <inheritdoc />
        public RegistrationService(
            IUserRegistrationRepository regsRepo,
            IChatBotRepository botsRepo,
            ILogger logger = default
        )
        {
            _regsRepo = regsRepo;
            _botsRepo = botsRepo;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<Error> RegisterUserAsync(
            string botName,
            string username,
            string userId,
            CancellationToken cancellationToken = default
        )
        {
            Error error;

            var bot = await _botsRepo.GetByNameAsync(botName, cancellationToken)
                .ConfigureAwait(false);

            if (bot == null)
            {
                _logger?.LogInformation("Chat bot {0} doesn't exists.", botName);
                error = new Error(ErrorCode.BotNotFound);
            }
            else
            {
                // ToDo: ensure username exists with Zevere GraphQL API
                // ToDo:  if (error == default)

                try
                {
                    await _regsRepo.AddAsync(new Registration
                        {
                            ChatBotDbRef = new MongoDBRef(MongoConstants.Collections.Bots.Name, bot.Id),
                            Username = username,
                            ChatUserId = userId
                        }, cancellationToken
                    ).ConfigureAwait(false);

                    error = null;
                }
                catch (DuplicateKeyException e)
                {
                    _logger?.LogInformation(e, "Bot {0} has already registered user {1}.", botName, username);
                    error = new Error(ErrorCode.RegistrationExists, "Bot has already registered this user.");
                }
            }

            return error;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<(Registration, ChatBot)> Registrations, Error Error)> GetUserRegistrationsAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            // ToDo return "not found" error if username doesn't exist

            var regs = (
                await _regsRepo.GetAllForUserAsync(username, cancellationToken)
                    .ConfigureAwait(false)
            ).ToArray();

            if (!regs.Any())
            {
                return (null, new Error(ErrorCode.RegistrationNotFound));
            }

            // ToDo maybe use mongodb functions for this aggregation
            var getBotTasks = regs
                .Select(r => r.ChatBotDbRef.Id.ToString())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(botId => _botsRepo.GetByIdAsync(botId, cancellationToken));

            var bots = await Task.WhenAll(getBotTasks)
                .ConfigureAwait(false);

            var aggregatedRegs = regs
                .Select(r => (r, bots.Single(b => b.Id == r.ChatBotDbRef.Id.ToString())))
                .ToArray();

            return (aggregatedRegs, null);
        }

        /// <inheritdoc />
        public async Task<Error> DeleteUserRegistrationAsync(
            string botName,
            string username,
            CancellationToken cancellationToken = default
        )
        {
            Error error;

            var bot = await _botsRepo.GetByNameAsync(botName, cancellationToken)
                .ConfigureAwait(false);

            if (bot == null)
            {
                _logger?.LogInformation("Chat bot {0} doesn't exists.", botName);
                error = new Error(ErrorCode.BotNotFound);
            }
            else
            {
                var registration = await _regsRepo.GetSingleAsync(bot.Id, username, cancellationToken)
                    .ConfigureAwait(false);

                if (registration == null)
                {
                    _logger?.LogInformation("No registration exists for bot {0} and user {1}.", botName, username);
                    error = new Error(ErrorCode.RegistrationNotFound);
                }
                else
                {
                    await _regsRepo.DeleteAsync(registration, cancellationToken)
                        .ConfigureAwait(false);
                    error = null;
                }
            }

            return error;
        }
    }
}
