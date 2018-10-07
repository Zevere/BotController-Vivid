using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo.Entities;

namespace Vivid.Data.Mongo
{
    /// <inheritdoc />
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private FilterDefinitionBuilder<RegistrationMongo> Filter => Builders<RegistrationMongo>.Filter;

        private readonly IMongoCollection<RegistrationMongo> _collection;

        public UserRegistrationRepository(
            IMongoCollection<RegistrationMongo> collection
        )
        {
            _collection = collection;
        }

        /// <inheritdoc />
        public async Task AddAsync(
            Registration registration,
            CancellationToken cancellationToken = default
        )
        {
            var entity = RegistrationMongo.Clone(registration);

            try
            {
                await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (MongoWriteException e)
                when (e.WriteError.Category == ServerErrorCategory.DuplicateKey &&
                      e.WriteError.Message
                          .Contains($" index: {MongoConstants.Collections.Registrations.Indexes.BotUsername} ")
                )
            {
                throw new DuplicateKeyException(nameof(Registration.ChatBotId), nameof(Registration.Username));
            }
        }

        /// <inheritdoc />
        public async Task<Registration> GetSingleAsync(
            string botId,
            string username,
            CancellationToken cancellationToken = default
        )
        {
            username = Regex.Unescape(username);

            var filter = Filter.And(
                Filter.Eq(r => r.ChatBotDbRef.Id, botId),
                Filter.Regex(reg => reg.Username, new BsonRegularExpression($"^{username}$", "i"))
            );

            Registration registration = await _collection
                .Find(filter)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return registration;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Registration>> GetAllForUserAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            username = Regex.Unescape(username);

            var filter = Filter.Regex(reg => reg.Username, new BsonRegularExpression($"^{username}$", "i"));

            IList<RegistrationMongo> registrations = await _collection
                .Find(filter)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return registrations;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(
            Registration registration,
            CancellationToken cancellationToken = default
        )
        {
            var filter = Filter.Eq(_ => _.Id, registration.Id);

            await _collection
                .FindOneAndDeleteAsync(filter, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
