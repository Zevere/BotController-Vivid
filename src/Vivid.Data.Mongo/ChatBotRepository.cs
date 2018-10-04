using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Mongo
{
    public class ChatBotRepository : IChatBotRepository
    {
        private readonly IMongoCollection<ChatBot> _collection;

        private FilterDefinitionBuilder<ChatBot> Filter => Builders<ChatBot>.Filter;

        public ChatBotRepository(IMongoCollection<ChatBot> collection)
        {
            _collection = collection;
        }

        public async Task<ChatBot> AddAsync(
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

            return bot;
        }

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

            if (bot is null)
            {
                throw new EntityNotFoundException(nameof(ChatBot.Name), name);
            }

            return bot;
        }

        public async Task<ChatBot> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        )
        {
            var filter = Builders<ChatBot>.Filter.Eq(b => b.Token, token);
            var bot = await _collection.Find(filter)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (bot is null)
            {
                throw new EntityNotFoundException(nameof(ChatBot.Token), token);
            }

            return bot;
        }
    }
}
