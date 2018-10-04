using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Data.Mongo
{
    public static class Initializer
    {
        public static async Task CreateSchemaAsync(
            IMongoDatabase database,
            CancellationToken cancellationToken = default
        )
        {
            {
                // "bots" Collection
                await database
                    .CreateCollectionAsync(MongoConstants.Collections.Bots.Name, null, cancellationToken)
                    .ConfigureAwait(false);
                var botsCollection = database.GetCollection<ChatBot>(MongoConstants.Collections.Bots.Name);

                // create unique index "bot_id" on the field "name"
                var key = Builders<ChatBot>.IndexKeys.Ascending(u => u.Name);
                await botsCollection.Indexes.CreateOneAsync(new CreateIndexModel<ChatBot>(
                        key,
                        new CreateIndexOptions
                            { Name = MongoConstants.Collections.Bots.Indexes.BotId, Unique = true }),
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
            }
        }

        public static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ChatBot)))
            {
                BsonClassMap.RegisterClassMap<ChatBot>(map =>
                {
                    map.MapIdProperty(b => b.Id).SetIdGenerator(new StringObjectIdGenerator());
                    map.MapProperty(b => b.Name).SetElementName("name").SetOrder(1);
                    map.MapProperty(u => u.Platform).SetElementName("platform");
                    map.MapProperty(u => u.Url).SetElementName("url");
                    map.MapProperty(u => u.Token).SetElementName("token");
                    map.MapProperty(u => u.JoinedAt).SetElementName("joined");
                });
            }
        }
    }
}
