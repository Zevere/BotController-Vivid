using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo.Entities;

namespace Vivid.Data.Mongo
{
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly IMongoCollection<RegistrationMongo> _collection;

        public UserRegistrationRepository(
            IMongoCollection<RegistrationMongo> collection
        )
        {
            _collection = collection;
        }

        public async Task<Registration> AddAsync(
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

            return entity;
        }

        public Task<IEnumerable<Registration>> GetAllForUserAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }
    }
}
