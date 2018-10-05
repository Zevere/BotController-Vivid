using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;

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
            Error error = default;

            ChatBot bot;
            try
            {
                bot = await _botsRepo.GetByNameAsync(botName, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (EntityNotFoundException e)
            {
                _logger?.LogInformation(e, "Chat bot {0} doesn't exists.", botName);
                error = new Error(ErrorCode.BotNotFound);
                bot = null;
            }

            if (bot != null)
            {
                // ToDo: ensure username exists with Zevere GraphQL API
                // ToDo:  if (error == default)

                try
                {
                    await _regsRepo.AddAsync(new Registration
                        {
                            ChatBotId = bot.Id,
                            Username = username,
                            ChatUserId = userId
                        }, cancellationToken
                    ).ConfigureAwait(false);
                }
                catch (DuplicateKeyException e)
                {
                    _logger?.LogInformation(e, "Bot {0} has already registered user {1}.", botName, username);
                    error = new Error(ErrorCode.RegistrationExists);
                }
            }

            return error;
        }
    }
}
