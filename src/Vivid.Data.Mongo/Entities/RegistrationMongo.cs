using Vivid.Data.Abstractions.Entities;
using MongoDB.Driver;

namespace Vivid.Data.Mongo.Entities
{
    public class RegistrationMongo : Registration
    {
        public MongoDBRef ChatBotDbRef
        {
            get => _botDbRef;
            private set
            {
                _botDbRef = value;
                ChatBotId = value?.Id.AsString;
            }
        }

        private MongoDBRef _botDbRef;

        public static RegistrationMongo Clone(Registration reg) =>
            new RegistrationMongo
            {
                Id = reg.Id,
                Username = reg.Username,
                ChatBotId = reg.ChatBotId,
                ChatUserId = reg.ChatUserId,
                RegisteredAt = reg.RegisteredAt,
                ChatBotDbRef = new MongoDBRef(MongoConstants.Collections.Bots.Name, reg.ChatBotId),
            };
    }
}
