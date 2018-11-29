using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Vivid.Data.Entities;

namespace Vivid.Data
{
    /// <inheritdoc />
    public class ChatBotRepository : IChatBotRepository
    {
        private readonly IMongoCollection<ChatBot> _collection;

        private FilterDefinitionBuilder<ChatBot> Filter => Builders<ChatBot>.Filter;

        /// <inheritdoc />
        public ChatBotRepository(
            IMongoCollection<ChatBot> collection
        )
        {
            _collection = collection;
        }

        /// <inheritdoc />
        public async Task AddAsync(
            ChatBot bot,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                await _collection.InsertOneAsync(bot, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (MongoWriteException e)
                when (
                    e.WriteError.Category == ServerErrorCategory.DuplicateKey &&
                    e.WriteError.Message.Contains($" index: {MongoConstants.Collections.Bots.Indexes.BotId} ")
                )
            {
                throw new DuplicateKeyException(nameof(ChatBot.Name));
            }
        }

        /// <inheritdoc />
        public async Task<ChatBot> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var filter = Filter.Eq(_ => _.Id, id);

            ChatBot bot = await _collection
                .Find(filter)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return bot;
        }

        /// <inheritdoc />
        public async Task<ChatBot> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        )
        {
            name = Regex.Escape(name);
            var filter = Filter.Regex(b => b.Name, new BsonRegularExpression($"^{name}$", "i"));

            ChatBot bot = await _collection
                .Find(filter)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return bot;
        }

        /// <inheritdoc />
        public async Task<ChatBot> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        )
        {
            var filter = Builders<ChatBot>.Filter.Eq(b => b.Token, token);
            var bot = await _collection.Find(filter)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return bot;
        }
    }
}
